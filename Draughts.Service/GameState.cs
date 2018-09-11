using RichTea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    /// <summary>
    /// An immutable representation of the game.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Gets the game state before the move played that created this game state. This could be null.
        /// </summary>
        public GameState PreviousGameState { get; }

        /// <summary>
        /// Gets a list of game pieces.
        /// </summary>
        public IReadOnlyList<GamePiece> GamePieceList { get; }

        /// <summary>
        /// Gets the width of the board.
        /// </summary>
        public int XLength { get; }

        /// <summary>
        /// Gets the height of the board.
        /// </summary>
        public int YLength { get; }

        /// <summary>
        /// Initialises an instance game state.
        /// </summary>
        /// <param name="previousGameState"></param>
        /// <param name="gamePieceList"></param>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public GameState(GameState previousGameState, IEnumerable<GamePiece> gamePieceList, int xLength, int yLength)
        {
            PreviousGameState = previousGameState;
            GamePieceList = gamePieceList.ToList();
            XLength = xLength;
            YLength = yLength;
        }

        private PieceRank CalculatePieceRank(int yPosition, PieceColour pieceColour, PieceRank pieceRank)
        {
            PieceRank resultPieceRank = PieceRank.Minion;
            if (pieceRank == PieceRank.King)
            {
                resultPieceRank = PieceRank.King;
            }
            else
            {
                switch (pieceColour)
                {
                    case PieceColour.White:
                        if (yPosition == YLength - 1)
                        {
                            resultPieceRank = PieceRank.King;
                        }
                        break;
                    case PieceColour.Black:
                        if (yPosition == 0)
                        {
                            resultPieceRank = PieceRank.King;
                        }
                        break;
                }
            }
            return resultPieceRank;
        }

        /// <summary>
        /// Calculates all available moves for both players that can be played with this state.
        /// </summary>
        /// <returns></returns>
        public IList<GameMove> CalculateAvailableMoves()
        {
            var moves = CalculateAvailableMoves(GamePieceList);
            return moves;
        }

        /// <summary>
        /// Calculates all available moves for the specified player that can be played with this state.
        /// </summary>
        /// <param name="pieceColour">Piece colour of the player.</param>
        /// <returns></returns>
        public IList<GameMove> CalculateAvailableMoves(PieceColour pieceColour)
        {
            var moves = CalculateAvailableMoves(GamePieceList.Where(p => p.PieceColour == pieceColour));
            return moves;
        }

        /// <summary>
        /// Calculates all available moves for the given pieces that can be played with this state.
        /// </summary>
        /// <param name="gamePieces">Pieces to find moves for.</param>
        /// <returns></returns>
        public IList<GameMove> CalculateAvailableMoves(IEnumerable<GamePiece> gamePieces)
        {
            var gameMoves = new List<GameMove>();
            foreach (var piece in gamePieces)
            {
                var pieceMoves = FindMovesForPiece(this, piece);
                gameMoves.AddRange(pieceMoves);
            }

            var resultGameMoves = new List<GameMove>();

            // pieces must be taken
            var colourGroupList = gameMoves.GroupBy(m => m.StartGamePiece.PieceColour);
            foreach (var colourGroup in colourGroupList)
            {
                if (colourGroup.Any(m => m.TakenGamePieces.Count > 0))
                {
                    resultGameMoves.AddRange(colourGroup.Where(m => m.TakenGamePieces.Count > 0));
                }
                else
                {
                    resultGameMoves.AddRange(colourGroup);
                }
            }

            return resultGameMoves;
        }

        private static List<GameMove> FindMovesForPiece(GameState gameState, GamePiece piece)
        {
            var gameMoves = new List<GameMove>();
            var newYList = new List<int>(4);

            if (piece.PieceRank == PieceRank.Minion)
            {
                int yDelta = 0;
                if (piece.PieceColour == PieceColour.White)
                {
                    yDelta = 1;
                }
                else if (piece.PieceColour == PieceColour.Black)
                {
                    yDelta = -1;
                }
                else
                {
                    throw new InvalidOperationException("Unknown piece colour.");
                }
                newYList.Add(piece.Ycoord + yDelta);
            }
            else if (piece.PieceRank == PieceRank.King)
            {
                newYList.Add(piece.Ycoord + 1);
                newYList.Add(piece.Ycoord - 1);
            }
            else
            {
                throw new InvalidOperationException("Unknown piece rank.");
            }

            foreach (var newY in newYList)
            {
                // check the y coord is in game bounds
                if (newY < 0 || newY >= gameState.YLength)
                {
                    continue;
                }

                var newXcoords = new List<int>(2);
                int newXleft = piece.Xcoord - 1;
                if (newXleft >= 0)
                {
                    newXcoords.Add(newXleft);
                }

                int newXright = piece.Xcoord + 1;
                if (newXright < gameState.XLength)
                {
                    newXcoords.Add(newXright);
                }

                foreach (var newX in newXcoords)
                {
                    var occupiedPiece = gameState.GamePieceList.FirstOrDefault(p => p.Xcoord == newX && p.Ycoord == newY);
                    if (occupiedPiece == null)
                    {
                        var newRank = gameState.CalculatePieceRank(newY, piece.PieceColour, piece.PieceRank);
                        var endPiece = new GamePiece(piece.PieceColour, newRank, newX, newY);
                        gameMoves.Add(new GameMove(piece, endPiece, new List<GamePiece>(), gameState));
                    }
                    else if (occupiedPiece.PieceColour != piece.PieceColour)
                    {
                        int jumpedX = ((newX - piece.Xcoord) * 2) + piece.Xcoord;
                        int jumpedY = ((newY - piece.Ycoord) * 2) + piece.Ycoord;
                        if (jumpedX >= 0 && jumpedX < gameState.XLength && jumpedY >= 0 && jumpedY < gameState.YLength)
                        {
                            var jumpedPiece = gameState.GamePieceList.FirstOrDefault(p => p.Xcoord == jumpedX && p.Ycoord == jumpedY);
                            if (jumpedPiece == null)
                            {
                                var newRank = gameState.CalculatePieceRank(jumpedY, piece.PieceColour, piece.PieceRank);
                                var endPiece = new GamePiece(piece.PieceColour, newRank, jumpedX, jumpedY);
                                var foundMove = new GameMove(piece, endPiece, new List<GamePiece>() { occupiedPiece }, gameState);
                                var chainedMoves = FindMovesForPiece(foundMove.PerformMove(), endPiece).Where(m => m.StartGamePiece == endPiece && m.TakenGamePieces.Any());

                                // Check rank doesn't change. Pieces should not move immediately after being promoted.
                                if (chainedMoves.Any() && newRank == piece.PieceRank) {
                                    // combine chain moves
                                    foreach (var chainMove in chainedMoves)
                                    {
                                        var takenPieces = new List<GamePiece>
                                        {
                                            occupiedPiece
                                        };
                                        takenPieces.AddRange(chainMove.TakenGamePieces);
                                        var gameMove = new GameMove(piece, chainMove.EndGamePiece, takenPieces, gameState);
                                        gameMoves.Add(gameMove);
                                    }
                                } else {
                                    gameMoves.Add(foundMove);
                                }
                            }
                        }
                    }
                }
            }
        
            return gameMoves;
        }

        public override string ToString()
        {
            return new ToStringBuilder<GameState>(this)
                .Append(p => p.GamePieceList)
                .Append(p => p.XLength)
                .Append(p => p.YLength)
                .ToString();
        }

        public override bool Equals(object that)
        {
            var otherGameState = that as GameState;
            return new EqualsBuilder<GameState>(this, that)
                .Append(GamePieceList, otherGameState?.GamePieceList)
                .Append(XLength, otherGameState?.XLength)
                .Append(YLength, otherGameState?.YLength)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(GamePieceList)
                .Append(XLength)
                .Append(YLength)
                .HashCode;
        }

    }
}
