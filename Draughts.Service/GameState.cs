using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class GameState
    {
        public readonly IReadOnlyList<GamePiece> GamePieceList;

        public readonly int XLength;

        public readonly int YLength;

        public GameState(IEnumerable<GamePiece> gamePieceList, int xLength, int yLength)
        {
            GamePieceList = gamePieceList.ToList();
            XLength = xLength;
            YLength = yLength;
        }

        public ICollection<GameMove> CalculateAvailableMoves()
        {
            throw new NotImplementedException();
        }
    }
}
