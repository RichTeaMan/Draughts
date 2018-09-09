using Draughts.Service;
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

        private static Random random = new Random();

        [HttpPost]
        [HttpGet]
        [Route("api/hello")]
        public string Hello()
        {
            return "hello world";
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        [Route("api/game/create")]
        public IActionResult CreateGame()
        {
            var playerId = Guid.NewGuid().ToString();
            var humanPlayer = new HumanPlayer();

            IGamePlayer white;
            IGamePlayer black;

            if (random.Next() % 2 == 0)
            {
                white = humanPlayer;
                black = Program.AiOpponent;
                humanPlayer.PieceColour = Service.PieceColour.White;
            }
            else
            {
                white = Program.AiOpponent;
                black = humanPlayer;
                humanPlayer.PieceColour = Service.PieceColour.Black;
            }

            var match = new GameMatch(GameStateFactory.StandardStartGameState(), white, black);

            humanPlayer.GameMatch = match;

            humanPlayers.TryAdd(playerId, humanPlayer);

            return Ok(playerId);
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
                else if(player.GameMatch.CurrentTurn != player.PieceColour)
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
        [ProducesResponseType(200, Type = typeof(GameBoard))]
        [ProducesResponseType(409)]
        [Route("api/game")]
        public IActionResult PlayTurn([FromBody]MoveRequest moveRequest)
        {
            HumanPlayer player;
            if (!humanPlayers.TryGetValue(moveRequest.PlayerId, out player))
            {
                return NotFound($"Player with ID {moveRequest.PlayerId} not found.");
            }

            var match = player.GameMatch;

            GameState gameState = match.GameState;

            if (match.CurrentTurn == player.PieceColour)
            {

                var transformedMoveRequest = moveRequest;
                if (player.PieceColour == Service.PieceColour.Black)
                {
                    transformedMoveRequest = new BoardRotaterMapper().Rotate(moveRequest, gameState.XLength, gameState.YLength);
                }

                var moves = match.GameState.CalculateAvailableMoves(player.PieceColour);
                var moveToPlay = moves.FirstOrDefault(m =>
                    m.StartGamePiece.Xcoord == transformedMoveRequest.StartX &&
                    m.StartGamePiece.Ycoord == transformedMoveRequest.StartY &&
                    m.EndGamePiece.Xcoord == transformedMoveRequest.EndX &&
                    m.EndGamePiece.Ycoord == transformedMoveRequest.EndY);

                if (moveToPlay != null)
                {
                    player.SelectedMove = moveToPlay;
                    match.CompleteTurn();

                    gameState = match.GameState;
                }
                else
                {
                    return Conflict("The given move is not playable.");
                }
            }

            var gameBoard = new GameBoardMapper().Map(match, player.PieceColour);

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

            return Ok(gameBoard);
        }
    }
}
