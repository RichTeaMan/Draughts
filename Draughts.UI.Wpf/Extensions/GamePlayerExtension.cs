using Draughts.Ai.Trainer;
using Draughts.Service;
using NameUtility;

namespace Draughts.UI.Wpf.Extensions
{
    public static class GamePlayerExtension
    {
        public static string PlayerSummary(this IGamePlayer gamePlayer)
        {
            string summary;
            if (gamePlayer is HumanPlayer)
            {
                summary = "Human player";
            }
            else if (gamePlayer is WeightedAiGamePlayer)
            {
                summary = $"Weighted AI player: {gamePlayer.GenerateName()}";
            }
            else if (gamePlayer is NeuralNetAiGamePlayer)
            {
                summary = $"Neural Net AI player: {gamePlayer.GenerateName()}";
            }
            else if (gamePlayer is RandomGamePlayer)
            {
                summary = "Random move player";
            }
            else
            {
                summary = "Unknown player";
            }
            return summary;
        }
    }
}
