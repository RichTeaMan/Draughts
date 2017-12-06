﻿using Draughts.Service;
using Newtonsoft.Json;
using RichTea.CommandLineParser;
using System;
using System.Collections.Generic;
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
            int iterationCount = 100
            )
        {
            Console.WriteLine("Training draughts AI.");
            Console.WriteLine($"Generation count: {generationCount}");
            Console.WriteLine($"Iteration count: {iterationCount}");
            Console.CancelKeyPress += Console_CancelKeyPress;
            Random random = new Random();
            var spawner = new WeightedAiGamePlayerSpawner();
            var contestants = Enumerable.Range(0, generationCount)
                .Select(i => new Contestant<WeightedAiGamePlayer>(
                    spawner.SpawnNewWeightedAiGamePlayer())
                ).ToList();

            foreach (var i in Enumerable.Range(0, iterationCount).Where(g => !shouldClose && !shouldClose))
            {
                int gamesPlayed = 0;
                int gamesDrawn = 0;

                Console.WriteLine($"Iteration {i} underway.");

                ParallelOptions parallelOptions = new ParallelOptions();
                Parallel.ForEach(contestants, (contestant, loopState) =>
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
                            Console.WriteLine($"{_count} games played. {gamesDrawn} games drawn.");
                        }

                        contestant.IncrementMatch();
                        opponent.IncrementMatch();
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

                if (shouldClose)
                {
                    return;
                }

                Console.WriteLine("Matches complete.");

                var json = JsonConvert.SerializeObject(contestants.OrderByDescending(c => c.Wins).ToArray());
                System.IO.File.WriteAllText($"Iteration{i}.json", json);

                Console.WriteLine("Contestants saved.");

                var winningContestant = contestants.OrderByDescending(c => c.Wins).First();

                Console.WriteLine($"Top winner won {winningContestant.Wins} matches.");

                var nextContestants = new List<Contestant<WeightedAiGamePlayer>>();
                foreach (var contestantI in contestants.OrderByDescending(c => c.Wins).Take(generationCount / 2))
                {
                    var spawnContestant = new Contestant<Service.WeightedAiGamePlayer>(spawner.SpawnNewWeightedAiGamePlayer(winningContestant.GamePlayer));
                    nextContestants.Add(spawnContestant);
                    nextContestants.Add(contestantI);
                    contestantI.ResetStats();
                }
                contestants = nextContestants;

                int testGameCount = 50;
                int randomWins = PlayGames(winningContestant.GamePlayer, new RandomGamePlayer(), testGameCount);
                if (shouldClose)
                {
                    return;
                }
                Console.WriteLine($"Best contestant beat random AI {randomWins} out {testGameCount} games.");
                int manualWins = PlayGames(winningContestant.GamePlayer, spawner.SpawnManualWeightedAiGamePlayer(), testGameCount);
                if (shouldClose)
                {
                    return;
                }
                Console.WriteLine($"Best contestant beat manually tuned AI {manualWins} out {testGameCount} games.");
            }

            contestants = contestants.OrderBy(c => c.Draws).ThenByDescending(c => c.Wins).ToList();

            var champion = contestants.First();

            Console.WriteLine($"Training complete. Top winner won {champion.Wins} matches. It is generation {champion.GamePlayer.Generation}.");
            return;
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
