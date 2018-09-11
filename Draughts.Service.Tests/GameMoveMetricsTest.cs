using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Service.Tests
{
    [TestClass]
    public class GameMoveMetricsTest
    {

        [TestMethod]
        public void CreatedFriendlyKingsTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 6),
                new GamePiece(PieceColour.White, PieceRank.King, 1, 7),
                new List<GamePiece>(),
                new GameState(null,
                new[] {
                    new GamePiece(PieceColour.White, PieceRank.Minion, 0, 6),
                    new GamePiece(PieceColour.White, PieceRank.King, 1, 7)
                },
                8, 8));

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(1, metrics.CreatedFriendlyKings);
        }


        [TestMethod]
        public void FriendlyMovesAvailableTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(7, metrics.FriendlyMovesAvailable);
        }

        [TestMethod]
        public void OpponentMovesAvailableTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(7, metrics.OpponentMovesAvailable);
        }

        public int NextMoveFriendlyPiecesAtRisk { get; }

        public int NextMoveOpponentPiecesAtRisk { get; }

        public int NextMoveFriendlyKingsCreated { get; }

        public int NextMoveOpponentKingsCreated { get; }

        [TestMethod]
        public void OpponentPiecesTakenTest()
        {
            var pieceColour = PieceColour.White;
            var pieceRank = PieceRank.Minion;
            int x = 2;
            int y = 0;

            var startGamePiece = new GamePiece(pieceColour, pieceRank, x, y);
            var endGamePiece = new GamePiece(pieceColour, pieceRank, x + 2, y + 2);
            var blockingGamePiece = new GamePiece(PieceColour.Black, pieceRank, x + 1, y + 1);
            var gameState = GameStateFactory.SeveralPieceGameState(startGamePiece, blockingGamePiece);


            var move = new GameMove(startGamePiece, endGamePiece, new[] { blockingGamePiece }, gameState);

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(1, metrics.OpponentPiecesTaken);
        }

        public int TotalPieces { get; }

        [TestMethod]
        public void TotalPiecesTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(24, metrics.TotalPieces);
        }

        [TestMethod]
        public void TotalFriendlyPiecesTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(12, metrics.TotalFriendlyPieces);
        }

        [TestMethod]
        public void TotalOpponentPiecesTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(12, metrics.TotalOpponentPieces);
        }

        public int TotalMinionPieces { get; }

        public int TotalFriendlyMinionPieces { get; }

        public int TotalOpponentMinionPieces { get; }

        public int TotalKingPieces { get; }

        public int TotalFriendlyKingPieces { get; }

        public int TotalOpponentKingPieces { get; }

        [TestMethod]
        public void FriendlyMinionsHomeTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(12, metrics.FriendlyMinionsHome);
        }

        [TestMethod]
        public void OpponentMinionsHomeTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(0, metrics.OpponentMinionsHome);
        }

        [TestMethod]
        public void FriendlyMinionsAwayTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(0, metrics.FriendlyMinionsAway);
        }

        [TestMethod]
        public void OpponentMinionsAwayTest()
        {
            var move = new GameMove(
                new GamePiece(PieceColour.White, PieceRank.Minion, 0, 2),
                new GamePiece(PieceColour.White, PieceRank.Minion, 1, 3),
                new List<GamePiece>(),
                GameStateFactory.StandardStartGameState()
            );

            var metrics = move.CalculateGameMoveMetrics(PieceColour.White);

            Assert.AreEqual(12, metrics.OpponentMinionsAway);
        }
    }
}
