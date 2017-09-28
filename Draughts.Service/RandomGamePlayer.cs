﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    /// <summary>
    /// AI player that will always make a random (legal) move for its colour.
    /// </summary>
    public class RandomGamePlayer : IGamePlayer
    {
        private Random random = new Random();

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            var moves = gameState.CalculateAvailableMoves().Where(c => c.StartGamePiece.PieceColour == pieceColour).ToList();

            GamePlayerMoveResult gamePlayerMoveResult;
            if (moves.Count > 0)
            {
                int selection = random.Next(moves.Count);
                var resultState = moves[selection].PerformMove();
                gamePlayerMoveResult = new GamePlayerMoveResult(resultState, MoveStatus.SuccessfulMove);
            }
            else
            {
                gamePlayerMoveResult = new GamePlayerMoveResult(null, MoveStatus.NoLegalMoves);
            }
            return gamePlayerMoveResult;
        }
    }
}