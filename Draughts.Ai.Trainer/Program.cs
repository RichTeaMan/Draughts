using Draughts.Service;
using Newtonsoft.Json;
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
        private const int generationCount = 40;

        private const int iterationCount = 10;

        static void Main(string[] args)
        {
            Random random = new Random();
            var spawner = new WeightedAiGamePlayerSpawner();
            var contestants = Enumerable.Range(0, generationCount)
                .Select(i => new Contestant<WeightedAiGamePlayer>(
                    spawner.SpawnNewWeightedAiGamePlayer())
                ).ToList();

            foreach (var i in Enumerable.Range(0, iterationCount))
            {
                int gamesPlayed = 0;
                int gamesDrawn = 0;

                Console.WriteLine($"Iteration {i} underway.");

                Parallel.ForEach(contestants, contestant =>
                {
                    foreach (var opponent in contestants.Where(c => c != contestant).ToList())
                    {
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

                Console.WriteLine("Matches complete.");

                var json = JsonConvert.SerializeObject(contestants.OrderByDescending(c => c.Wins).ToArray());
                System.IO.File.WriteAllText($"Iteration{i}.json", json);

                Console.WriteLine("Contestants saved.");

                var winningContestant = contestants.OrderByDescending(c => c.Wins).First();

                Console.WriteLine($"Top winner won {winningContestant.Wins} matches.");

                var nextContestants = new List<Contestant<WeightedAiGamePlayer>>();
                foreach (var contestantI in contestants.OrderByDescending(c => c.Wins).Take(generationCount / 2))
                {
                    var spawnContestant = new Contestant<WeightedAiGamePlayer>(spawner.SpawnNewWeightedAiGamePlayer(winningContestant.GamePlayer));
                    nextContestants.Add(spawnContestant);
                    nextContestants.Add(contestantI);
                    contestantI.ResetStats();
                }
                contestants = nextContestants;

                int wins = 0;
                int games = 50;
                foreach (var randomGameI in Enumerable.Range(0, games))
                {
                    var gameMatch = new GameMatch(
                        GameStateFactory.StandardStartGameState(),
                        winningContestant.GamePlayer,
                        new RandomGamePlayer());
                    var outcome = gameMatch.CompleteMatch();
                    if (outcome == GameMatchOutcome.WhiteWin)
                    {
                        wins++;
                    }
                }
                Console.WriteLine($"Best contestant beat random AI {wins} out {games} games.");
            }

            contestants = contestants.OrderBy(c => c.Draws).ThenByDescending(c => c.Wins).ToList();

            var champion = contestants.First();

            Console.WriteLine($"Training complete. Top winner won {champion.Wins} matches. It is generation {champion.GamePlayer.Generation}.");
        }
    }
}
