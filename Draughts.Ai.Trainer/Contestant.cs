using Draughts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Draughts.Ai.Trainer
{
    public class Contestant<T> where T : IGamePlayer
    {
        private int _matches = 0;
        public int Matches => _matches;

        private int _wins;
        public int Wins => _wins;

        private int _draws;
        public int Draws => _draws;

        public T GamePlayer { get; private set; }

        public Contestant(T gamePlayer)
        {
            GamePlayer = gamePlayer;
        }

        public int IncrementMatch()
        {
            return Interlocked.Increment(ref _matches);
        }

        public int IncrementWin()
        {
            return Interlocked.Increment(ref _wins);
        }

        public int IncrementDraw()
        {
            return Interlocked.Increment(ref _draws);
        }

        public void ResetStats()
        {
            _matches = 0;
            _wins = 0;
            _draws = 0;
        }
    }
}
