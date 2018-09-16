using Draughts.Service;
using Newtonsoft.Json;
using RichTea.CommandLineParser;
using RichTea.NeuralNetLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Draughts.Ai.Trainer
{
    internal class Program
    {
        private static bool shouldClose = false;

        private static void Main(string[] args)
        {
            Console.WriteLine("Arguments:");
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }


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
            string aiTypeStr = "neuralnet",
            [ClArgs("threads")]
            int? threads = null,
            [ClArgs("seed")]
            int? seed = null,
            [ClArgs("contestant-file", "cf")]
            string contestantFile = null,
            [ClArgs("output-path", "op")]
            string outputPath = null
            )
        {
            switch (aiTypeStr.ToLower())
            {
                case "neuralnet":
                    TrainNeuralNet(generationCount, iterationCount, threads, seed, contestantFile, outputPath);
                    break;
                default:
                    throw new Exception($"Unsupported AI type '{aiTypeStr}'.");
            }
        }

        public static void TrainNeuralNet(
            int generationCount,
            int iterationCount,
            int? threads,
            int? seed,
            string contestantFile,
            string outputPath
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

            Console.CancelKeyPress += Console_CancelKeyPress;

            var trainer = new GeneticAlgorithmTrainer<AiGamePlayerFitnessEvaluator>(random, new AiGamePlayerFitnessEvaluator(random));

            trainer.IterationStarted += (sender, args) =>
            {
                Console.WriteLine($"Iteration {args.TrainingStatus.CurrentIteration} underway.");
            };

            trainer.IterationInProgress += (sender, args) =>
            {
                if (args.TrainingStatus.GenerationEvaluations % 10 == 0)
                {
                    PrintStatus(sender.FitnessEvaluator.GamesDrawn, args.TrainingStatus.GenerationTimeSpan.TotalSeconds, sender.FitnessEvaluator.GamesPlayed);
                }
            };

            trainer.IterationComplete += (sender, args) =>
            {
                PrintStatus(sender.FitnessEvaluator.GamesDrawn, args.TrainingStatus.GenerationTimeSpan.TotalSeconds, sender.FitnessEvaluator.GamesPlayed);

                Console.WriteLine();
                Console.WriteLine($"{sender.FitnessEvaluator.GamesPlayed} matches complete.");
                Console.WriteLine($"{args.EvaluatedNets.Count} nets evaulated.");

                var orderedContestants = args.EvaluatedNets.OrderByDescending(n => n.FitnessScore).Select(n => new { Score = n.FitnessScore, Contestant = sender.FitnessEvaluator.NetPlayerLookup[n.Net] }).ToList();
                var json = new ContestantSerialiser().SerialiseContestants(orderedContestants.Select(c => c.Contestant).ToList());
                Directory.CreateDirectory("AiOutput");
                File.WriteAllText($"AiOutput/Iteration{args.TrainingStatus.CurrentIteration}.json", json);

                if (!string.IsNullOrEmpty(outputPath))
                {
                    Console.WriteLine($"Writing to '{outputPath}'.");
                    File.WriteAllText(outputPath, json);
                }

                Console.WriteLine("Contestants saved.");

                for (int topContestantIndex = 0; topContestantIndex < orderedContestants.Count(); topContestantIndex++)
                {
                    var scoredContestant = orderedContestants[topContestantIndex];
                    var contestant = scoredContestant.Contestant;
                    Console.WriteLine($"Rank: {topContestantIndex + 1}. Score: {scoredContestant.Score}. Name: {contestant.GamePlayer.Name} Wins: {contestant.Wins}. Losses: {contestant.Matches - (contestant.Wins + contestant.Draws)}. Draws: {contestant.Draws}. Random wins: {contestant.RandomWins}. States: {contestant.UniqueGameStates}.");
                }
                Console.WriteLine($"Distincts: {orderedContestants.Distinct().Count()} White wins: {sender.FitnessEvaluator.WhiteWins} Black wins: {sender.FitnessEvaluator.BlackWins}");

                sender.FitnessEvaluator.ResetStats();
            };

            trainer.NetsSpawned += (sender, args) =>
            {
                sender.FitnessEvaluator.RebuildPlayers(args.Nets);

                if (null != args.NeuralNetMutator)
                {
                    Console.WriteLine($"Nets generated with mutator: '{args.NeuralNetMutator.GetType().Name}'");
                }
            };

            List<Net> contestants = new List<Net>();
            if (contestantFile != null)
            {
                if (File.Exists(contestantFile))
                {
                    var fileContents = File.ReadAllText(contestantFile);
                    var players = JsonConvert.DeserializeObject<object[]>(fileContents, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    var ais = players
                        .Where(p => p is SerialisableNeuralNetAiGamePlayer)
                        .Cast<SerialisableNeuralNetAiGamePlayer>()
                        .Select(player => player.CreateNeuralNetAiGamePlayer())
                        .ToList()
                        .Distinct()
                        .ToList();

                    var netList = ais.Select(ai => ai.Net).ToList();
                    Console.WriteLine($"Loading {ais.Count()} unique contestants from {contestantFile}.");
                    ais.ForEach(ai => Console.WriteLine($"Loading {ai.Name}."));

                    // resize nets for contestants
                    var resizer = new RichTea.NeuralNetLib.Resizers.RandomInputResizer();

                    var resizedNets = netList.Select(n => resizer.ResizeInputs(n, NeuralNetAiGamePlayer.NetInputs)).ToList();

                    contestants.AddRange(resizedNets);

                }
                else
                {
                    Console.WriteLine("Could not load contestant file. Generating contestants instead.");
                }
            }

            if (contestants.Count < generationCount)
            {
                var contestantsToGenerate = generationCount - contestants.Count;
                Console.WriteLine($"Generating {contestantsToGenerate} contestants.");
                contestants.AddRange(new NetFactory().GenerateRandomNetList(NeuralNetAiGamePlayer.NetInputs, 1, 3, random, generationCount - contestants.Count));
            }

            trainer.TrainAi(contestants, generationCount, iterationCount);
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
