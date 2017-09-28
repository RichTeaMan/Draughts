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

            while (true)
            {
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
                var rowSeperator = new string(Enumerable.Range(0, (gameState.XLength * 2) - 1).Select(i => i % 2 == 0 ? '-' : '+').ToArray());
                var output = string.Join(Environment.NewLine + rowSeperator + Environment.NewLine, rows.Reverse<string>());

                var moveOutputLines = new List<string>();
                var moves = new List<GameMove>();
                foreach (var move in gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White))
                {
                    int index = moves.Count;
                    var s = move.StartGamePiece;
                    var e = move.EndGamePiece;
                    string moveOutputLine = $"{index} - Start: '{s.Xcoord}, {s.Ycoord}'    End: '{e.Xcoord}, {e.Ycoord}'";
                    moveOutputLines.Add(moveOutputLine);
                    moves.Add(move);
                }

                string moveOutput = string.Join(Environment.NewLine, moveOutputLines.ToArray());

                System.Console.Write(output);
                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine("Moves:");
                System.Console.Write(moveOutput);

                System.Console.WriteLine();
                System.Console.WriteLine("Choose a move:");

                int selection;
                while (true)
                {
                    var readLine = System.Console.ReadLine();
                    if (!int.TryParse(readLine, out selection))
                    {
                        System.Console.WriteLine("Type a number.");
                    }
                    else if (selection > moves.Count || selection < 0)
                    {
                        System.Console.WriteLine("Number not in range.");
                    }
                    else
                    {
                        break;
                    }
                }
                var selectedMove = moves[selection];
                gameState = selectedMove.PerformMove();
                System.Console.Clear();
            }
        }
    }
}
