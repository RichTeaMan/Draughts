using Common;
using Draughts.Service;
using NameUtility;
using NeuralNetLib;
using NeuralNetLib.Serialisation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Ai.Trainer
{
    public class SerialisableNeuralNetAiGamePlayer
    {
        public SerialisedNet Net { get; set; }

        public int Generation { get; set; }

        public NeuralNetAiGamePlayer CreateNeuralNetAiGamePlayer()
        {
            var net = Net.CreateNet();
            var neuralNetAiGamePlayer = new NeuralNetAiGamePlayer(net, Generation);
            return neuralNetAiGamePlayer;
        }

    }
}
