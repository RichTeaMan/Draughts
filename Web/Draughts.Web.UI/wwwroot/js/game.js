var currentGameBoard;
var selectedPiece = false;
var playerId = false;
var gameStopped = false;
var gameUpateInterval = false;

function createGame() {
    $.get("/api/game/create").done(function (data) {
        console.log(data);
        window.location.href = `/Home/game?playerId=${data}`;
    });
}

function setupGame(_playerId) {
    playerId = _playerId;
    reloadGame();

    $(document).on('click', ".piece-square", function (event) {

        if (gameStopped) {
            return;
        }
        var gameSquare = $(event.currentTarget);
        var x = gameSquare.data("x");
        var y = gameSquare.data("y");

        console.log(`${x}, ${y} clicked.`);

        var newSelectedPiece = false;
        currentGameBoard.gamePieces.forEach(function (piece) {
            if (x == piece.xcoord && y == piece.ycoord) {
                newSelectedPiece = piece;
            }
        });

        if (newSelectedPiece) {
            // unselect other pieces
            $("td").removeClass("selected");
            $("td").removeClass("destination");

            if (selectedPiece == newSelectedPiece) {
                selectedPiece = false;
            }
            else {
                selectedPiece = newSelectedPiece
                gameSquare.addClass("selected");

                // look for move end location
                currentGameBoard.gameMoves.forEach(function (move) {
                    if (move.startX == newSelectedPiece.xcoord && move.startY == newSelectedPiece.ycoord) {

                        var endSquare = $(`#square-${move.endX}-${move.endY}`);
                        if (endSquare) {
                            endSquare.addClass('destination');
                        }
                    }
                });
            }
        }
        else if (selectedPiece && !newSelectedPiece) {
            // an empty square has been selected, attempt to send move
            // look for move end location
            currentGameBoard.gameMoves.some(function (move) {
                if (move.endX == x && move.endY == y) {
                    sendMove(selectedPiece, x, y);
                    return true;
                }
            });
        }
    });

    gameUpateInterval = setInterval(reloadGame, 2000);
}

function sendMove(startPiece, endX, endY) {
    var moveRequest = {
        playerId: playerId,

        startX: startPiece.xcoord,
        startY: startPiece.ycoord,

        endX: endX,
        endY: endY
    };

    console.log("Sending move...");
    console.log(moveRequest);


    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: '/api/game',
        data: JSON.stringify(moveRequest),
        dataType: "json"
    }).done(function (data) {

        console.log("Sending move successful");
        console.log(data);
        renderBoard("game-grid", data);

        $("td").removeClass("selected");
        $("td").removeClass("destination");

    }).fail(function (error) {

        console.log("Sending move failed");
        console.log(error);
    });
}

function reloadGame() {
    $.get(`/api/game?playerId=${playerId}`).done(function (data) {
        console.log(data);

        renderBoard("game-grid", data);
        if (data.gameStatus != "inProgress") {
            if (gameUpateInterval) {
                clearInterval(gameUpateInterval);
            }
            $("#status").html(`${data.gameStatus}`);
            alert(`Game Over! ${data.gameStatus}`);
        }
    });
}

function renderBoard(destinationElementId, gameBoard) {

    if (JSON.stringify(currentGameBoard) != JSON.stringify(gameBoard)) {

        $("#vs").html(`${gameBoard.playerName} VS ${gameBoard.opponentName}`);
        $("#status").html(`It is ${gameBoard.currentTurnColour}'s turn.`);

        var html = '<table class="game-grid">';

        var squareCount = 0;
        for (var y = 0; y < gameBoard.height; y++) {
            html += "<tr>";
            for (var x = 0; x < gameBoard.width; x++) {

                var transformedY = gameBoard.height - (y + 1);
                var squareClass = squareCount % 2 == 0 ? "light" : "dark";
                html += `<td id="square-${x}-${transformedY}" data-x="${x}" data-y="${transformedY}" class="piece-square ${squareClass}"><div></div></td>`;
                squareCount++;
            }
            squareCount++;
            html += "</tr>";
        }
        html += "</table>";

        $(`#${destinationElementId}`).html(html);

        for (var i = 0; i < gameBoard.gamePieces.length; i++) {

            var piece = gameBoard.gamePieces[i];
            var id = `#square-${piece.xcoord}-${piece.ycoord}`;

            $(id).addClass(piece.pieceColour);
            $(id).addClass(piece.pieceRank);
        }

        currentGameBoard = gameBoard;
    }
}
