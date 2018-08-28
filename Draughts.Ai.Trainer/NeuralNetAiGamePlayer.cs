using Draughts.Service;
using Newtonsoft.Json;
using RichTea.Common;
using RichTea.NeuralNetLib;
using RichTea.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Ai.Trainer
{
    public class NeuralNetAiGamePlayer : IAiGamePlayer
    {
        public const int NetInputs = 20;

        public Net Net { get; }

        public int Generation { get; set; }

        [JsonIgnore]
        public string Name
        {
            get { return NameUtility.GenerateName(Net.CreateSerialisedNet()); }
        }

        public NeuralNetAiGamePlayer(
            Net net,
            int generation)
        {
            Net = net;
            Generation = generation;
        }

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            // build list of previous game states
            var previousGameStates = new HashSet<GameState>();
            GameState lastGameState = gameState;
            while ((lastGameState = lastGameState.PreviousGameState) != null)
            {
                previousGameStates.Add(lastGameState);
            }

            var currentMoves = gameState.CalculateAvailableMoves(pieceColour).ToList();
            if (!currentMoves.Any())
            {
                return new GamePlayerMoveResult(null, MoveStatus.NoLegalMoves);
            }

            var distinctMoves = currentMoves.Where(gm => !previousGameStates.Contains(gm.GameState)).ToList();
            if (!distinctMoves.Any())
            {
                return new GamePlayerMoveResult(null, MoveStatus.Resign);
            }

            var moveWeights = new List<Tuple<double, GameMove>>();
            foreach (var move in distinctMoves)
            {
                var moveMetric = move.CalculateGameMoveMetrics(pieceColour);

                var inputs = new double[] {
                    moveMetric.CreatedFriendlyKings,
                    moveMetric.FriendlyMovesAvailable,
                    moveMetric.OpponentMovesAvailable,
                    moveMetric.NextMoveFriendlyPiecesAtRisk,
                    moveMetric.NextMoveOpponentPiecesAtRisk,
                    moveMetric.NextMoveFriendlyKingsCreated,
                    moveMetric.NextMoveOpponentKingsCreated,
                    moveMetric.TotalPieces / 100.0,
                    moveMetric.TotalFriendlyPieces / 100.0,
                    moveMetric.TotalOpponentPieces / 100.0,
                    moveMetric.TotalMinionPieces / 100.0,
                    moveMetric.TotalFriendlyMinionPieces / 100.0,
                    moveMetric.TotalOpponentMinionPieces / 100.0,
                    moveMetric.TotalKingPieces / 100.0,
                    moveMetric.TotalFriendlyKingPieces / 100.0,
                    moveMetric.TotalOpponentKingPieces / 100.0,
                    moveMetric.FriendlyMinionsHome / 100.0,
                    moveMetric.OpponentMinionsHome / 100.0,
                    moveMetric.FriendlyMinionsAway / 100.0,
                    moveMetric.OpponentMinionsAway / 100.0
                };

                double weightedResult = Net.Calculate(inputs).Single();

                var resultTuple = new Tuple<double, GameMove>(weightedResult, move);
                moveWeights.Add(resultTuple);
            }

            var selectedMove = moveWeights.OrderByDescending(m => m.Item1).First().Item2;
            var selectedGameState = selectedMove.PerformMove();
            var result = new GamePlayerMoveResult(selectedGameState, MoveStatus.SuccessfulMove);
            return result;
        }

        public object CreateObjectForSerialisation()
        {
            var serialisablePlayer = new SerialisableNeuralNetAiGamePlayer
            {
                Generation = Generation,
                Net = Net.CreateSerialisedNet()
            };
            return serialisablePlayer;
        }

        public override string ToString()
        {
            return new ToStringBuilder<NeuralNetAiGamePlayer>(this)
                .Append(p => p.Net.CreateSerialisedNet())
                .Append(p => p.Generation)
                .ToString();
        }

        public override bool Equals(object that)
        {

            var _that = that as NeuralNetAiGamePlayer;
            var thisNet = Net.CreateSerialisedNet();
            var thatNet = _that?.Net.CreateSerialisedNet();
            return new EqualsBuilder<NeuralNetAiGamePlayer>(this, that)
                .Append(thisNet, thatNet)
                .Append(p => p.Generation)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<NeuralNetAiGamePlayer>(this)
                .Append(Net.CreateSerialisedNet())
                .Append(Generation)
                .HashCode;
        }

    }
}
