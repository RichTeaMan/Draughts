namespace Draughts.Web.UI.Domain
{
    /// <summary>
    /// Represents a draughts game.
    /// </summary>
    public class GameBoard
    {
        /// <summary>
        /// Gets or sets a list of game pieces.
        /// </summary>
        public GamePiece[] GamePieces { get; set; }

        /// <summary>
        /// Gets or sets the width of the game board.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game board.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the colour of the current turn.
        /// </summary>
        public string CurrentTurnColour { get; set; }
    }
}
