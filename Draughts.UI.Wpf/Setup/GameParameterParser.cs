using Draughts.Service;
using RichTea.CommandLineParser.ParameterParsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.UI.Wpf.Setup
{
    public class GameParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(GameParameter) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var result = new ParsedResult();

            bool error = false;
            PieceColour? pieceColour = null;
            PlayerType? playerType = null;
            string filePath = null;

            string lowArgFlag = argumentFlag.ToLower();
            if (lowArgFlag == "w" || lowArgFlag == "white")
            {
                pieceColour = PieceColour.White;
            }
            else if (lowArgFlag == "b" || lowArgFlag == "black")
            {
                pieceColour = PieceColour.Black;
            }
            else
            {
                error = true;
                result.ErrorOutput.Add(new ParserOutput($"Unknown player colour '{argumentFlag}'. Only 'black' or 'white' are supported."));
            }

            string firstArg = arguments.FirstOrDefault();
            switch (firstArg.ToLower())
            {
                case "human":
                    playerType = PlayerType.Human;
                    break;
                case "ai":
                    playerType = PlayerType.AI;
                    break;
                case "random":
                    playerType = PlayerType.Random;
                    break;
                default:
                    error = true;
                    result.ErrorOutput.Add(new ParserOutput($"Unknown player type '{firstArg}' specified. Try 'human' or'ai'."));
                    break;
            }

            if (PlayerType.AI == playerType)
            {
                if (arguments.Length >= 2)
                {
                    filePath = arguments[1];
                }
                else 
                {
                    error = true;
                    result.ErrorOutput.Add(new ParserOutput($"AI players must have a specified data file location."));
                }
            }

            if (!error)
            {
                result.Parameter = new GameParameter(pieceColour.Value, playerType.Value, filePath);
            }

            return result;
        }

    }
}
