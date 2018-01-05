using Draughts.Ai.Trainer;
using Draughts.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.UI.Wpf.Services
{
    public class AiLoader
    {
        public IGamePlayer CreateRandomPlayer()
        {
            return new RandomGamePlayer();
        }

        public List<IAiGamePlayer> LoadFromJsonfile(string filePath)
        {
            var contents = File.ReadAllText(filePath);
            var players = LoadFromJson(contents);
            return players;
        }

        public List<IAiGamePlayer> LoadFromJson(string json)
        {
            var players = JsonConvert.DeserializeObject<object[]>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            List<IAiGamePlayer> loadedPlayerList = new List<IAiGamePlayer>();
            foreach (var player in players)
            {
                if (player is WeightedAiGamePlayer weightedAiGamePlayer)
                {
                    loadedPlayerList.Add(weightedAiGamePlayer);
                }
                else if (player is SerialisableNeuralNetAiGamePlayer serialisableNeuralNetAiGamePlayer)
                {
                    var neuralNetAiGamePlayer = serialisableNeuralNetAiGamePlayer.CreateNeuralNetAiGamePlayer();
                    loadedPlayerList.Add(neuralNetAiGamePlayer);
                }
                else
                {
                    throw new Exception($"Unknown player type: '{player.GetType()}'");
                }
            }
            return loadedPlayerList;
        }
    }
}
