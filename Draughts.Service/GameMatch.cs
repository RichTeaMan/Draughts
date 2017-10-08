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

        public GameState GameState { get; private set; }

        public IGamePlayer WhiteGamePlayer { get; private set; }

        public IGamePlayer BlackGamePlayer { get; private set; }

        public PieceColour CurrentTurn { get; private set; } = PieceColour.White;

        public int TurnCount { get; private set; } = 0;

        public GameMatchOutcome GameMatchOutcome { get; private set; } = GameMatchOutcome.InProgress;

        [Obsolete]
        public GameMatch()
        {

        }

        /// <summary>
        /// Creates a game match with the given game and players.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="whiteGamePlayer"></param>
        /// <param name="blackGamePlayer"></param>
        public GameMatch(GameState gameState, IGamePlayer whiteGamePlayer, IGamePlayer blackGamePlayer)
        {
            GameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            WhiteGamePlayer = whiteGamePlayer ?? throw new ArgumentNullException(nameof(whiteGamePlayer));
            BlackGamePlayer = blackGamePlayer ?? throw new ArgumentNullException(nameof(blackGamePlayer));
        }

        /// <summary>
        /// Completes a turn.
        /// </summary>
        /// <returns></returns>
        public GameMatchOutcome CompleteTurn()
        {
            if (GameMatchOutcome == GameMatchOutcome.InProgress)
            {
                if (TurnCount < TurnLimit)
                {
                    IGamePlayer currentGamePlayer;
                    GameMatchOutcome winOutcome;
                    PieceColour nextTurn;
                    if (CurrentTurn == PieceColour.White)
                    {
                        currentGamePlayer = WhiteGamePlayer;
                        winOutcome = GameMatchOutcome.BlackWin;
                        nextTurn = PieceColour.Black;
                    }
                    else
                    {
                        currentGamePlayer = BlackGamePlayer;
                        winOutcome = GameMatchOutcome.WhiteWin;
                        nextTurn = PieceColour.White;
                    }
                    var turnResult = currentGamePlayer.MakeMove(CurrentTurn, GameState);
                    if (turnResult.MoveStatus == MoveStatus.NoLegalMoves)
                    {
                        GameMatchOutcome = winOutcome;
                    }
                    else
                    {
                        GameState = turnResult.GameState;
                        TurnCount++;
                        CurrentTurn = nextTurn;
                    }
                }
                else
                {
                    GameMatchOutcome = GameMatchOutcome.Draw;
                }
            }
            return GameMatchOutcome;
        }

        /// <summary>
        /// Completes a match between two players. White always plays first.
        /// </summary>
        /// <returns></returns>
        public GameMatchOutcome CompleteMatch()
        {
            while (CompleteTurn() != GameMatchOutcome.InProgress) ;
            return GameMatchOutcome;
        }

        /// <summary>
        /// Completes a match between two players. White always plays first.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="whiteGamePlayer"></param>
        /// <param name="blackGamePlayer"></param>
        /// <returns></returns>
        [Obsolete]
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
