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

        public GameMove (
            GamePiece startGamePiece,
            GamePiece endGamePiece,
            IEnumerable<GamePiece> takenGamePieces)
        {
            StartGamePiece = startGamePiece;
            EndGamePiece = endGamePiece;
            TakenGamePieces = takenGamePieces.ToList();
        }
    }
}
