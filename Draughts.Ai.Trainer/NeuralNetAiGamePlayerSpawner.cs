using Draughts.Ai.Trainer;
using RichTea.NeuralNetLib;
using System;

namespace Draughts.Service
{
    public class NeuralNetAiGamePlayerSpawner : IAiGamePlayerSpawner
    {
        double mutationFactor = 0.001;

        private Random _random;

        public NeuralNetAiGamePlayerSpawner() : this(new Random())
        {
        }

        public NeuralNetAiGamePlayerSpawner(Random random)
        {
            _random = random;
        }

        public IAiGamePlayer SpawnAiGamePlayer()
        {
            return SpawnNewNeuralNetAiGamePlayer();
        }

        public IAiGamePlayer SpawnDerivedAiGamePlayer(IGamePlayer player)
        {
            var neuralNetAiGamePlayer = player as NeuralNetAiGamePlayer;
            if (null == neuralNetAiGamePlayer) {
                throw new ArgumentException("Player must be of type NeuralNetAiGamePlayer.");
            }
            return SpawnNewNeuralNetAiGamePlayer(neuralNetAiGamePlayer);
        }

        public NeuralNetAiGamePlayer SpawnNewNeuralNetAiGamePlayer()
        {
            NetFactory netFactory = new NetFactory();
            Net net = netFactory.GenerateRandomNet(NeuralNetAiGamePlayer.NetInputs, 1, 8, _random);
            var generation = 0;

            var newPlayer = new NeuralNetAiGamePlayer(
                net,
                generation);
            return newPlayer;
        }

        public NeuralNetAiGamePlayer SpawnNewNeuralNetAiGamePlayer(NeuralNetAiGamePlayer player)
        {
            NetFactory netFactory = new NetFactory();
            Net net = netFactory.CreateMutatedNet(player.Net, _random, mutationFactor);

            var newPlayer = new NeuralNetAiGamePlayer(
                net,
                player.Generation + 1);
            return newPlayer;
        }

    }
}
