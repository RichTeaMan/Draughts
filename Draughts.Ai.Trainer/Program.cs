using Draughts.Service;
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
        private const int generationCount = 20;
        
        private const int iterationCount = 10;

        static void Main(string[] args)
        {
            Random random = new Random();
            var contestants = Enumerable.Range(0, generationCount)
                .Select(i => new Contestant<WeightedAiGamePlayer>(
                    new WeightedAiGamePlayer(random))
                ).ToList();

            var gameMatch = new GameMatch();
            int gamesPlayed = 0;
            int gamesDrawn = 0;
            foreach(var i in Enumerable.Range(0, iterationCount)) {
                
                Console.WriteLine($"Iteration {i} underway.");

                Parallel.ForEach(contestants, contestant =>
                {
                    foreach (var opponent in contestants.Where(c => c != contestant).ToList())
                    {

                        var matchResult = gameMatch.CompleteMatch(
                            GameStateFactory.StandardStartGameState(),
                            contestant.GamePlayer,
                            opponent.GamePlayer);

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

                var winningContestants = contestants.OrderByDescending(c => c.Wins).Take(generationCount / 2).ToList();
                
                Console.WriteLine($"Top winner won {winningContestants.First().Wins} matches.");

                var nextContestants = new List<Contestant<WeightedAiGamePlayer>>();
                foreach(var contestant in winningContestants)
                {
                    contestant.ResetStats();
                    nextContestants.Add(contestant);
                    var spawnContestant = new Contestant<WeightedAiGamePlayer>(contestant.GamePlayer.SpawnNewWeightedAiGamePlayer());
                    nextContestants.Add(spawnContestant);
                }
                contestants = nextContestants;

                var bestContestant = contestants.OrderBy(c => c.Draws).ThenByDescending(c => c.Wins).First();
                int wins = 0;
                int games = 50;
                foreach (var randomGameI in Enumerable.Range(0, games)) {
                    var outcome = gameMatch.CompleteMatch(GameStateFactory.StandardStartGameState(), bestContestant.GamePlayer, new RandomGamePlayer());
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
            Console.ReadLine();
        }
    }
}
