﻿namespace Draughts.Web.UI.Domain
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
        /// Gets or sets the piece that just moved or was taken in their positions before the move.
        /// </summary>
        public GamePiece[] MovedPieces { get; set; }

        /// <summary>
        /// Gets or sets the width of the game board.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game board.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the colour of the current turn. Allowed values: 'white', 'black'.
        /// </summary>
        public string CurrentTurnColour { get; set; }

        /// <summary>
        /// Gets or sets the status of the game. Allowed values: 'inProgress', 'draw', 'whiteWin', 'blackWin'.
        /// </summary>
        public string GameStatus { get; set; }

        /// <summary>
        /// Gets or sets player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets opponent name.
        /// </summary>
        public string OpponentName { get; set; }

        /// <summary>
        /// Gets or sets player colour.
        /// </summary>
        public string PlayerColour { get; set; }

        /// <summary>
        /// Gets or sets opponent colour.
        /// </summary>
        public string OpponentColour { get; set; }

        /// <summary>
        /// Gets or sets allowed game moves.
        /// </summary>
        public GameMove[] GameMoves { get; set; }
    }
}
