using Draughts.Service;
using Draughts.Web.UI.Domain;
using System.Collections.Generic;

namespace Draughts.Web.UI.Mapper
{
    public class GameBoardMapper
    {

        public GameBoard Map(GameMatch gameMatch)
        {
            List<Domain.GamePiece> pieces = new List<Domain.GamePiece>();
            foreach (var piece in gameMatch.GameState.GamePieceList)
            {

                string pieceColour = null;
                switch (piece.PieceColour)
                {
                    case Service.PieceColour.Black:
                        pieceColour = "black";
                        break;
                    case Service.PieceColour.White:
                        pieceColour = "white";
                        break;
                }

                var controllerPiece = new Domain.GamePiece()
                {

                    Xcoord = piece.Xcoord,
                    Ycoord = piece.Ycoord,
                    PieceColour = pieceColour,
                    PieceRank = piece.PieceRank.ToString()
                };

                pieces.Add(controllerPiece);
            }

            string currentTurnColour = null;
            switch (gameMatch.CurrentTurn)
            {
                case Service.PieceColour.Black:
                    currentTurnColour = "black";
                    break;
                case Service.PieceColour.White:
                    currentTurnColour = "white";
                    break;
            }

            string gameStatus = null;
            switch (gameMatch.GameMatchOutcome)
            {
                case GameMatchOutcome.WhiteWin:
                    gameStatus = "whiteWin";
                    break;
                case GameMatchOutcome.BlackWin:
                    gameStatus = "blackWin";
                    break;
                case GameMatchOutcome.Draw:
                    gameStatus = "draw";
                    break;
                case GameMatchOutcome.InProgress:
                    gameStatus = "inProgress";
                    break;
            }
            var gameBoard = new GameBoard()
            {
                Width = gameMatch.GameState.XLength,
                Height = gameMatch.GameState.YLength,
                GamePieces = pieces.ToArray(),
                CurrentTurnColour = currentTurnColour,
                GameStatus = gameStatus
            };

            return gameBoard;
        }
    }
}
