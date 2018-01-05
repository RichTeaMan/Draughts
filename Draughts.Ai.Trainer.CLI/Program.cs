using Draughts.Service;
using NameUtility;
using Newtonsoft.Json;
using RichTea.CommandLineParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Draughts.Ai.Trainer
{
    class Program
    {
        private static bool shouldClose = false;

        static void Main(string[] args)
        {
            MethodInvoker command = null;
            try
            {
                command = new CommandLineParserInvoker().GetCommand(typeof(Program), args);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing command:");
                Console.WriteLine(ex);
            }
            if (command != null)
            {
                try
                {
                    command.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error running command:");
                    Console.WriteLine(ex);

                    var inner = ex.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine(inner);
                        Console.WriteLine();
                        inner = inner.InnerException;
                    }

                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        [ClCommand("train")]
        public static void TrainAi(
            [ClArgs("generation-count", "gc")]
            int generationCount = 20,
            [ClArgs("iteration-count", "ic")]
            int iterationCount = 100,
            [ClArgs("ai-type")]
            string aiTypeStr = "weighted",
            [ClArgs("threads")]
            int? threads = null,
            [ClArgs("seed")]
            int? seed = null
            )
        {
            Console.WriteLine("Training draughts AI.");
            Console.WriteLine($"Generation count: {generationCount}");
            Console.WriteLine($"Iteration count: {iterationCount}");
            if (!threads.HasValue)
            {
                threads = Environment.ProcessorCount;
            }
            Console.WriteLine($"Thread count: {threads}");
            Random random;
            if (seed.HasValue)
            {
                Console.WriteLine($"Random seed: {seed}");
                random = new Random(seed.Value);
            }
            else
            {
                random = new Random();
            }
            AiType aiType;
            switch(aiTypeStr.ToLower())
            {
                case "weighted":
                    aiType = AiType.Weighted;
                    
                    break;
                case "neuralnet":
                    aiType = AiType.NeuralNet;
                    break;
                default:
                    throw new Exception($"Unsupported AI type '{aiTypeStr}'.");
            }

            Console.CancelKeyPress += Console_CancelKeyPress;

            IAiGamePlayerSpawner spawner= null;
            var contestants = new List<Contestant>();
            switch (aiType)
            {
                case AiType.Weighted:
                    spawner = new WeightedAiGamePlayerSpawner(random);
                    Console.WriteLine("Using weighted AI.");
                    break;
                case AiType.NeuralNet:
                    spawner = new NeuralNetAiGamePlayerSpawner(random);
                    Console.WriteLine("Using neural net AI.");
                    break;
            }

            contestants.AddRange(Enumerable.Range(0, generationCount)
                .Select(i => new Contestant(
                    spawner.SpawnAiGamePlayer())
                ));

            foreach (var i in Enumerable.Range(0, iterationCount).Where(g => !shouldClose && !shouldClose))
            {
                int gamesPlayed = 0;
                int gamesDrawn = 0;

                Console.WriteLine($"Iteration {i} underway.");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                ParallelOptions parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = threads.Value
                };
                Parallel.ForEach(contestants, parallelOptions,(contestant, loopState) =>
                {
                    foreach (var opponent in contestants.Where(c => c != contestant && !shouldClose).ToList())
                    {
                        if (shouldClose)
                        {
                            loopState.Stop();
                            break;
                        }
                        var gameMatch = new GameMatch(
                            GameStateFactory.StandardStartGameState(),
                            contestant.GamePlayer,
                            opponent.GamePlayer);

                        var matchResult = gameMatch.CompleteMatch();

                        var _count = Interlocked.Increment(ref gamesPlayed);
                        if (_count % 100 == 0)
                        {
                            PrintStatus(gamesDrawn, stopwatch.Elapsed.TotalSeconds, _count);
                        }

                        int uniqueGameStateCount = gameMatch.GameStateList.Distinct().Count();

                        contestant.IncrementMatch();
                        contestant.AddUniqueGameStates(uniqueGameStateCount);
                        opponent.IncrementMatch();
                        opponent.AddUniqueGameStates(uniqueGameStateCount);
                        if (matchResult == GameMatchOutcome.WhiteWin)
                        {
                            contestant.IncrementWin();
                        }
                        else if (matchResult == GameMatchOutcome.BlackWin)
                        {
                            opponent.IncrementWin();
                        }
                        else if (matchResult == GameMatchOutcome.Draw)
                        {
                            contestant.IncrementDraw();
                            opponent.IncrementDraw();
                            Interlocked.Increment(ref gamesDrawn);
                        }
                    }
                });
                stopwatch.Stop();
                PrintStatus(gamesDrawn, stopwatch.Elapsed.TotalSeconds, gamesPlayed);

                if (shouldClose)
                {
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Matches complete.");

                var orderedContestants = contestants.OrderByDescending(c => c.Wins).ThenBy(c => c.UniqueGameStates).ToList();
                var json = new ContestantSerialiser().SerialiseContestants(orderedContestants);
                System.IO.File.WriteAllText($"Iteration{i}.json", json);

                Console.WriteLine("Contestants saved.");

                var winningContestant = orderedContestants.First();

                for (int topContestantIndex = 0; topContestantIndex < orderedContestants.Count(); topContestantIndex++)
                {
                    var contestant = orderedContestants[topContestantIndex];
                    Console.WriteLine($"Rank: {topContestantIndex + 1}. Name: {contestant.GamePlayer.GenerateName()}. Net Name: {(contestant.GamePlayer as NeuralNetAiGamePlayer).Net.CreateSerialisedNet().GenerateName()}. Wins: {contestant.Wins}. Losses: {contestant.Matches - contestant.Wins}. Draws: {contestant.Draws}. States: {contestant.UniqueGameStates}.");
                }

                int testGameCount = 50;
                int randomWins = PlayGames(winningContestant.GamePlayer, new RandomGamePlayer(), testGameCount);
                if (shouldClose)
                {
                    return;
                }
                Console.WriteLine($"Best contestant beat random AI {randomWins} out {testGameCount} games.");

                var nextContestants = new List<Contestant>();
                foreach (var contestantI in orderedContestants.Take(generationCount / 2))
                {
                    var spawnContestant = new Contestant(spawner.SpawnDerivedAiGamePlayer(contestantI.GamePlayer));
                    nextContestants.Add(spawnContestant);
                    nextContestants.Add(contestantI);
                    contestantI.ResetStats();
                }
                contestants = nextContestants;
            }

            Console.WriteLine($"Training complete.");
            return;
        }

        private static void PrintStatus(int gamesDrawn, double totalSeconds, int _count)
        {
            var gamesPerSecond = _count / totalSeconds;
            double percentageDrawn = ((double)gamesDrawn / _count) * 100;
            Console.Write($"\r{_count} games played. {percentageDrawn:F0}% games drawn. Processing {gamesPerSecond:F2} games per second.");
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Trainer stopping...");
            shouldClose = true;
        }

        private static int PlayGames(IGamePlayer contestant, IGamePlayer opponent, int gameCount)
        {
            int wins = 0;
            foreach (var randomGameI in Enumerable.Range(0, gameCount).Where(g => !shouldClose))
            {
                var gameMatch = new GameMatch(
                    GameStateFactory.StandardStartGameState(),
                    contestant,
                    opponent);
                var outcome = gameMatch.CompleteMatch();
                if (outcome == GameMatchOutcome.WhiteWin)
                {
                    wins++;
                }
            }
            return wins;
        }
    }
}
