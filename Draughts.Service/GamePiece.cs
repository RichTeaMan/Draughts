﻿using RichTea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GamePiece
    {
        public PieceColour PieceColour { get; }

        public PieceRank PieceRank { get; }

        /// <summary>
        /// Gets the X position of this piece, where 0 is the most left column.
        /// </summary>
        public int Xcoord { get; }

        /// <summary>
        /// Gets the Y positino of this piece, where 0 is the most bottom row.
        /// </summary>
        public int Ycoord { get; }

        public GamePiece(PieceColour pieceColour, PieceRank pieceRank, int xCoord, int yCoord)
        {
            PieceColour = pieceColour;
            PieceRank = pieceRank;
            Xcoord = xCoord;
            Ycoord = yCoord;
        }

        public override string ToString()
        {
            return new ToStringBuilder<GamePiece>(this)
                .Append(p => p.PieceColour)
                .Append(p => p.PieceRank)
                .Append(p => p.Xcoord)
                .Append(p => p.Ycoord)
                .ToString();
        }

        public override bool Equals(object that)
        {
            var otherGamePiece = that as GamePiece;
            return Xcoord == otherGamePiece?.Xcoord &&
                Ycoord == otherGamePiece?.Ycoord &&
                PieceColour == otherGamePiece?.PieceColour &&
                PieceRank == otherGamePiece?.PieceRank;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(PieceColour)
                .Append(PieceRank)
                .Append(Xcoord)
                .Append(Ycoord)
                .HashCode;
        }
    }
}
