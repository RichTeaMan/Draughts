using RichTea.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Draughts.Service
{
    public class GameMoveMetrics
    {
        public int CreatedFriendlyKings { get; }

        public int FriendlyMovesAvailable { get; }

        public int OpponentMovesAvailable { get; }

        public int NextMoveFriendlyPiecesAtRisk { get; }

        public int NextMoveOpponentPiecesAtRisk { get; }

        public int NextMoveFriendlyKingsCreated { get; }

        public int NextMoveOpponentKingsCreated { get; }

        public int TotalPieces { get; }

        public int TotalFriendlyPieces { get; }

        public int TotalOpponentPieces { get; }

        public int TotalMinionPieces { get; }

        public int TotalFriendlyMinionPieces { get; }

        public int TotalOpponentMinionPieces { get; }

        public int TotalKingPieces { get; }

        public int TotalFriendlyKingPieces { get; }

        public int TotalOpponentKingPieces { get; }

        public int FriendlyMinionsHome { get; }

        public int OpponentMinionsHome { get; }

        public int FriendlyMinionsAway { get; }

        public int OpponentMinionsAway { get; }

        public GameMoveMetrics(int createdFriendlyKings,
            int friendlyMovesAvailable,
            int opponentMovesAvailable,
            int nextMoveFriendlyPiecesAtRisk,
            int nextMoveOpponentPiecesAtRisk,
            int nextMoveFriendlyKingsCreated,
            int nextMoveOpponentKingsCreated,
            int totalPieces,
            int totalFriendlyPieces,
            int totalOpponentPieces,
            int totalMinionPieces,
            int totalFriendlyMinionPieces,
            int totalOpponentMinionPieces,
            int totalKingPieces,
            int totalFriendlyKingPieces,
            int totalOpponentKingPieces,
            int friendlyMinionsHome,
            int opponentMinionsHome,
            int friendlyMinionsAway,
            int opponentMinionsAway)
        {
            CreatedFriendlyKings = createdFriendlyKings;
            FriendlyMovesAvailable = friendlyMovesAvailable;
            OpponentMovesAvailable = opponentMovesAvailable;
            NextMoveFriendlyPiecesAtRisk = nextMoveFriendlyPiecesAtRisk;
            NextMoveOpponentPiecesAtRisk = nextMoveOpponentPiecesAtRisk;
            NextMoveFriendlyKingsCreated = nextMoveFriendlyKingsCreated;
            NextMoveOpponentKingsCreated = nextMoveOpponentKingsCreated;
            TotalPieces = totalPieces;
            TotalFriendlyPieces = totalFriendlyPieces;
            TotalOpponentPieces = totalOpponentPieces;
            TotalMinionPieces = totalMinionPieces;
            TotalFriendlyMinionPieces = totalFriendlyMinionPieces;
            TotalOpponentMinionPieces = totalOpponentMinionPieces;
            TotalKingPieces = totalKingPieces;
            TotalFriendlyKingPieces = totalFriendlyKingPieces;
            TotalOpponentKingPieces = totalOpponentKingPieces;
            FriendlyMinionsHome = friendlyMinionsHome;
            OpponentMinionsHome = opponentMinionsHome;
            FriendlyMinionsAway = friendlyMinionsAway;
            OpponentMinionsAway = opponentMinionsAway;
        }

        public override string ToString()
        {
            return new ToStringBuilder<GameMoveMetrics>(this)
                .Append(m => m.CreatedFriendlyKings)
                .Append(m => m.FriendlyMovesAvailable)
                .Append(m => m.OpponentMovesAvailable)
                .Append(m => m.NextMoveFriendlyPiecesAtRisk)
                .Append(m => m.NextMoveOpponentPiecesAtRisk)
                .Append(m => m.NextMoveFriendlyKingsCreated)
                .Append(m => m.NextMoveOpponentKingsCreated)
                .Append(m => m.TotalPieces)
                .Append(m => m.TotalFriendlyPieces)
                .Append(m => m.TotalOpponentPieces)
                .Append(m => m.TotalMinionPieces)
                .Append(m => m.TotalFriendlyMinionPieces)
                .Append(m => m.TotalOpponentMinionPieces)
                .Append(m => m.TotalKingPieces)
                .Append(m => m.TotalFriendlyKingPieces)
                .Append(m => m.TotalOpponentKingPieces)
                .Append(m => m.FriendlyMinionsHome)
                .Append(m => m.OpponentMinionsHome)
                .Append(m => m.FriendlyMinionsAway)
                .Append(m => m.OpponentMinionsAway)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<GameMoveMetrics>(this, that)
                .Append(m => m.CreatedFriendlyKings)
                .Append(m => m.FriendlyMovesAvailable)
                .Append(m => m.OpponentMovesAvailable)
                .Append(m => m.NextMoveFriendlyPiecesAtRisk)
                .Append(m => m.NextMoveOpponentPiecesAtRisk)
                .Append(m => m.NextMoveFriendlyKingsCreated)
                .Append(m => m.NextMoveOpponentKingsCreated)
                .Append(m => m.TotalPieces)
                .Append(m => m.TotalFriendlyPieces)
                .Append(m => m.TotalOpponentPieces)
                .Append(m => m.TotalMinionPieces)
                .Append(m => m.TotalFriendlyMinionPieces)
                .Append(m => m.TotalOpponentMinionPieces)
                .Append(m => m.TotalKingPieces)
                .Append(m => m.TotalFriendlyKingPieces)
                .Append(m => m.TotalOpponentKingPieces)
                .Append(m => m.FriendlyMinionsHome)
                .Append(m => m.OpponentMinionsHome)
                .Append(m => m.FriendlyMinionsAway)
                .Append(m => m.OpponentMinionsAway)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<GameMoveMetrics>(this)
                .Append(CreatedFriendlyKings)
                .Append(FriendlyMovesAvailable)
                .Append(OpponentMovesAvailable)
                .Append(NextMoveFriendlyPiecesAtRisk)
                .Append(NextMoveOpponentPiecesAtRisk)
                .Append(NextMoveFriendlyKingsCreated)
                .Append(NextMoveOpponentKingsCreated)
                .Append(TotalPieces)
                .Append(TotalFriendlyPieces)
                .Append(TotalOpponentPieces)
                .Append(TotalMinionPieces)
                .Append(TotalFriendlyMinionPieces)
                .Append(TotalOpponentMinionPieces)
                .Append(TotalKingPieces)
                .Append(TotalFriendlyKingPieces)
                .Append(TotalOpponentKingPieces)
                .Append(FriendlyMinionsHome)
                .Append(OpponentMinionsHome)
                .Append(FriendlyMinionsAway)
                .Append(OpponentMinionsAway)
                .HashCode;
        }
    }
}
