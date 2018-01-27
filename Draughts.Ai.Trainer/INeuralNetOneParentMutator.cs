﻿using RichTea.NeuralNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Draughts.Ai.Trainer
{
    public interface INeuralNetOneParentMutator : INeuralNetMutator
    {
        Net GenetateMutatedNeuralNet(Net parentNet);
    }
}
