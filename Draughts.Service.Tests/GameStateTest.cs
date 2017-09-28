using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Draughts.Service.Tests
{
    [TestClass]
    public class GameStateTest
    {
        [TestMethod]
        public void SinglePieceGameState()
        {
            var pieceColour = PieceColour.Black;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            Assert.AreEqual(1, gameState.GamePieceList.Count);
            Assert.AreEqual(pieceColour, gameState.GamePieceList.First().PieceColour);
            Assert.AreEqual(pieceRank, gameState.GamePieceList.First().PieceRank);
            Assert.AreEqual(x, gameState.GamePieceList.First().Xcoord);
            Assert.AreEqual(y, gameState.GamePieceList.First().Ycoord);
        }
    }
}
