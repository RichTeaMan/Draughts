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
        [HttpPost]
        [HttpGet]
        [Route("api/hello")]
        public string Hello()
        {
            return "hello world";
        }

        [HttpGet]
        [Route("api/game")]
        public GameBoard Game()
        {
            var gameState = GameStateFactory.StandardStartGameState();

            var gameBoard = new GameBoardMapper().Map(gameState);

            return gameBoard;
        }
    }
}