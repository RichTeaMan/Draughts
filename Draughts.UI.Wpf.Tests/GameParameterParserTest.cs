using System;
using Draughts.UI.Wpf.Setup;
using Draughts.Service;
using NUnit.Framework;

namespace Draughts.UI.Wpf.Tests
{
    [TestFixture]
    public class GameParameterParserTest
    {
        /// <summary>
        /// Class under test.
        /// </summary>
        private GameParameterParser gameParameterParser;

        [SetUp]
        public void Setup()
        {
            gameParameterParser = new GameParameterParser();
        }

        [Test]
        public void WhiteAiGameParameterParserTest()
        {
            // expected
            GameParameter expectedGameParameter = new GameParameter(PieceColour.White, PlayerType.AI, "data.json");

            // test
            var parseResult = gameParameterParser.ParseParameter("w", new[] { "ai", "data.json" });
            GameParameter actualGameParameter = parseResult.Parameter as GameParameter;

            // assert
            Assert.AreEqual(expectedGameParameter, actualGameParameter);
        }

        [Test]
        public void BlackHumanGameParameterParserTest()
        {
            // expected
            GameParameter expectedGameParameter = new GameParameter(PieceColour.Black, PlayerType.Human);

            // test
            var parseResult = gameParameterParser.ParseParameter("b", new[] { "human" });
            GameParameter actualGameParameter = parseResult.Parameter as GameParameter;

            // assert
            Assert.AreEqual(expectedGameParameter, actualGameParameter);
        }
    }
}
