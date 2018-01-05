using Draughts.Ai.Trainer;
using Draughts.Service;
using Draughts.UI.Wpf.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.UI.Wpf.Tests
{
    [TestClass]
    public class AiLoaderTest
    {
        /// <summary>
        /// Class under test.
        /// </summary>
        private AiLoader aiLoader;

        private ContestantSerialiser contestantSerialiser;

        [TestInitialize]
        public void Setup()
        {
            aiLoader = new AiLoader();

            contestantSerialiser = new ContestantSerialiser();
        }

        [TestMethod]
        public void LoadWeightedAiTest()
        {
            // Setup
            WeightedAiGamePlayerSpawner weightedAiGamePlayerSpawner = new WeightedAiGamePlayerSpawner();
            var weightedAi = weightedAiGamePlayerSpawner.SpawnNewWeightedAiGamePlayer();
            var contestant = new Contestant(weightedAi);

            var json = contestantSerialiser.SerialiseContestants(new [] { contestant });

            // Test
            var loadedAiList = aiLoader.LoadFromJson(json);

            Assert.AreEqual(weightedAi.CreateObjectForSerialisation(), loadedAiList.Single().CreateObjectForSerialisation());
        }

        [TestMethod]
        public void LoadNeuralNetAiTest()
        {
            // Setup
            NeuralNetAiGamePlayerSpawner neuralNetAiGamePlayerSpawner = new NeuralNetAiGamePlayerSpawner();
            var neuralNetAi = neuralNetAiGamePlayerSpawner.SpawnNewNeuralNetAiGamePlayer();
            var contestant = new Contestant(neuralNetAi);

            var json = contestantSerialiser.SerialiseContestants(new[] { contestant });

            // Test
            var loadedAiList = aiLoader.LoadFromJson(json);

            Assert.IsInstanceOfType(loadedAiList.Single(), typeof(NeuralNetAiGamePlayer));
            Assert.AreEqual(neuralNetAi.CreateObjectForSerialisation(), loadedAiList.Single().CreateObjectForSerialisation());
        }
    }
}
