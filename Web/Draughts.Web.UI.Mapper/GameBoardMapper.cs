using Draughts.Service;
using Draughts.Web.UI.Domain;
using System;
using System.Collections.Generic;

namespace Draughts.Web.UI.Mapper
{
    public class GameBoardMapper
    {

        public GameBoard Map(GameMatch gameMatch)
        {
            List<Domain.GamePiece> pieces = new List<Domain.GamePiece>();
            foreach(var piece in gameMatch.GameState.GamePieceList)
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
                Width = gameMatch.GameState.XLength,
                Height = gameMatch.GameState.YLength,
                GamePieces = pieces.ToArray(),
                CurrentTurnColour = gameMatch.CurrentTurn.ToString()
            };

            return gameBoard;
        }
    }
}
