
function updateGame() {
    $.get("/api/game").done(function (data) {
        console.log(data);
        renderBoard("game-grid", data);
    });
}

function renderBoard(destinationElementId, gameBoard) {
    var html = '<table class="game-grid">';

    for (var y = 0; y < gameBoard.height; y++) {
        html += "<tr>";
        for (var x = 0; x < gameBoard.width; x++) {

            var transformedY = gameBoard.height - (y + 1);
            html += `<td id="square-${x}-${transformedY}" class="piece-square"></td>`;
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

$(document).ready(function () { updateGame(); });
