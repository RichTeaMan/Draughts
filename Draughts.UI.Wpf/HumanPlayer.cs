using Draughts.Service;
using System;
using System.Linq;
using System.Threading;

namespace Draughts.UI.Wpf
{
    public class HumanPlayer : IGamePlayer
    {
        public DraughtsBoard DraughtsBoard { get; set; }

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            while (true)
            {
                if (DraughtsBoard.SelectedGamePiece != null)
                {
                    var piecesToHighlight = gameState.CalculateAvailableMoves()
                        .Where(p => p.StartGamePiece == DraughtsBoard.SelectedGamePiece);

                    foreach (var piece in piecesToHighlight)
                    {
                        DraughtsBoard.Dispatcher.Invoke(() =>
                        {
                            var panel = DraughtsBoard.FindSquare(piece.EndGamePiece.Xcoord, piece.EndGamePiece.Ycoord);
                            panel.Background = DraughtsBoard.PossibleMoveColourBrush;
                        });
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
