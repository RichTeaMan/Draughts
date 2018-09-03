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
                    Ycoord = piece.Ycoord
                };

                switch (piece.PieceColour)
                {
                    case Service.PieceColour.Black:
                        controllerPiece.PieceColour = Domain.PieceColour.Black;
                        break;
                    case Service.PieceColour.White:
                        controllerPiece.PieceColour = Domain.PieceColour.White;
                        break;
                }

                switch (piece.PieceRank)
                {
                    case Service.PieceRank.Minion:
                        controllerPiece.PieceRank = Domain.PieceRank.Man;
                        break;
                    case Service.PieceRank.King:
                        controllerPiece.PieceRank = Domain.PieceRank.King;
                        break;
                }

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
