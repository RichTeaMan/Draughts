using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public interface IGamePlayer
    {
        GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState);

        /// <summary>
        /// Gets a name for the game player.
        /// </summary>
        string Name { get; }

    }
}
