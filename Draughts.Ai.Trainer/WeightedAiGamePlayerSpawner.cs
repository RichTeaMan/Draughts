using Draughts.Ai.Trainer;
using NameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class WeightedAiGamePlayerSpawner
    {
        double mutationFactor = 0.001;

        public string Name
        {
            get { return this.GenerateName(); }
        }

        private Random _random;

        public WeightedAiGamePlayerSpawner() : this(new Random())
        {
        }

        public WeightedAiGamePlayerSpawner(Random random)
        {
            _random = random;
        }

        public WeightedAiGamePlayer SpawnManualWeightedAiGamePlayer()
        {
            var nextAvailableMoveCountWeight = 0.1;
            var opponentAvailableMoveCountWeight = 0.1;
            var nextMovePiecesAtRiskWeight = -0.5;
            var nextMovePiecesToTakeWeight = 0.2;
            var kingWeight = 0.5;
            var nextMoveKingWeight = 0.5;
            var opponentNextMoveKingWeight = -0.5;
            var generation = 0;

            var newPlayer = new WeightedAiGamePlayer(
                nextAvailableMoveCountWeight,
                opponentAvailableMoveCountWeight,
                nextMovePiecesAtRiskWeight,
                nextMovePiecesToTakeWeight,
                kingWeight,
                nextMoveKingWeight,
                opponentNextMoveKingWeight,
                generation);
            return newPlayer;
        }

        public WeightedAiGamePlayer SpawnNewWeightedAiGamePlayer()
        {
            var nextAvailableMoveCountWeight = (_random.NextDouble() * 2) - 1;
            var opponentAvailableMoveCountWeight = (_random.NextDouble() * 2) - 1;
            var nextMovePiecesAtRiskWeight = (_random.NextDouble() * 2) - 1;
            var nextMovePiecesToTakeWeight = (_random.NextDouble() * 2) - 1;
            var kingWeight = (_random.NextDouble() * 2) - 1;
            var nextMoveKingWeight = (_random.NextDouble() * 2) - 1;
            var opponentNextMoveKingWeight = (_random.NextDouble() * 2) - 1;
            var generation = 0;

            var newPlayer = new WeightedAiGamePlayer(
                nextAvailableMoveCountWeight,
                opponentAvailableMoveCountWeight,
                nextMovePiecesAtRiskWeight,
                nextMovePiecesToTakeWeight,
                kingWeight,
                nextMoveKingWeight,
                opponentNextMoveKingWeight,
                generation);
            return newPlayer;
        }

        public WeightedAiGamePlayer SpawnNewWeightedAiGamePlayer(WeightedAiGamePlayer player)
        {
            var newPlayer = new WeightedAiGamePlayer(
                _random.NextDouble(player.NextAvailableMoveCountWeight, mutationFactor),
                _random.NextDouble(player.OpponentNextAvailableMoveCountWeight, mutationFactor),
                _random.NextDouble(player.NextMovePiecesAtRiskWeight, mutationFactor),
                _random.NextDouble(player.NextMovePiecesToTakeWeight, mutationFactor),
                _random.NextDouble(player.KingWeight, mutationFactor),
                _random.NextDouble(player.NextMoveKingWeight, mutationFactor),
                _random.NextDouble(player.OpponentNextMoveKingWeight, mutationFactor),
                player.Generation + 1);
            return newPlayer;
        }

    }
}
