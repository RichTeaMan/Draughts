﻿using Draughts.Ai.Trainer;
using NameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Service
{
    public class WeightedAiGamePlayer : IGamePlayer
    {
        public double NextAvailableMoveCountWeight { get; private set; }

        public double NextMovePiecesAtRiskWeight { get; private set; }

        public double NextMovePiecesToTakeWeight { get; private set; }

        public double KingWeight { get; private set; }

        public double NextMoveKingWeight { get; private set; }

        public int Generation { get; }

        public string Name
        {
            get { return this.GenerateName(); }
        }

        public WeightedAiGamePlayer(
            double nextAvailableMoveCountWeight,
            double nextMovePiecesAtRiskWeight,
            double nextMovePiecesToTakeWeight,
            double kingWeight,
            double nextMoveKingWeight,
            int generation)
        {
            NextAvailableMoveCountWeight = nextAvailableMoveCountWeight;
            NextMovePiecesAtRiskWeight = nextMovePiecesAtRiskWeight;
            NextMovePiecesToTakeWeight = nextMovePiecesToTakeWeight;
            KingWeight = kingWeight;
            NextMoveKingWeight = nextMoveKingWeight;
            Generation = generation;
        }

        public GamePlayerMoveResult MakeMove(PieceColour pieceColour, GameState gameState)
        {
            var currentMoves = gameState.CalculateAvailableMoves().Where(m => m.StartGamePiece.PieceColour == pieceColour).ToList();
            if (!currentMoves.Any())
            {
                return new GamePlayerMoveResult(null, MoveStatus.NoLegalMoves);
            }

            var moveWeights = new List<Tuple<double, GameMove>>();
            foreach (var move in currentMoves)
            {
                double weightedResult = 0;
                if (move.StartGamePiece.PieceRank == PieceRank.Minion && move.EndGamePiece.PieceRank == PieceRank.King)
                {
                    weightedResult += KingWeight;
                }

                var futureMoves = move.PerformMove().CalculateAvailableMoves();
                var friendlyMoves = futureMoves.Where(m => m.StartGamePiece.PieceColour == pieceColour).ToList();
                var opponentMoves = futureMoves.Where(m => m.StartGamePiece.PieceColour != pieceColour).ToList();


                weightedResult += NextAvailableMoveCountWeight * friendlyMoves.Count;
                weightedResult += NextMovePiecesAtRiskWeight * opponentMoves.Sum(m => m.TakenGamePieces.Count);
                weightedResult += NextMovePiecesToTakeWeight * friendlyMoves.Sum(m => m.TakenGamePieces.Count);
                weightedResult += NextMoveKingWeight * friendlyMoves.Count(
                    m => m.StartGamePiece.PieceRank == PieceRank.Minion &&
                    m.EndGamePiece.PieceRank == PieceRank.King);

                var resultTuple = new Tuple<double, GameMove>(weightedResult, move);
                moveWeights.Add(resultTuple);
            }

            var selectedMove = moveWeights.OrderByDescending(m => m.Item1).First().Item2;
            var selectedGameState = selectedMove.PerformMove();
            var result = new GamePlayerMoveResult(selectedGameState, MoveStatus.SuccessfulMove);
            return result;
        }

    }
}
