using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GameMove
    {
        public GamePiece StartGamePiece { get; }

        public GamePiece EndGamePiece { get; }

        public IReadOnlyCollection<GamePiece> TakenGamePieces { get; }

        public GameState GameState { get; }

        public GameMove (
            GamePiece startGamePiece,
            GamePiece endGamePiece,
            IEnumerable<GamePiece> takenGamePieces,
            GameState gameState)
        {
            StartGamePiece = startGamePiece;
            EndGamePiece = endGamePiece;
            TakenGamePieces = takenGamePieces.ToList();
            GameState = gameState;
        }

        public GameState PerformMove()
        {
            var previousPieces = GameState.GamePieceList;

            var newPieces = previousPieces.Where(p => p != StartGamePiece && !TakenGamePieces.Contains(p)).ToList();
            newPieces.Add(EndGamePiece);

            var newGameState = new GameState(newPieces, GameState.XLength, GameState.YLength);
            return newGameState;
        }
    }
}
