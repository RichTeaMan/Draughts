using RichTea.NeuralNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Draughts.Ai.Trainer
{
    public interface INeuralNetTwoParentMutator : INeuralNetMutator
    {
        Net GenetateMutatedNeuralNet(Net firstParentNet, Net secondParentNet);
    }
}
