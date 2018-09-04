using Draughts.Ai.Trainer;
using Draughts.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Draughts.Web.UI
{
    public class Program
    {
        public static IGamePlayer AiOpponent { get; private set; }

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine($"Building nets from {args[1]}.");
                try
                {
                    var fileContents = File.ReadAllText(args[1]);
                    var players = JsonConvert.DeserializeObject<object[]>(fileContents, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    var ais = players
                        .Where(p => p is SerialisableNeuralNetAiGamePlayer)
                        .Cast<SerialisableNeuralNetAiGamePlayer>()
                        .Select(player => player.CreateNeuralNetAiGamePlayer())
                        .ToList()
                        .Distinct()
                        .ToList();

                    AiOpponent = ais.First();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load nets:");
                    Console.WriteLine(ex);
                    throw ex;
                }
            }

            if (AiOpponent == null)
            {
                Console.WriteLine("The opponent is a random player.");
                AiOpponent = new RandomGamePlayer();
            }
            else
            {
                Console.WriteLine($"The opponent is neural net '{AiOpponent.Name}'.");
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
