using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GameStateFactory
    {
        public const int StandardLength = 8;

        /// <summary>
        /// Creates a game state with the single specified piece.
        /// </summary>
        /// <param name="gamePiece"></param>
        /// <returns></returns>
        public static GameState SinglePieceGameState(GamePiece gamePiece)
        {
            var pieces = new[] { gamePiece };
            var gameState = new GameState(pieces, StandardLength, StandardLength);
            return gameState;
        }

        /// <summary>
        /// Creates a game state with the single specified piece.
        /// </summary>
        /// <param name="gamePiece"></param>
        /// <returns></returns>
        public static GameState SeveralPieceGameState(params GamePiece[] gamePieces)
        {
            var gameState = new GameState(gamePieces, StandardLength, StandardLength);
            return gameState;
        }

        /// <summary>
        /// Creates a standard game state for play with 12 pieces each.
        /// </summary>
        /// <param name="gamePiece"></param>
        /// <returns></returns>
        public static GameState StandardStartGameState()
        {
            var pieces = new List<GamePiece>();

            // white pieces
            pieces.AddRange(new[]
            {
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0),
                new GamePiece(PieceColour.White, PieceRank.Minion, 2, 0),
                new GamePiece(PieceColour.White, PieceRank.Minion, 4, 0),
                new GamePiece(PieceColour.White, PieceRank.Minion, 6, 0),

                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 1),
                new GamePiece(PieceColour.White, PieceRank.Minion, 3, 1),
                new GamePiece(PieceColour.White, PieceRank.Minion, 5, 1),
                new GamePiece(PieceColour.White, PieceRank.Minion, 7, 1),

                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 2, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 4, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 6, 2),
            });

            // black pieces
            pieces.AddRange(new[]
            {
                new GamePiece(PieceColour.Black, PieceRank.Minion, 1, 7),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 3, 7),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 5, 7),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 7, 7),

                new GamePiece(PieceColour.Black, PieceRank.Minion, 0, 6),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 2, 6),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 4, 6),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 6, 6),

                new GamePiece(PieceColour.Black, PieceRank.Minion, 1, 5),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 3, 5),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 5, 5),
                new GamePiece(PieceColour.Black, PieceRank.Minion, 7, 5),
            });

            var gameState = new GameState(pieces, StandardLength, StandardLength);
            return gameState;
        }
    }
}
