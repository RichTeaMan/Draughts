using Draughts.Web.UI.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Web.UI.Mapper.Tests
{
    [TestClass]
    public class BoardRotaterMapperTest
    {
        private BoardRotaterMapper boardRotaterMapper;

        [TestInitialize]
        public void Setup()
        {
            boardRotaterMapper = new BoardRotaterMapper();
        }

        [TestMethod]
        public void RotateNullGameBoardTest()
        {
            var result = boardRotaterMapper.Rotate(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RotateNullMoveRequestTest()
        {
            var result = boardRotaterMapper.Rotate(null, 0, 0);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RotatePoint00Test()
        {
            var point = boardRotaterMapper.Rotate(0, 0, 8, 8);

            Assert.AreEqual(7, point.X);
            Assert.AreEqual(7, point.Y);
        }

        [TestMethod]
        public void RotatePoint23Test()
        {
            var point = boardRotaterMapper.Rotate(2, 3, 8, 8);

            Assert.AreEqual(5, point.X);
            Assert.AreEqual(4, point.Y);
        }

        [TestMethod]
        public void RotatePoint77Test()
        {
            var point = boardRotaterMapper.Rotate(7, 7, 8, 8);

            Assert.AreEqual(0, point.X);
            Assert.AreEqual(0, point.Y);
        }

        [TestMethod]
        public void RotateSinglePiece00Test()
        {
            var gamePiece = new GamePiece()
            {
                PieceColour = Constants.BLACK,
                PieceRank = Constants.MAN,
                Xcoord = 0,
                Ycoord = 0
            };
            var gamePieces = new List<GamePiece>();
            gamePieces.Add(gamePiece);

            var gameBoard = new GameBoard()
            {
                GamePieces = gamePieces.ToArray(),
                Height = 8,
                Width = 8
            };

            var rotatedGameBoard = boardRotaterMapper.Rotate(gameBoard);

            Assert.AreEqual(gamePieces.Count, rotatedGameBoard.GamePieces.Length);
            Assert.AreEqual(7, rotatedGameBoard.GamePieces.First().Xcoord);
            Assert.AreEqual(7, rotatedGameBoard.GamePieces.First().Ycoord);
            Assert.AreEqual(gamePiece.PieceColour, rotatedGameBoard.GamePieces.First().PieceColour);
            Assert.AreEqual(gamePiece.PieceRank, rotatedGameBoard.GamePieces.First().PieceRank);
        }

        [TestMethod]
        public void RotateSinglePiece01Test()
        {
            var gamePiece = new GamePiece()
            {
                PieceColour = Constants.WHITE,
                PieceRank = Constants.KING,
                Xcoord = 0,
                Ycoord = 1
            };
            var gamePieces = new List<GamePiece>();
            gamePieces.Add(gamePiece);

            var gameBoard = new GameBoard()
            {
                GamePieces = gamePieces.ToArray(),
                Height = 8,
                Width = 8
            };

            var rotatedGameBoard = boardRotaterMapper.Rotate(gameBoard);

            Assert.AreEqual(gamePieces.Count, rotatedGameBoard.GamePieces.Length);
            Assert.AreEqual(7, rotatedGameBoard.GamePieces.First().Xcoord);
            Assert.AreEqual(6, rotatedGameBoard.GamePieces.First().Ycoord);
            Assert.AreEqual(gamePiece.PieceColour, rotatedGameBoard.GamePieces.First().PieceColour);
            Assert.AreEqual(gamePiece.PieceRank, rotatedGameBoard.GamePieces.First().PieceRank);
        }

        [TestMethod]
        public void RotateSinglePiece10Test()
        {
            var gamePiece = new GamePiece()
            {
                PieceColour = Constants.BLACK,
                PieceRank = Constants.KING,
                Xcoord = 1,
                Ycoord = 0
            };
            var gamePieces = new List<GamePiece>();
            gamePieces.Add(gamePiece);

            var gameBoard = new GameBoard()
            {
                GamePieces = gamePieces.ToArray(),
                Height = 8,
                Width = 8
            };

            var rotatedGameBoard = boardRotaterMapper.Rotate(gameBoard);

            Assert.AreEqual(gamePieces.Count, rotatedGameBoard.GamePieces.Length);
            Assert.AreEqual(6, rotatedGameBoard.GamePieces.First().Xcoord);
            Assert.AreEqual(7, rotatedGameBoard.GamePieces.First().Ycoord);
            Assert.AreEqual(gamePiece.PieceColour, rotatedGameBoard.GamePieces.First().PieceColour);
            Assert.AreEqual(gamePiece.PieceRank, rotatedGameBoard.GamePieces.First().PieceRank);
        }

        [TestMethod]
        public void RotateSinglePieceWithMovesTest()
        {
            var gamePiece = new GamePiece()
            {
                PieceColour = Constants.BLACK,
                PieceRank = Constants.MAN,
                Xcoord = 1,
                Ycoord = 0
            };
            var gamePieces = new List<GamePiece>();
            gamePieces.Add(gamePiece);

            var gameMove = new GameMove()
            {
                StartX = 2,
                StartY = 3,
                EndX = 5,
                EndY = 4
            };
            var gameMoves = new List<GameMove>();
            gameMoves.Add(gameMove);

            var gameBoard = new GameBoard()
            {
                GamePieces = gamePieces.ToArray(),
                Height = 8,
                Width = 8,
                GameMoves = gameMoves.ToArray()
            };

            var rotatedGameBoard = boardRotaterMapper.Rotate(gameBoard);

            Assert.AreEqual(gamePieces.Count, rotatedGameBoard.GamePieces.Length);
            Assert.AreEqual(6, rotatedGameBoard.GamePieces.First().Xcoord);
            Assert.AreEqual(7, rotatedGameBoard.GamePieces.First().Ycoord);

            Assert.AreEqual(gameMoves.Count, rotatedGameBoard.GameMoves.Length);
            Assert.AreEqual(5, rotatedGameBoard.GameMoves.First().StartX);
            Assert.AreEqual(4, rotatedGameBoard.GameMoves.First().StartY);
            Assert.AreEqual(2, rotatedGameBoard.GameMoves.First().EndX);
            Assert.AreEqual(3, rotatedGameBoard.GameMoves.First().EndY);
        }

        [TestMethod]
        public void RotateMoveRequestTest()
        {
            var moveRequest = new MoveRequest()
            {
                StartX = 2,
                StartY = 3,
                EndX = 5,
                EndY = 4,
                PlayerId = Guid.NewGuid().ToString()
            };

            var rotatedMoveRequest = boardRotaterMapper.Rotate(moveRequest, 8, 8);

            Assert.AreEqual(5, rotatedMoveRequest.StartX);
            Assert.AreEqual(4, rotatedMoveRequest.StartY);
            Assert.AreEqual(2, rotatedMoveRequest.EndX);
            Assert.AreEqual(3, rotatedMoveRequest.EndY);
            Assert.AreEqual(moveRequest.PlayerId, rotatedMoveRequest.PlayerId);
        }
    }
}
