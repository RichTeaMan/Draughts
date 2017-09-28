using Draughts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameState = GameStateFactory.StandardStartGameState();

            var board = new GamePiece[gameState.XLength, gameState.YLength];
            foreach (var piece in gameState.GamePieceList)
            {
                board[piece.Xcoord, piece.Ycoord] = piece;
            }

            var rows = new List<string>();
            foreach (var y in Enumerable.Range(0, gameState.YLength))
            {
                List<string> row = new List<string>();
                foreach (var x in Enumerable.Range(0, gameState.XLength))
                {
                    char pieceSymbol;
                    var piece = board[x, y];
                    if (null == piece)
                    {
                        pieceSymbol = ' ';
                    }
                    else if (PieceColour.Black == piece.PieceColour)
                    {
                        pieceSymbol = 'X';
                    }
                    else if (PieceColour.White == piece.PieceColour)
                    {
                        pieceSymbol = 'O';
                    }
                    else
                    {
                        throw new ApplicationException("Unknown piece colour.");
                    }
                    row.Add(pieceSymbol.ToString());
                }
                rows.Add(string.Join("|", row.ToArray()));
            }
            var rowSeperator = new string(Enumerable.Range(0, (gameState.XLength * 2) - 1).Select(i => i % 2 == 0 ? '-': '+').ToArray());
            var output = string.Join(Environment.NewLine + rowSeperator + Environment.NewLine, rows.Reverse<string>());

            System.Console.Write(output);
            System.Console.ReadLine();
        }
    }
}
