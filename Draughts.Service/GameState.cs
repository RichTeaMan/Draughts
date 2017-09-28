﻿using System;
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

        private PieceRank CalculatePieceRank(int yPosition, PieceColour pieceColour, PieceRank pieceRank)
        {
            PieceRank resultPieceRank = PieceRank.Minion;
            if (pieceRank == PieceRank.King)
            {
                resultPieceRank = PieceRank.King;
            }
            else
            {
                switch (pieceColour)
                {
                    case PieceColour.White:
                        if (yPosition == YLength - 1)
                        {
                            resultPieceRank = PieceRank.King;
                        }
                        break;
                    case PieceColour.Black:
                        if (yPosition == 0)
                        {
                            resultPieceRank = PieceRank.King;
                        }
                        break;
                }
            }
            return resultPieceRank;
        }

        public IList<GameMove> CalculateAvailableMoves()
        {
            var gameMoves = new List<GameMove>();
            foreach (var piece in GamePieceList)
            {
                var newYList = new List<int>();

                if (piece.PieceRank == PieceRank.Minion)
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
                    newYList.Add(piece.Ycoord + yDelta);
                }
                else if (piece.PieceRank == PieceRank.King)
                {
                    newYList.Add(piece.Ycoord + 1);
                    newYList.Add(piece.Ycoord - 1);
                }
                else
                {
                    throw new ApplicationException("Unknown piece rank.");
                }

                foreach (var newY in newYList)
                {
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
                            var newRank = CalculatePieceRank(newY, piece.PieceColour, piece.PieceRank);
                            var endPiece = new GamePiece(piece.PieceColour, newRank, newX, newY);
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
                                    var newRank = CalculatePieceRank(newY, piece.PieceColour, piece.PieceRank);
                                    var endPiece = new GamePiece(piece.PieceColour, newRank, jumpedX, jumpedY);
                                    gameMoves.Add(new GameMove(piece, endPiece, new[] { occupiedPiece }, this));
                                }
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
