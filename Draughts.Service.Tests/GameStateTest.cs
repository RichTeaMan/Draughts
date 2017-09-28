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

        [TestMethod]
        public void SinglePieceWhiteGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var piece1 = moves[0].EndGamePiece;
            var piece2 = moves[1].EndGamePiece;

            Assert.AreEqual(2, moves.Count);

            Assert.AreEqual(x - 1, piece1.Xcoord);
            Assert.AreEqual(y + 1, piece1.Ycoord);

            Assert.AreEqual(x + 1, piece2.Xcoord);
            Assert.AreEqual(y + 1, piece2.Ycoord);
        }

        [TestMethod]
        public void SinglePieceWhiteLeftEdgeGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 0;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var piece1 = moves[0].EndGamePiece;

            Assert.AreEqual(1, moves.Count);

            Assert.AreEqual(x + 1, piece1.Xcoord);
            Assert.AreEqual(y + 1, piece1.Ycoord);
        }

        [TestMethod]
        public void SinglePieceBlackGameMove()
        {
            var pieceColour = PieceColour.Black;
            var pieceRank = PieceRank.Minion;
            int x = 5;
            int y = 7;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var piece1 = moves[0].EndGamePiece;
            var piece2 = moves[1].EndGamePiece;

            Assert.AreEqual(2, moves.Count);

            Assert.AreEqual(x - 1, piece1.Xcoord);
            Assert.AreEqual(y - 1, piece1.Ycoord);

            Assert.AreEqual(x + 1, piece2.Xcoord);
            Assert.AreEqual(y - 1, piece2.Ycoord);
        }

        [TestMethod]
        public void SinglePieceBlackLeftEdgeGameMove()
        {
            var pieceColour = PieceColour.Black;
            var pieceRank = PieceRank.Minion;
            int x = 7;
            int y = 7;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var piece1 = moves[0].EndGamePiece;

            Assert.AreEqual(1, moves.Count);

            Assert.AreEqual(x - 1, piece1.Xcoord);
            Assert.AreEqual(y - 1, piece1.Ycoord);
        }

        [TestMethod]
        public void SinglePieceWhiteBlockedGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece = new GamePiece(pieceColour, pieceRank, x + 1, y + 1);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var piece1 = moves[0].EndGamePiece;

            Assert.AreEqual(3, moves.Count);

            Assert.AreEqual(x - 1, piece1.Xcoord);
            Assert.AreEqual(y + 1, piece1.Ycoord);
        }

        [TestMethod]
        public void SinglePieceWhiteTakeGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece = new GamePiece(PieceColour.Black, pieceRank, x + 1, y + 1);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var piece1 = moves[0].EndGamePiece;

            Assert.AreEqual(1, moves.Count);

            Assert.AreEqual(x + 2, piece1.Xcoord);
            Assert.AreEqual(y + 2, piece1.Ycoord);
        }
    }
}
