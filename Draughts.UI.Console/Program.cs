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
            IGamePlayer ai = new RandomGamePlayer();

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
                        else if (PieceColour.Black == piece.PieceColour && PieceRank.King == piece.PieceRank)
                        {
                            pieceSymbol = 'X';
                        }
                        else if (PieceColour.White == piece.PieceColour && PieceRank.King == piece.PieceRank)
                        {
                            pieceSymbol = 'O';
                        }
                        else if (PieceColour.Black == piece.PieceColour && PieceRank.Minion == piece.PieceRank)
                        {
                            pieceSymbol = 'x';
                        }
                        else if (PieceColour.White == piece.PieceColour && PieceRank.Minion == piece.PieceRank)
                        {
                            pieceSymbol = 'o';
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
                var availableMoves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();
                if (availableMoves.Count == 0)
                {
                    System.Console.WriteLine("No available moves. You have lost.");
                    System.Console.ReadKey();
                    return;
                }
                foreach (var move in availableMoves)
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

                // perform ai move
                var aiResult = ai.MakeMove(PieceColour.Black, gameState);

                if (aiResult.MoveStatus == MoveStatus.NoLegalMoves)
                {
                    System.Console.WriteLine("Opponent cannot move. You have won.");
                    System.Console.ReadKey();
                    return;
                } else if(aiResult.MoveStatus == MoveStatus.SuccessfulMove)
                {
                    gameState = aiResult.GameState;
                } else
                {
                    throw new ApplicationException("Unknown AI result state.");
                }

                System.Console.Clear();
            }
        }
    }
}
