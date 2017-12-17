using Draughts.Service;
using System;
using System.Linq;
using System.Threading;

namespace Draughts.UI.Wpf
{
    public class HumanPlayer : IGamePlayer
    {
        public DraughtsBoard DraughtsBoard { get; set; }

        /// <summary>
        /// Gets if it's this players turn.
        /// </summary>
        public bool CurrentTurn { get; private set; }

        public GameMove SelectedMove { get; set; }

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            CurrentTurn = true;
            while (null == SelectedMove)
            {
                Thread.Sleep(10);
            }
            var gamePlayerMoveResult = new GamePlayerMoveResult(SelectedMove.PerformMove(), MoveStatus.SuccessfulMove);
            SelectedMove = null;
            CurrentTurn = false;
            return gamePlayerMoveResult;
        }
    }
}
