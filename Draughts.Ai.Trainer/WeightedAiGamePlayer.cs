using Draughts.Service;
using Newtonsoft.Json;
using RichTea.Common;
using RichTea.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Ai.Trainer
{
    public class WeightedAiGamePlayer : IAiGamePlayer
    {
        public double NextAvailableMoveCountWeight { get; set; }

        public double OpponentNextAvailableMoveCountWeight { get; set; }

        public double NextMovePiecesAtRiskWeight { get; set; }

        public double NextMovePiecesToTakeWeight { get; set; }

        public double KingWeight { get; set; }

        public double NextMoveKingWeight { get; set; }

        public double OpponentNextMoveKingWeight { get; set; }

        public int Generation { get; set; }

        [JsonIgnore]
        public string Name
        {
            get { return NameUtility.GenerateName(this); }
        }

        public WeightedAiGamePlayer(
            double nextAvailableMoveCountWeight,
            double opponentNextAvailableMoveCountWeight,
            double nextMovePiecesAtRiskWeight,
            double nextMovePiecesToTakeWeight,
            double kingWeight,
            double nextMoveKingWeight,
            double opponentNextMoveKingWeight,
            int generation)
        {
            NextAvailableMoveCountWeight = nextAvailableMoveCountWeight;
            OpponentNextAvailableMoveCountWeight = opponentNextAvailableMoveCountWeight;
            NextMovePiecesAtRiskWeight = nextMovePiecesAtRiskWeight;
            NextMovePiecesToTakeWeight = nextMovePiecesToTakeWeight;
            KingWeight = kingWeight;
            NextMoveKingWeight = nextMoveKingWeight;
            OpponentNextMoveKingWeight = opponentNextAvailableMoveCountWeight;
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

                double weightedResult = 0;
                weightedResult += KingWeight * moveMetric.CreatedFriendlyKings;
                weightedResult += NextAvailableMoveCountWeight * moveMetric.FriendlyMovesAvailable;
                weightedResult += OpponentNextAvailableMoveCountWeight * moveMetric.OpponentMovesAvailable;
                weightedResult += NextMovePiecesAtRiskWeight * moveMetric.NextMoveFriendlyPiecesAtRisk;
                weightedResult += NextMovePiecesToTakeWeight * moveMetric.NextMoveOpponentPiecesAtRisk;
                weightedResult += NextMoveKingWeight * moveMetric.NextMoveFriendlyKingsCreated;
                weightedResult += OpponentNextMoveKingWeight * moveMetric.NextMoveOpponentKingsCreated;

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
            return this;
        }

        public override string ToString()
        {
            return new ToStringBuilder<WeightedAiGamePlayer>(this)
                .Append(p => p.NextAvailableMoveCountWeight)
                .Append(p => p.NextMovePiecesAtRiskWeight)
                .Append(p => p.NextMovePiecesToTakeWeight)
                .Append(p => p.KingWeight)
                .Append(p => p.NextMoveKingWeight)
                .Append(p => p.Generation)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<WeightedAiGamePlayer>(this, that)
                .Append(p => p.NextAvailableMoveCountWeight)
                .Append(p => p.NextMovePiecesAtRiskWeight)
                .Append(p => p.NextMovePiecesToTakeWeight)
                .Append(p => p.KingWeight)
                .Append(p => p.NextMoveKingWeight)
                .Append(p => p.Generation)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(NextAvailableMoveCountWeight)
                .Append(NextMovePiecesAtRiskWeight)
                .Append(NextMovePiecesToTakeWeight)
                .Append(KingWeight)
                .Append(NextMoveKingWeight)
                .Append(Generation)
                .HashCode;
        }
    }
}
