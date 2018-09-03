using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Draughts.Service;
using Draughts.Web.UI.Domain;
using Draughts.Web.UI.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Draughts.Web.UI.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {

        private static Dictionary<string, HumanPlayer> humanPlayers = new Dictionary<string, HumanPlayer>();

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
            var match = new GameMatch(GameStateFactory.StandardStartGameState(), humanPlayer, new RandomGamePlayer());

            humanPlayer.GameMatch = match;
            humanPlayer.PieceColour = Service.PieceColour.White;

            humanPlayers.Add(playerId, humanPlayer);

            return playerId;
        }

        [HttpGet]
        [Route("api/game")]
        public GameBoard Game([FromQuery]string playerId)
        {
            var player = humanPlayers[playerId];
            var gameState = player.GameMatch.GameState;

            var gameBoard = new GameBoardMapper().Map(gameState);

            return gameBoard;
        }

        [HttpPost]
        [Route("api/game")]
        public GameBoard PlayTurn([FromBody]MoveRequest moveRequest)
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

                    match.CompleteTurn();
                }
            }

            var gameBoard = new GameBoardMapper().Map(gameState);

            return gameBoard;
        }
    }
}
