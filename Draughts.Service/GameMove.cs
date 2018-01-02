using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GameMove
    {
        public GamePiece StartGamePiece { get; }

        public GamePiece EndGamePiece { get; }

        public IReadOnlyCollection<GamePiece> TakenGamePieces { get; }

        public GameState GameState { get; }

        public GameMove (
            GamePiece startGamePiece,
            GamePiece endGamePiece,
            IEnumerable<GamePiece> takenGamePieces,
            GameState gameState)
        {
            StartGamePiece = startGamePiece;
            EndGamePiece = endGamePiece;
            TakenGamePieces = takenGamePieces.ToList();
            GameState = gameState;
        }

        public GameMoveMetrics CalculateGameMoveMetrics (PieceColour pieceColour)
        {
            int createdFriendlyKings = 0;
            if (StartGamePiece.PieceColour == pieceColour && StartGamePiece.PieceRank == PieceRank.Minion && EndGamePiece.PieceRank == PieceRank.King)
            {
                createdFriendlyKings = 1;
            }

            var futureMoves = PerformMove().CalculateAvailableMoves();
            var friendlyMoves = futureMoves.Where(m => m.StartGamePiece.PieceColour == pieceColour).ToList();
            var opponentMoves = futureMoves.Where(m => m.StartGamePiece.PieceColour != pieceColour).ToList();

            int friendlyMovesAvailable = friendlyMoves.Count;
            int opponentMovesAvailable = opponentMoves.Count;
            int nextMoveFriendlyPiecesAtRisk = opponentMoves.Sum(m => m.TakenGamePieces.Count);
            int nextMoveOpponentPiecesAtRisk = friendlyMoves.Sum(m => m.TakenGamePieces.Count);
            int nextMoveFriendlyKingsCreated = friendlyMoves.Count(
                m => m.StartGamePiece.PieceRank == PieceRank.Minion &&
                m.EndGamePiece.PieceRank == PieceRank.King);
            int nextMoveOpponentKingsCreated = opponentMoves.Count(
                m => m.StartGamePiece.PieceRank == PieceRank.Minion &&
                m.EndGamePiece.PieceRank == PieceRank.King);

            GameMoveMetrics gameMoveMetrics = new GameMoveMetrics(
                createdFriendlyKings,
                friendlyMovesAvailable,
                opponentMovesAvailable,
                nextMoveFriendlyPiecesAtRisk,
                nextMoveOpponentPiecesAtRisk,
                nextMoveFriendlyKingsCreated,
                nextMoveOpponentKingsCreated);
            return gameMoveMetrics;
        }

        public GameState PerformMove()
        {
            var previousPieces = GameState.GamePieceList;

            var newPieces = previousPieces.Where(p => p != StartGamePiece && !TakenGamePieces.Contains(p)).ToList();
            newPieces.Add(EndGamePiece);

            var newGameState = new GameState(newPieces, GameState.XLength, GameState.YLength);
            return newGameState;
        }

        public override string ToString()
        {
            return new ToStringBuilder<GameMove>(this)
                .Append(p => p.StartGamePiece)
                .Append(p => p.EndGamePiece)
                .Append(p => p.TakenGamePieces)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<GameMove>(this, that)
                .Append(p => p.StartGamePiece)
                .Append(p => p.EndGamePiece)
                .Append(p => p.TakenGamePieces)
                .Append(p => p.GameState)
                .Equals();
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<GameMove>(this)
                .Append(p => p.StartGamePiece)
                .Append(p => p.EndGamePiece)
                .Append(p => p.TakenGamePieces)
                .Append(p => p.GameState)
            .HashCode;
        }
    }
}
