using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Ai.Trainer
{
    public class ContestantSerialiser
    {
        public string SerialiseContestants(IEnumerable<Contestant> contestants)
        {
            var json = JsonConvert.SerializeObject(
                contestants.Select(c => c.GamePlayer.CreateObjectForSerialisation()).ToArray(),
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            return json;
        }
    }
}
