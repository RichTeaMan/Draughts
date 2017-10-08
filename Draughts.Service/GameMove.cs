using Common;
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

        public override string ToString()
        {
            return new ToStringBuilder<GameMove>(this)
                .Append(p => p.StartGamePiece)
                .Append(p => p.EndGamePiece)
                .Append(p => p.TakenGamePieces)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<GameMove>(this, that)
                .Append(p => p.StartGamePiece)
                .Append(p => p.EndGamePiece)
                .Append(p => p.TakenGamePieces)
                .Append(p => p.GameState)
                .Equals();
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<GameMove>(this)
                .Append(p => p.StartGamePiece)
                .Append(p => p.EndGamePiece)
                .Append(p => p.TakenGamePieces)
                .Append(p => p.GameState)
            .HashCode;
        }
    }
}
