using Draughts.Web.UI.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Draughts.Web.UI.Mapper
{
    /// <summary>
    ///  A mapper that calcuates opposite coordinates, as though the board was rotated.
    /// </summary>
    public class BoardRotaterMapper
    {
        /// <summary>
        /// Creates a new gameboard that has been rotated. This includes all pieces and moves coordinates.
        /// </summary>
        /// <param name="gameBoard">Gameboard to rotate.</param>
        /// <returns>GameBoard</returns>
        public GameBoard Rotate(GameBoard gameBoard)
        {
            if (gameBoard == null)
            {
                return null;
            }

            var rotatedPieces = new List<GamePiece>();
            if (gameBoard.GamePieces != null)
            {
                foreach (var piece in gameBoard.GamePieces)
                {
                    var rotatedPoint = Rotate(piece.Xcoord, piece.Ycoord, gameBoard.Width, gameBoard.Height);
                    var rotatedPiece = new GamePiece()
                    {
                        PieceColour = piece.PieceColour,
                        PieceRank = piece.PieceRank,
                        Xcoord = rotatedPoint.X,
                        Ycoord = rotatedPoint.Y
                    };
                    rotatedPieces.Add(rotatedPiece);
                }
            }

            var rotatedMoves = new List<GameMove>();
            if (gameBoard.GameMoves != null)
            {
                foreach (var move in gameBoard.GameMoves)
                {
                    var rotatedStartPoint = Rotate(move.StartX, move.StartY, gameBoard.Width, gameBoard.Height);
                    var rotatedEndPoint = Rotate(move.EndX, move.EndY, gameBoard.Width, gameBoard.Height);
                    var rotatedMove = new GameMove()
                    {
                        StartX = rotatedStartPoint.X,
                        StartY = rotatedStartPoint.Y,
                        EndX = rotatedEndPoint.X,
                        EndY = rotatedEndPoint.Y
                    };
                    rotatedMoves.Add(rotatedMove);
                }
            }

            var rotatedGameBoard = new GameBoard()
            {
                CurrentTurnColour = gameBoard.CurrentTurnColour,
                GameMoves = rotatedMoves.ToArray(),
                GamePieces = rotatedPieces.ToArray(),
                GameStatus = gameBoard.GameStatus,
                Height = gameBoard.Height,
                OpponentColour = gameBoard.OpponentColour,
                OpponentName = gameBoard.OpponentName,
                PlayerColour = gameBoard.PlayerColour,
                PlayerName = gameBoard.PlayerName,
                Width = gameBoard.Width
            };

            return rotatedGameBoard;
        }

        /// <summary>
        /// Rotates a given coordinate.
        /// </summary>
        /// <param name="x">X value to rotate.</param>
        /// <param name="y">Y value to rotate.</param>
        /// <param name="width">Width of the board.</param>
        /// <param name="height">Height of the board.</param>
        /// <returns>Point</returns>
        public Point Rotate(int x, int y, int width, int height)
        {
            int transformedX = (width - 1) - x;

            int transformedY = (height - 1) - y;

            return new Point(transformedX, transformedY);
        }
    }
}
