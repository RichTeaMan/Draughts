var currentGameBoard;
var selectedPiece = false;
var playerId = false;

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

            if (selectedPiece == newSelectedPiece) {
                selectedPiece = false;
            }
            else {
                selectedPiece = newSelectedPiece
                gameSquare.addClass("selected");
            }
        }
        else if (selectedPiece && !newSelectedPiece) {
            // an empty square has been selected, attempy to send move

            sendMove(selectedPiece, x, y);
        }
    });

    setInterval(reloadGame, 2000);
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

        console.log(data);
        renderBoard("game-grid", data);
        currentGameBoard = data;

        $("td").removeClass("selected");

    }).fail(function (error) {
        console.log(error);
    });
}

function reloadGame() {
    $.get(`/api/game?playerId=${playerId}`).done(function (data) {
        console.log(data);
        renderBoard("game-grid", data);
        currentGameBoard = data;
    });
}

function renderBoard(destinationElementId, gameBoard) {
    var html = '<table class="game-grid">';

    for (var y = 0; y < gameBoard.height; y++) {
        html += "<tr>";
        for (var x = 0; x < gameBoard.width; x++) {

            var transformedY = gameBoard.height - (y + 1);
            html += `<td id="square-${x}-${transformedY}" data-x="${x}" data-y="${transformedY}" class="piece-square"><div></div></td>`;
        }
        html += "</tr>";
    }
    html += "</table>";

    $(`#${destinationElementId}`).html(html);

    for (var i = 0; i < gameBoard.gamePieces.length; i++) {

        var piece = gameBoard.gamePieces[i];
        var id = `#square-${piece.xcoord}-${piece.ycoord}`;

        $(id).addClass(piece.pieceColour);
    }
}
