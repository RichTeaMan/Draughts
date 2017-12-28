using Common;
using Draughts.Service;

namespace Draughts.UI.Wpf.Setup
{
    public class GameParameter
    {
        public PieceColour PieceColour { get; }

        public PlayerType PlayerType { get; }

        public string FilePath { get; }

        public GameParameter(PieceColour pieceColour, PlayerType playerType, string filePath = null)
        {
            PieceColour = pieceColour;
            PlayerType = playerType;
            FilePath = filePath;
        }

        public override string ToString()
        {
            return new ToStringBuilder<GameParameter>(this)
                .Append(p => p.PieceColour)
                .Append(p => p.PlayerType)
                .Append(p => p.FilePath)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<GameParameter>(this, that)
                .Append(p => p.PieceColour)
                .Append(p => p.PlayerType)
                .Append(p => p.FilePath)
                .Equals();
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<GameParameter>(this)
                .Append(p => p.PieceColour)
                .Append(p => p.PlayerType)
                .Append(p => p.FilePath)
                .HashCode;
        }
    }
}
