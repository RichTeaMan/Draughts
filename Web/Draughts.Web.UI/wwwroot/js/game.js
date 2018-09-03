var currentGameBoard;
var selectedPiece = false;

function updateGame() {
    $.get("/api/game").done(function (data) {
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

$(document).ready(function () {
    updateGame();

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
    });
});
