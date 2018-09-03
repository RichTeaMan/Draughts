namespace Draughts.Web.UI.Domain
{
    public class GamePiece
    {
        /// <summary>
        /// Gets or sets the colour of the piece.
        /// </summary>
        public PieceColour PieceColour { get; set; }

        /// <summary>
        /// Gets or sets the rank of the piece.
        /// </summary>
        public PieceRank PieceRank { get; set; }

        /// <summary>
        /// Gets or sets the X position of this piece, where 0 is the most left column.
        /// </summary>
        public int Xcoord { get; set; }

        /// <summary>
        /// Gets sets the Y positino of this piece, where 0 is the most bottom row.
        /// </summary>
        public int Ycoord { get; set; }
    }
}
