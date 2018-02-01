using Draughts.Service;
using RichTea.NeuralNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Draughts.Ai.Trainer
{
    public class AiGamePlayerFitnessEvaluator : IFitnessEvaluator
    {
        private int _gamesPlayed;
        private int _gamesDrawn;

        public int WinWeight { get; set; } = 1000;
        public int DrawWeight { get; set; } = -50;
        public int GenerationWeight { get; set; } = 10;
        public int RandomWinWeight { get; set; } = 100;
        public int UniqueGameStateWeight { get; set; } = 10;
        public int RandomGamePlayedWeight { get; set; } = 50;

        public int GamesPlayed { get { return _gamesPlayed; } }
        public int GamesDrawn { get { return _gamesDrawn; } }

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

            foreach (var i in Enumerable.Range(0, RandomGamePlayedWeight))
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

        public int EvaluateNet(IReadOnlyList<Net> competingNets, Net evaluatingNet, TrainingStatus trainingStatus)
        {
            int wins = 0;
            int draws = 0;
            var currentPlayer = new NeuralNetAiGamePlayer(evaluatingNet, 0);
            foreach (var opponentNet in competingNets)
            {
                var opponent = new NeuralNetAiGamePlayer(opponentNet, 0);

                var gameMatch = new GameMatch(
                    GameStateFactory.StandardStartGameState(),
                    currentPlayer,
                    opponent
                    );

                var matchResult = gameMatch.CompleteMatch();

                Interlocked.Increment(ref _gamesPlayed);
                //int uniqueGameStateCount = gameMatch.GameStateList.Distinct().Count();

                if (matchResult == GameMatchOutcome.WhiteWin)
                {
                    wins++;
                }
                else if (matchResult == GameMatchOutcome.BlackWin)
                {

                }
                else if (matchResult == GameMatchOutcome.Draw)
                {
                    draws++;
                    Interlocked.Increment(ref _gamesDrawn);
                }
            }


            int winScore = wins * WinWeight;
            int drawScore = draws * DrawWeight;
            //int generationWeight = contestant.GamePlayer.Generation * GenerationWeight;

            var randomGamePlayer = new RandomGamePlayer(_random);
            int randomWins = 0;

            foreach (var i in Enumerable.Range(0, RandomGamePlayedWeight))
            {
                var gameMatch = new GameMatch(
                    GameStateFactory.StandardStartGameState(),
                    currentPlayer,
                    randomGamePlayer);
                var outcome = gameMatch.CompleteMatch();
                if (outcome == GameMatchOutcome.WhiteWin)
                {
                    randomWins++;
                }
            }

            int randomScore = randomWins * RandomWinWeight;

            int score = winScore * drawScore * randomScore;
            return score;
        }

        public void ResetStats()
        {
            _gamesDrawn = 0;
            _gamesPlayed = 0;
        }
    }
}
