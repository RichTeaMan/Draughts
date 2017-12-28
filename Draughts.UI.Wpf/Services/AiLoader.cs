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

        public List<IGamePlayer> LoadFromJsonfile(string filePath)
        {
            var contents = File.ReadAllText(filePath);
            var players = JsonConvert.DeserializeObject<Contestant<WeightedAiGamePlayer>[]>(contents);

            var loadedGamePlayers = new List<IGamePlayer>(players.Select(p => p.GamePlayer));
            return loadedGamePlayers;
        }
    }
}
