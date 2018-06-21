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
        private List<GameState> _gameStateList = new List<GameState>();

        public const int TurnLimit = 100;

        public GameState GameState { get; private set; }

        /// <summary>
        /// Gets a list of game states representing every turn in the game. The first turn is the first game state.
        /// </summary>
        public IReadOnlyList<GameState> GameStateList => _gameStateList;

        public IGamePlayer WhiteGamePlayer { get; private set; }

        public IGamePlayer BlackGamePlayer { get; private set; }

        public PieceColour CurrentTurn { get; private set; } = PieceColour.White;

        public int TurnCount { get; private set; } = 0;

        public GameMatchOutcome GameMatchOutcome { get; private set; } = GameMatchOutcome.InProgress;

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
                    if (turnResult.MoveStatus == MoveStatus.NoLegalMoves || turnResult.MoveStatus == MoveStatus.Resign)
                    {
                        GameMatchOutcome = winOutcome;
                    }
                    else
                    {
                        GameState = turnResult.GameState;
                        _gameStateList.Add(GameState);
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
            while (CompleteTurn() == GameMatchOutcome.InProgress) ;
            return GameMatchOutcome;
        }

    }
}
