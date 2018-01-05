using Draughts.Service;
using NameUtility;
using Newtonsoft.Json;
using RichTea.Common;
using RichTea.NeuralNetLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Ai.Trainer
{
    public class NeuralNetAiGamePlayer : IAiGamePlayer
    {
        public const int NetInputs = 7;

        public Net Net { get; }

        public int Generation { get; set; }

        [JsonIgnore]
        public string Name
        {
            get { return this.GenerateName(); }
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
            var currentMoves = gameState.CalculateAvailableMoves(pieceColour).ToList();
            if (!currentMoves.Any())
            {
                return new GamePlayerMoveResult(null, MoveStatus.NoLegalMoves);
            }

            var moveWeights = new List<Tuple<double, GameMove>>();
            foreach (var move in currentMoves)
            {
                var moveMetric = move.CalculateGameMoveMetrics(pieceColour);

                var inputs = new double[] {
                moveMetric.CreatedFriendlyKings,
                moveMetric.FriendlyMovesAvailable,
                moveMetric.OpponentMovesAvailable,
                moveMetric.NextMoveFriendlyPiecesAtRisk,
                moveMetric.NextMoveOpponentPiecesAtRisk,
                moveMetric.NextMoveFriendlyKingsCreated,
                moveMetric.NextMoveOpponentKingsCreated
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
                .Append(p => p.Net)
                .Append(p => p.Generation)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<NeuralNetAiGamePlayer>(this, that)
                .Append(p => p.Net)
                .Append(p => p.Generation)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<NeuralNetAiGamePlayer>(this)
                .Append(Net)
                .Append(Generation)
                .HashCode;
        }

    }
}
