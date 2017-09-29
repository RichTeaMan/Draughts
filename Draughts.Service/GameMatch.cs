using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    /// <summary>
    /// Class for playing an entire match between two IGamePlayers.
    /// </summary>
    public class GameMatch
    {
        public const int TurnLimit = 1000;

        /// <summary>
        /// Completes a match between two players. White always plays first.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="whiteGamePlayer"></param>
        /// <param name="blackGamePlayer"></param>
        /// <returns></returns>
        public GameMatchOutcome CompleteMatch(GameState gameState, IGamePlayer whiteGamePlayer, IGamePlayer blackGamePlayer)
        {
            GameMatchOutcome outcome = GameMatchOutcome.Draw;
            var currentGameState = gameState;
            int turns = 0;
            while(turns < TurnLimit) {

                var whiteResult = whiteGamePlayer.MakeMove(PieceColour.White, currentGameState);
                if (whiteResult.MoveStatus == MoveStatus.NoLegalMoves)
                {
                    outcome = GameMatchOutcome.BlackWin;
                    break;
                }
                currentGameState = whiteResult.GameState;

                var blackResult = blackGamePlayer.MakeMove(PieceColour.Black, currentGameState);
                if (blackResult.MoveStatus == MoveStatus.NoLegalMoves)
                {
                    outcome = GameMatchOutcome.WhiteWin;
                    break;
                }
                currentGameState = blackResult.GameState;
                turns++;
            }
            return outcome;
        }
    }
}
