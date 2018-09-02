using Draughts.Service;
using RichTea.NeuralNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Draughts.Ai.Trainer
{
    public class AiGamePlayerFitnessEvaluator : IFitnessEvaluator
    {
        private int _gamesPlayed;
        private int _gamesDrawn;

        private int _whiteWins = 0;
        private int _blackWins = 0;

        public int WinWeight { get; set; } = 1000;
        public int DrawWeight { get; set; } = -500;
        public int GenerationWeight { get; set; } = 10;
        public int RandomWinWeight { get; set; } = 1000;
        public int UniqueGameStateWeight { get; set; } = 10;
        public int RandomGamesToPlay { get; set; } = 500;

        public int GamesPlayed { get { return _gamesPlayed; } }
        public int GamesDrawn { get { return _gamesDrawn; } }

        public int WhiteWins { get { return _whiteWins; } }
        public int BlackWins { get { return _blackWins; } }

        public Dictionary<Net, Contestant> NetPlayerLookup { get; private set; } = new Dictionary<Net, Contestant>();

        private readonly Random _random;

        public AiGamePlayerFitnessEvaluator(Random random)
        {
            _random = random;
        }

        public int EvaluateNet(IReadOnlyList<Net> competingNets, Net evaluatingNet, TrainingStatus trainingStatus)
        {
            var currentPlayer = NetPlayerLookup[evaluatingNet];
            foreach (var opponentNet in competingNets)
            {
                var opponent = NetPlayerLookup[opponentNet];

                var gameMatch = new GameMatch(
                    GameStateFactory.StandardStartGameState(),
                    currentPlayer.GamePlayer,
                    opponent.GamePlayer
                    );

                var matchResult = gameMatch.CompleteMatch();

                Interlocked.Increment(ref _gamesPlayed);
                currentPlayer.IncrementMatch();
                opponent.IncrementMatch();
                int uniqueGameStateCount = gameMatch.GameStateList.Distinct().Count();
                currentPlayer.AddUniqueGameStates(uniqueGameStateCount);

                if (matchResult == GameMatchOutcome.WhiteWin)
                {
                    currentPlayer.IncrementWin();
                    Interlocked.Increment(ref _whiteWins);
                }
                else if (matchResult == GameMatchOutcome.BlackWin)
                {
                    Interlocked.Increment(ref _blackWins);
                }
                else if (matchResult == GameMatchOutcome.Draw)
                {
                    currentPlayer.IncrementDraw();
                    opponent.IncrementDraw();
                    Interlocked.Increment(ref _gamesDrawn);
                }
            }


            int winScore = currentPlayer.Wins * WinWeight;
            int drawScore = currentPlayer.Draws * DrawWeight;

            var randomGamePlayer = new RandomGamePlayer(_random);
            int randomWins = 0;

            foreach (var i in Enumerable.Range(0, RandomGamesToPlay))
            {
                GameMatch gameMatch;
                GameMatchOutcome winOutcome;
                if (i % 2 == 0)
                {
                    gameMatch = new GameMatch(
                                        GameStateFactory.StandardStartGameState(),
                                        currentPlayer.GamePlayer,
                                        randomGamePlayer);
                    winOutcome = GameMatchOutcome.WhiteWin;
                }
                else
                {
                    gameMatch = new GameMatch(
                                        GameStateFactory.StandardStartGameState(),
                                        randomGamePlayer,
                                        currentPlayer.GamePlayer);
                    winOutcome = GameMatchOutcome.BlackWin;
                }
                var outcome = gameMatch.CompleteMatch();
                if (outcome == winOutcome)
                {
                    randomWins++;
                }
            }

            int randomScore = randomWins * RandomWinWeight;

            int score = winScore + drawScore + randomScore;
            return score;
        }

        public void RebuildPlayers(IEnumerable<Net> nets)
        {
            NetPlayerLookup = new Dictionary<Net, Contestant>();
            foreach (var net in nets)
            {
                var gamePlayer = new NeuralNetAiGamePlayer(net, 0);
                NetPlayerLookup.Add(net, new Contestant(gamePlayer));
            }
        }

        public void ResetStats()
        {
            _gamesDrawn = 0;
            _gamesPlayed = 0;
            _whiteWins = 0;
            _blackWins = 0;
        }
    }
}
