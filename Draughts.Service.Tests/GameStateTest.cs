using System;
using System.Linq;
using NUnit.Framework;

namespace Draughts.Service.Tests
{
    [TestFixture]
    public class GameStateTest
    {

        [Test]
        public void PieceEquals()
        {
            var pieceA = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);
            var pieceAClone = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);

            var pieceB = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);
            var pieceBClone = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);

            Assert.AreEqual(pieceA, pieceAClone);
            Assert.AreEqual(pieceB, pieceBClone);

            Assert.AreNotEqual(pieceA, pieceBClone);
            Assert.AreNotEqual(pieceB, pieceAClone);
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void SinglePieceWhitePerformMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var afterState = moves.First().PerformMove();

            Assert.AreEqual(1, afterState.GamePieceList.Count);

            Assert.AreEqual(x - 1, afterState.GamePieceList[0].Xcoord);
            Assert.AreEqual(y + 1, afterState.GamePieceList[0].Ycoord);
        }

        [Test]
        public void SinglePieceWhiteTakePerformMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece = new GamePiece(PieceColour.Black, pieceRank, x + 1, y + 1);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var afterState = moves.First().PerformMove();

            Assert.AreEqual(1, afterState.GamePieceList.Count);

            Assert.AreEqual(x + 2, afterState.GamePieceList[0].Xcoord);
            Assert.AreEqual(y + 2, afterState.GamePieceList[0].Ycoord);
        }

        [Test]
        public void SinglePieceWhiteKingOnLastRow()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 0;
            int y = 6;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var afterState = moves.First().PerformMove();

            Assert.AreEqual(PieceRank.King, moves[0].EndGamePiece.PieceRank);
            Assert.AreEqual(PieceRank.King, afterState.GamePieceList[0].PieceRank);
        }

        [Test]
        public void SinglePieceKingWhiteGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.King;
            int x = 4;
            int y = 4;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            var piece1 = moves.Single(m => m.EndGamePiece.Xcoord == x - 1 && m.EndGamePiece.Ycoord == y + 1).EndGamePiece;
            var piece2 = moves.Single(m => m.EndGamePiece.Xcoord == x + 1 && m.EndGamePiece.Ycoord == y - 1).EndGamePiece;
            var piece3 = moves.Single(m => m.EndGamePiece.Xcoord == x + 1 && m.EndGamePiece.Ycoord == y + 1).EndGamePiece;
            var piece4 = moves.Single(m => m.EndGamePiece.Xcoord == x - 1 && m.EndGamePiece.Ycoord == y - 1).EndGamePiece;

            Assert.AreEqual(4, moves.Count);

            Assert.AreEqual(x - 1, piece1.Xcoord);
            Assert.AreEqual(y + 1, piece1.Ycoord);

            Assert.AreEqual(x + 1, piece2.Xcoord);
            Assert.AreEqual(y - 1, piece2.Ycoord);

            Assert.AreEqual(x + 1, piece3.Xcoord);
            Assert.AreEqual(y + 1, piece3.Ycoord);

            Assert.AreEqual(x - 1, piece4.Xcoord);
            Assert.AreEqual(y - 1, piece4.Ycoord);
        }

        [Test]
        public void SinglePieceWhiteBlockedByTopEdgeGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 7;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            Assert.AreEqual(0, moves.Count);
        }

        [Test]
        public void SinglePieceBlackBlockedByBottomEdgeGameMove()
        {
            var pieceColour = PieceColour.Black;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var gameState = GameStateFactory.SinglePieceGameState(gamePiece);

            var moves = gameState.CalculateAvailableMoves();

            Assert.AreEqual(0, moves.Count);
        }

        [Test]
        public void SinglePieceWhiteTakeStraightChainGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 0;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece1 = new GamePiece(PieceColour.Black, pieceRank, x + 1, y + 1);
            var blockingGamePiece2 = new GamePiece(PieceColour.Black, pieceRank, x + 3, y + 3);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece1, blockingGamePiece2);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var piece1 = moves[0].EndGamePiece;

            Assert.AreEqual(1, moves.Count);

            Assert.AreEqual(x + 4, piece1.Xcoord);
            Assert.AreEqual(y + 4, piece1.Ycoord);
        }

        [Test]
        public void SinglePieceWhiteTakeStraightCrookedGameMove()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece1 = new GamePiece(PieceColour.Black, pieceRank, x + 1, y + 1);
            var blockingGamePiece2 = new GamePiece(PieceColour.Black, pieceRank, x + 1, y + 3);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece1, blockingGamePiece2);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var piece1 = moves[0].EndGamePiece;

            Assert.AreEqual(1, moves.Count);

            Assert.AreEqual(x, piece1.Xcoord);
            Assert.AreEqual(y + 4, piece1.Ycoord);
        }
    }
}
