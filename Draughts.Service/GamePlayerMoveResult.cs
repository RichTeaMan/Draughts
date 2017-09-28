using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GamePlayerMoveResult
    {
        public GameState GameState { get; }

        public MoveStatus MoveStatus { get; }

        public GamePlayerMoveResult(GameState gameState, MoveStatus moveStatus)
        {
            GameState = gameState;
            MoveStatus = moveStatus;
        }
    }
}
