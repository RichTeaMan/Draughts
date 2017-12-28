using Draughts.Service;
using Draughts.UI.Wpf.Services;
using RichTea.CommandLineParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.UI.Wpf.Setup
{
    public class SetupService
    {
        public static IReadOnlyCollection<GameParameter> GameParameterList { get; private set; }

        [DefaultClCommand]
        public static void DefaultGameSetup(
            [ClArgs("white", "w")]
            GameParameter whiteGameParameter,
            [ClArgs("black", "b")]
            GameParameter blackGameParameter
            )
        {
            var gameParameterList = new List<GameParameter>
            {
                whiteGameParameter,
                blackGameParameter
            };

            GameParameterList = gameParameterList;
        }

        public static IGamePlayer FetchWhitePlayer()
        {
            var whiteParameter = GameParameterList.Single(p => p.PieceColour == PieceColour.White);
            return FetchPlayer(whiteParameter);
        }

        public static IGamePlayer FetchBlackPlayer()
        {
            var blackParameter = GameParameterList.Single(p => p.PieceColour == PieceColour.Black);
            return FetchPlayer(blackParameter);
        }

        private static IGamePlayer FetchPlayer(GameParameter gameParameter)
        {
            IGamePlayer gamePlayer;
            switch (gameParameter.PlayerType)
            {
                case PlayerType.Human:
                    gamePlayer = new HumanPlayer();
                    break;
                case PlayerType.AI:
                    gamePlayer = new AiLoader().LoadFromJsonfile(gameParameter.FilePath).First();
                    break;
                default:
                    throw new ApplicationException($"Unknown player type '{gameParameter.PlayerType}'");
            }
            return gamePlayer;
        }
    }
}
