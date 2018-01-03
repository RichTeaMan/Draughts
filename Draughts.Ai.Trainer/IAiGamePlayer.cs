using Draughts.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Draughts.Ai.Trainer
{
    public interface IAiGamePlayer : IGamePlayer
    {
        int Generation { get; }

        object CreateObjectForSerialisation();
    }
}
