using Draughts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Ai.Trainer
{
    public class AiGamePlayerFitnessEvaluator
    {
        public int WinWeight { get; set; } = 1000;
        public int DrawWeight { get; set; } = -50;
        public int GenerationWeight { get; set; } = 10;
        public int RandomWinWeight { get; set; } = 100;
        public int UniqueGameStateWeight { get; set; } = 10;

        public int RandomGamePlayed { get; set; } = 50;

        private Random _random;

        public AiGamePlayerFitnessEvaluator(Random random)
        {
            _random = random;
        }

        public int EvaluateGamePlayer(Contestant contestant)
        {
            int winScore = contestant.Wins * WinWeight;
            int drawScore = contestant.Draws * DrawWeight;
            int generationWeight = contestant.GamePlayer.Generation * GenerationWeight;

            var randomGamePlayer = new RandomGamePlayer(_random);
            int randomWins = 0;

            foreach(var i in Enumerable.Range(0, RandomGamePlayed))
            {
                var gameMatch = new GameMatch(
                    GameStateFactory.StandardStartGameState(),
                    contestant.GamePlayer,
                    randomGamePlayer);
                var outcome = gameMatch.CompleteMatch();
                if (outcome == GameMatchOutcome.WhiteWin)
                {
                    randomWins++;
                }
            }

            int randomScore = randomWins * RandomWinWeight;

            int score = winScore * drawScore * generationWeight * randomScore;
            return score;
        }
    }
}
