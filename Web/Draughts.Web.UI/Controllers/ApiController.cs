﻿using Draughts.Service;
using Draughts.Web.UI.Domain;
using Draughts.Web.UI.Mapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Draughts.Web.UI.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {

        private static ConcurrentDictionary<string, HumanPlayer> humanPlayers = new ConcurrentDictionary<string, HumanPlayer>();

        [HttpPost]
        [HttpGet]
        [Route("api/hello")]
        public string Hello()
        {
            return "hello world";
        }

        [HttpGet]
        [Route("api/game/create")]
        public string CreateGame()
        {
            var playerId = Guid.NewGuid().ToString();
            var humanPlayer = new HumanPlayer();
            var match = new GameMatch(GameStateFactory.StandardStartGameState(), humanPlayer, Program.AiOpponent);

            humanPlayer.GameMatch = match;
            humanPlayer.PieceColour = Service.PieceColour.White;

            humanPlayers.TryAdd(playerId, humanPlayer);

            return playerId;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GameBoard))]
        [ProducesResponseType(404)]
        [Route("api/game")]
        public IActionResult Game([FromQuery]string playerId)
        {
            HumanPlayer player;
            if (humanPlayers.TryGetValue(playerId, out player))
            {
                var hasMoves = player.GameMatch.GameState.CalculateAvailableMoves(player.PieceColour).Any();
                if (!hasMoves)
                {
                    player.GameMatch.CompleteTurn();
                }

                var gameBoard = new GameBoardMapper().Map(player.GameMatch, player.PieceColour);

                return Ok(gameBoard);
            }
            else
            {
                return NotFound($"Player with ID {playerId} not found.");
            }
        }

        [HttpPost]
        [Route("api/game")]
        public GameBoard PlayTurn([FromBody]MoveRequest moveRequest)
        {
            try
            {
                var player = humanPlayers[moveRequest.PlayerId];

                var match = player.GameMatch;

                GameState gameState = match.GameState;

                if (match.CurrentTurn == player.PieceColour)
                {

                    var moves = match.GameState.CalculateAvailableMoves(player.PieceColour);
                    var moveToPlay = moves.FirstOrDefault(m =>
                        m.StartGamePiece.Xcoord == moveRequest.StartX &&
                        m.StartGamePiece.Ycoord == moveRequest.StartY &&
                        m.EndGamePiece.Xcoord == moveRequest.EndX &&
                        m.EndGamePiece.Ycoord == moveRequest.EndY);

                    if (moveToPlay != null)
                    {
                        player.SelectedMove = moveToPlay;
                        match.CompleteTurn();

                        gameState = match.GameState;
                    }
                    else
                    {
                        throw new Exception("No moves found");
                    }
                }

                var gameBoard = new GameBoardMapper().Map(match, player.PieceColour);

                // find if opponent is ai and decide whether to play turn.
                IGamePlayer opponent;
                if (player.PieceColour == Service.PieceColour.White)
                {
                    opponent = match.BlackGamePlayer;
                }
                else
                {
                    opponent = match.WhiteGamePlayer;
                }

                if (!(opponent is HumanPlayer))
                {
                    match.CompleteTurn();
                }

                return gameBoard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
