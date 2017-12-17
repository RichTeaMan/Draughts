using Draughts.Service;
using System;
using System.Linq;
using System.Threading;

namespace Draughts.UI.Wpf
{
    public class HumanPlayer : IGamePlayer
    {
        public DraughtsBoard DraughtsBoard { get; set; }

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
