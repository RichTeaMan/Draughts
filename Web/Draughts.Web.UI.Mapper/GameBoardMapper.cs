using Draughts.Service;
using Draughts.Web.UI.Domain;
using System;
using System.Collections.Generic;

namespace Draughts.Web.UI.Mapper
{
    public class GameBoardMapper
    {

        public GameBoard Map(GameState gameState)
        {
            List<Domain.GamePiece> pieces = new List<Domain.GamePiece>();
            foreach(var piece in gameState.GamePieceList)
            {

                var controllerPiece = new Domain.GamePiece()
                {
                    Xcoord = piece.Xcoord,
                    Ycoord = piece.Ycoord,
                    PieceColour = piece.PieceColour.ToString(),
                    PieceRank = piece.PieceRank.ToString()
                };

                pieces.Add(controllerPiece);
            }

            var gameBoard = new GameBoard()
            {
                Width = gameState.XLength,
                Height = gameState.YLength,
                GamePieces = pieces.ToArray()
            };

            return gameBoard;
        }
    }
}
