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

        public GameMoveMetrics(int createdFriendlyKings,
            int friendlyMovesAvailable,
            int opponentMovesAvailable,
            int nextMoveFriendlyPiecesAtRisk,
            int nextMoveOpponentPiecesAtRisk,
            int nextMoveFriendlyKingsCreated,
            int nextMoveOpponentKingsCreated)
        {
            CreatedFriendlyKings = createdFriendlyKings;
            FriendlyMovesAvailable = friendlyMovesAvailable;
            OpponentMovesAvailable = opponentMovesAvailable;
            NextMoveFriendlyPiecesAtRisk = nextMoveFriendlyPiecesAtRisk;
            NextMoveOpponentPiecesAtRisk = nextMoveOpponentPiecesAtRisk;
            NextMoveFriendlyKingsCreated = nextMoveFriendlyKingsCreated;
            NextMoveOpponentKingsCreated = nextMoveOpponentKingsCreated;
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
                .HashCode;
        }
    }
}
