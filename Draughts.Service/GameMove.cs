using RichTea.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Service
{
    /// <summary>
    /// An immutable representation of a game move.
    /// </summary>
    public class GameMove
    {
        /// <summary>
        /// Gets the moving game piece before it has moved.
        /// </summary>
        public GamePiece StartGamePiece { get; }

        /// <summary>
        /// Gets the moving game piece after it has moved.
        /// </summary>
        public GamePiece EndGamePiece { get; }

        /// <summary>
        /// Gets a list of pieces that will be taken during this move. This might be an empty list, but not null.
        /// </summary>
        public IReadOnlyCollection<GamePiece> TakenGamePieces { get; }

        /// <summary>
        /// Gets that game state before the move has occurred.
        /// </summary>
        public GameState GameState { get; }

        /// <summary>
        /// Initialises an instance of game move.
        /// </summary>
        /// <param name="startGamePiece"></param>
        /// <param name="endGamePiece"></param>
        /// <param name="takenGamePieces"></param>
        /// <param name="gameState"></param>
        public GameMove(
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

        /// <summary>
        /// Calculates metrics for a move with respect to a player.
        /// </summary>
        /// <param name="pieceColour">The player colour to calculate metrics for.</param>
        /// <returns></returns>
        public GameMoveMetrics CalculateGameMoveMetrics(PieceColour pieceColour)
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

            int opponentPiecesTaken = TakenGamePieces.Count(p => p.PieceColour != pieceColour);

            int totalPieces = GameState.GamePieceList.Count;
            int totalFriendlyPieces = GameState.GamePieceList.Count(p => p.PieceColour == pieceColour);
            int totalOpponentPieces = GameState.GamePieceList.Count(p => p.PieceColour != pieceColour);

            int totalMinionPieces = GameState.GamePieceList.Count(p => p.PieceRank == PieceRank.Minion);
            int totalFriendlyMinionPieces = GameState.GamePieceList.Count(p => p.PieceRank == PieceRank.Minion && p.PieceColour == pieceColour);
            int totalOpponentMinionPieces = GameState.GamePieceList.Count(p => p.PieceRank == PieceRank.Minion && p.PieceColour != pieceColour);

            int totalKingPieces = GameState.GamePieceList.Count(p => p.PieceRank == PieceRank.King);
            int totalFriendlyKingPieces = GameState.GamePieceList.Count(p => p.PieceRank == PieceRank.King && p.PieceColour == pieceColour);
            int totalOpponentKingPieces = GameState.GamePieceList.Count(p => p.PieceRank == PieceRank.King && p.PieceColour != pieceColour);

            var whitePiecesInBottom = GameState.GamePieceList.Where(p => p.PieceColour == PieceColour.White && p.Ycoord < GameState.YLength / 2);
            var whitePiecesInTop = GameState.GamePieceList.Where(p => p.PieceColour == PieceColour.White && p.Ycoord > GameState.YLength / 2);

            var blackPiecesInBottom = GameState.GamePieceList.Where(p => p.PieceColour == PieceColour.Black && p.Ycoord < GameState.YLength / 2);
            var blackPiecesInTop = GameState.GamePieceList.Where(p => p.PieceColour == PieceColour.Black && p.Ycoord > GameState.YLength / 2);

            int friendlyMinionsHome = 0;
            int friendlyMinionsAway = 0;
            int opponentMinionsHome = 0;
            int opponentMinionsAway = 0;
            if (pieceColour == PieceColour.White)
            {
                friendlyMinionsHome = whitePiecesInBottom.Count();
                friendlyMinionsAway = whitePiecesInTop.Count();

                opponentMinionsHome = blackPiecesInBottom.Count();
                opponentMinionsAway = blackPiecesInTop.Count();
            }
            else if (pieceColour == PieceColour.Black)
            {
                friendlyMinionsHome = blackPiecesInTop.Count();
                friendlyMinionsAway = blackPiecesInBottom.Count();

                opponentMinionsHome = whitePiecesInTop.Count();
                opponentMinionsAway = whitePiecesInBottom.Count();
            }
            else
            {
                throw new InvalidOperationException("Unknown piece colour.");
            }

            GameMoveMetrics gameMoveMetrics = new GameMoveMetrics(
                createdFriendlyKings,
                friendlyMovesAvailable,
                opponentMovesAvailable,
                nextMoveFriendlyPiecesAtRisk,
                nextMoveOpponentPiecesAtRisk,
                nextMoveFriendlyKingsCreated,
                nextMoveOpponentKingsCreated,
                opponentPiecesTaken,
                totalPieces,
                totalFriendlyPieces,
                totalOpponentPieces,
                totalMinionPieces,
                totalFriendlyMinionPieces,
                totalOpponentMinionPieces,
                totalKingPieces,
                totalFriendlyKingPieces,
                totalOpponentKingPieces,
                friendlyMinionsHome,
                opponentMinionsHome,
                friendlyMinionsAway,
                opponentMinionsAway
            );
            return gameMoveMetrics;
        }

        /// <summary>
        /// Creates a game state of the completed move.
        /// </summary>
        /// <returns></returns>
        public GameState PerformMove()
        {
            var previousPieces = GameState.GamePieceList;

            var newPieces = previousPieces.Where(p => p != StartGamePiece && !TakenGamePieces.Contains(p)).ToList();
            newPieces.Add(EndGamePiece);

            var newGameState = new GameState(GameState, newPieces, GameState.XLength, GameState.YLength);
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
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(StartGamePiece)
                .Append(EndGamePiece)
                .Append(TakenGamePieces)
                .Append(GameState)
            .HashCode;
        }
    }
}
