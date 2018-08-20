using Draughts.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Draughts.Ai.Trainer
{
    public interface IAiGamePlayer : IGamePlayer
    {
        int Generation { get; }

        /// <summary>
        /// Gets an identifierable name for the player.
        /// </summary>
        string Name { get; }

        object CreateObjectForSerialisation();
    }
}
