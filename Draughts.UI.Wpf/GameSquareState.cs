using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.UI.Wpf
{
    /// <summary>
    /// Enum of states a game square can be in.
    /// </summary>
    public enum GameSquareState
    {
        /// <summary>
        /// There is no special condition this square is in.
        /// </summary>
        Standard,

        /// <summary>
        /// This square is currently selected.
        /// </summary>
        PlayerSelected,

        /// <summary>
        /// A piece can be moved to this square.
        /// </summary>
        PossibleMove,
    }
}
