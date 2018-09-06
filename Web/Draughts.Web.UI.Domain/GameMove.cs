namespace Draughts.Web.UI.Domain
{
    /// <summary>
    /// Represents an allowed move.
    /// </summary>
    public class GameMove
    {
        /// <summary>
        /// Gets or sets a piece's starting X position.
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// Gets or sets a piece's starting Y position.
        /// </summary>
        public int StartY { get; set; }

        /// <summary>
        /// Gets or sets the piece's ending X position.
        /// </summary>
        public int EndX { get; set; }

        /// <summary>
        /// Gets or sets the piece's ending Y position.
        /// </summary>
        public int EndY { get; set; }
    }
}
