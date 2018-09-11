using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Draughts.Service.Tests
{
    [TestClass]
    public class GameStateTest
    {

        [TestMethod]
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

        [TestMethod]
        public void GameStateEquals()
        {
            var pieceA = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);
            var pieceAClone = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);

            var pieceB = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);
            var pieceBClone = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);

            var gameState = new GameState(null, new[] { pieceA, pieceB }, 8, 8);
            var gameStateClone = new GameState(null, new[] { pieceAClone, pieceBClone }, 8, 8);

            Assert.AreEqual(gameState, gameStateClone);
        }

        [TestMethod]
        public void GameStateNotEquals()
        {
            var pieceA = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);
            var pieceAClone = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);

            var pieceB = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);
            var pieceBBadClone = new GamePiece(PieceColour.Black, PieceRank.Minion, 0, 0);

            var gameState = new GameState(null, new[] { pieceA, pieceB }, 8, 8);
            var gameStateClone = new GameState(null, new[] { pieceAClone, pieceBBadClone }, 8, 8);

            Assert.AreNotEqual(gameState, gameStateClone);
        }

        [TestMethod]
        public void GameStateHashEquals()
        {
            var pieceA = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);
            var pieceAClone = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);

            var pieceB = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);
            var pieceBClone = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);

            var gameState = new GameState(null, new[] { pieceA, pieceB }, 8, 8);
            var gameStateClone = new GameState(null, new[] { pieceAClone, pieceBClone }, 8, 8);

            Assert.AreEqual(gameState.GetHashCode(), gameStateClone.GetHashCode());
        }

        [TestMethod]
        public void GameStateHashNotEquals()
        {
            var pieceA = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);
            var pieceAClone = new GamePiece(PieceColour.Black, PieceRank.King, 5, 5);

            var pieceB = new GamePiece(PieceColour.White, PieceRank.Minion, 0, 0);
            var pieceBBadClone = new GamePiece(PieceColour.Black, PieceRank.Minion, 0, 0);

            var gameState = new GameState(null, new[] { pieceA, pieceB }, 8, 8);
            var gameStateClone = new GameState(null, new[] { pieceAClone, pieceBBadClone }, 8, 8);

            Assert.AreNotEqual(gameState.GetHashCode(), gameStateClone.GetHashCode());
        }

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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void SinglePieceWhiteKingOnLastRowAfterJump()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 0;
            int y = 5;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece = new GamePiece(PieceColour.Black, pieceRank, 1, 6);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var afterState = moves.First().PerformMove();

            var expectedPiece = new GamePiece(PieceColour.White, PieceRank.King, 2, 7);

            Assert.AreEqual(1, afterState.GamePieceList.Count);

            Assert.AreEqual(expectedPiece, afterState.GamePieceList[0]);
        }

        /// <summary>
        /// Tests if a king is created after jumping to the last row, but cannot jump another piece in the same turn.
        /// </summary>
        [TestMethod]
        public void SinglePieceWhiteKingOnLastRowAfterJumpCantJumpInSameTurn()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 0;
            int y = 5;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece1 = new GamePiece(PieceColour.Black, pieceRank, 1, 6);
            var blockingGamePiece2 = new GamePiece(PieceColour.Black, pieceRank, 3, 6);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece1, blockingGamePiece2);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var afterState = moves.First().PerformMove();

            var expectedPiece = new GamePiece(PieceColour.White, PieceRank.King, 2, 7);

            var actualPiece = afterState.GamePieceList.Single(p => p.PieceColour == PieceColour.White);

            Assert.AreEqual(2, afterState.GamePieceList.Count);

            Assert.AreEqual(expectedPiece, actualPiece);
        }

        [TestMethod]
        public void SinglePieceWhiteKingOnLastRowAfterDoubleJump()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 3;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece1 = new GamePiece(PieceColour.Black, pieceRank, 1, 6);
            var blockingGamePiece2 = new GamePiece(PieceColour.Black, pieceRank, 1, 4);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece1, blockingGamePiece2);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var afterState = moves.First().PerformMove();

            var expectedPiece = new GamePiece(PieceColour.White, PieceRank.King, 2, 7);

            Assert.AreEqual(1, afterState.GamePieceList.Count);

            Assert.AreEqual(expectedPiece, afterState.GamePieceList[0]);
        }

        [TestMethod]
        public void SinglePieceWhiteKingOnLastRowAfterDoubleJumpCheckMoveObjects()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.King;
            int x = 3;
            int y = 7;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece1 = new GamePiece(PieceColour.Black, pieceRank, 4, 6);
            var blockingGamePiece2 = new GamePiece(PieceColour.Black, pieceRank, 6, 6);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece1, blockingGamePiece2);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var expectedPiece = new GamePiece(PieceColour.White, PieceRank.King, 7, 7);

            Assert.AreEqual(2, moves.First().TakenGamePieces.Count);
            Assert.AreEqual(gamePiece, moves.First().StartGamePiece);
            Assert.AreEqual(expectedPiece, moves.First().EndGamePiece);
        }

        [TestMethod]
        public void SinglePieceWhiteKingOnLastRowAfterTripleJumpCheckMoveObjects()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.King;
            int x = 1;
            int y = 7;

            var gamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var blockingGamePiece1 = new GamePiece(PieceColour.Black, pieceRank, 2, 6);
            var blockingGamePiece2 = new GamePiece(PieceColour.Black, pieceRank, 4, 6);
            var blockingGamePiece3 = new GamePiece(PieceColour.Black, pieceRank, 6, 6);
            var gameState = GameStateFactory.SeveralPieceGameState(gamePiece, blockingGamePiece1, blockingGamePiece2, blockingGamePiece3);

            var moves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == PieceColour.White).ToList();

            var expectedPiece = new GamePiece(PieceColour.White, PieceRank.King, 7, 5);

            Assert.AreEqual(3, moves.First().TakenGamePieces.Count);
            Assert.AreEqual(gamePiece, moves.First().StartGamePiece);
            Assert.AreEqual(expectedPiece, moves.First().EndGamePiece);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
