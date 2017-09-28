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
            foreach(var piece in GamePieceList)
            {
                int yDelta = 0;
                if (piece.PieceColour == PieceColour.White)
                {
                    yDelta = 1;
                } else if (piece.PieceColour == PieceColour.Black)
                {
                    yDelta = -1;
                } else
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
                    var occupiedPiece = GamePieceList.SingleOrDefault(p => p.Xcoord == newX && piece.Ycoord == newY);
                    if (occupiedPiece == null)
                    {
                        var endPiece = new GamePiece(piece.PieceColour, piece.PieceRank, newX, newY);
                        gameMoves.Add(new GameMove(piece, endPiece, new List<GamePiece>()));
                    }
                }
            }
            return gameMoves;
        }
    }
}
