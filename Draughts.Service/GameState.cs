using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GameState
    {
        public readonly IReadOnlyList<GamePiece> GamePieceList;

        public readonly int XLength;

        public readonly int YLength;

        public GameState(IEnumerable<GamePiece> gamePieceList, int xLength, int yLength)
        {
            GamePieceList = gamePieceList.ToList();
            XLength = xLength;
            YLength = yLength;
        }

        public IList<GameMove> CalculateAvailableMoves()
        {
            var gameMoves = new List<GameMove>();
            foreach (var piece in GamePieceList)
            {
                int yDelta = 0;
                if (piece.PieceColour == PieceColour.White)
                {
                    yDelta = 1;
                }
                else if (piece.PieceColour == PieceColour.Black)
                {
                    yDelta = -1;
                }
                else
                {
                    throw new ApplicationException("Unknown piece colour.");
                }

                int newY = piece.Ycoord + yDelta;

                var newXcoords = new List<int>();
                int newXleft = piece.Xcoord - 1;
                if (newXleft >= 0)
                {
                    newXcoords.Add(newXleft);
                }

                int newXright = piece.Xcoord + 1;
                if (newXright < XLength)
                {
                    newXcoords.Add(newXright);
                }

                foreach (var newX in newXcoords)
                {
                    var occupiedPiece = GamePieceList.SingleOrDefault(p => p.Xcoord == newX && p.Ycoord == newY);
                    if (occupiedPiece == null)
                    {
                        var endPiece = new GamePiece(piece.PieceColour, piece.PieceRank, newX, newY);
                        gameMoves.Add(new GameMove(piece, endPiece, new List<GamePiece>(), this));
                    }
                    else if (occupiedPiece.PieceColour != piece.PieceColour)
                    {
                        int jumpedX = ((newX - piece.Xcoord) * 2) + piece.Xcoord;
                        int jumpedY = ((newY - piece.Ycoord) * 2) + piece.Ycoord;
                        if (jumpedX >= 0 && jumpedX < XLength && jumpedY >= 0 && jumpedY < YLength)
                        {
                            var jumpedPiece = GamePieceList.SingleOrDefault(p => p.Xcoord == jumpedX && p.Ycoord == jumpedY);
                            if (jumpedPiece == null)
                            {
                                var endPiece = new GamePiece(piece.PieceColour, piece.PieceRank, jumpedX, jumpedY);
                                gameMoves.Add(new GameMove(piece, endPiece, new[] { occupiedPiece }, this));
                            }
                        }
                    }
                }
            }

            var resultGameMoves = new List<GameMove>();

            // pieces must be taken
            var colourGroupList = gameMoves.GroupBy(m => m.StartGamePiece.PieceColour);
            foreach(var colourGroup in colourGroupList)
            {
                if (colourGroup.Any(m => m.TakenGamePieces.Count > 0))
                {
                    resultGameMoves.AddRange(colourGroup.Where(m => m.TakenGamePieces.Count > 0));
                } else
                {
                    resultGameMoves.AddRange(colourGroup);
                }
            }

            return resultGameMoves;
        }
    }
}
