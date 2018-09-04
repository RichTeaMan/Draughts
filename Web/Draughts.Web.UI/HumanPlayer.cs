using Draughts.Service;
using System;
using System.Linq;
using System.Threading;

namespace Draughts.Web.UI
{
    public class HumanPlayer : IGamePlayer
    {

        public GameMatch GameMatch { get; set; }

        public PieceColour PieceColour { get; set; }

        /// <summary>
        /// Gets if it's this players turn.
        /// </summary>
        public bool CurrentTurn { get; private set; }

        public GameMove SelectedMove { get; set; }

        public string Name { get; set; } = "Human player";

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            CurrentTurn = true;
            GamePlayerMoveResult gamePlayerMoveResult;
            // check if human has lost
            if (gameState.CalculateAvailableMoves(pieceColour).Count > 0)
            {
                while (null == SelectedMove)
                {
                    Thread.Sleep(10);
                }
                gamePlayerMoveResult = new GamePlayerMoveResult(SelectedMove.PerformMove(), MoveStatus.SuccessfulMove);
            }
            else
            {
                gamePlayerMoveResult = new GamePlayerMoveResult(gameState, MoveStatus.NoLegalMoves);
            }
            SelectedMove = null;
            CurrentTurn = false;
            return gamePlayerMoveResult;
        }
    }
}
