using Draughts.Ai.Trainer;
using NameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public interface IAiGamePlayerSpawner
    {
        IAiGamePlayer SpawnAiGamePlayer();

        IAiGamePlayer SpawnDerivedAiGamePlayer(IGamePlayer player);

    }
}
