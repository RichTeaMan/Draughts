using Draughts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Draughts.UI.Wpf
{
    /// <summary>
    /// Interaction logic for DraughtsBoard.xaml
    /// </summary>
    public partial class DraughtsBoard : UserControl
    {
        public readonly int BoardWidth = 8;

        public readonly int BoardHeight = 8;

        public GameState CurrentGameState { get; private set; }

        public GamePiece SelectedGamePiece { get; private set; }

        public Brush DarkColourBrush { get; set; } = new SolidColorBrush(Color.FromRgb(140, 140, 140));

        public Brush LightColourBrush { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        private List<GameSquare> squareList = new List<GameSquare>();

        public DraughtsBoard()
        {
            InitializeComponent();

            foreach (var width in Enumerable.Range(0, BoardWidth))
            {
                foreach (var height in Enumerable.Range(0, BoardHeight))
                {
                    var cell = new GameSquare();
                    if (width % 2 == 0 && height % 2 == 0)
                    {
                        cell.SquareColour = LightColourBrush;
                    }
                    else if (width % 2 != 0 && height % 2 != 0)
                    {
                        cell.SquareColour = LightColourBrush;
                    }
                    else
                    {
                        cell.SquareColour = DarkColourBrush;
                    }
                    Grid.SetColumn(cell, width);
                    Grid.SetRow(cell, height);
                    Board.Children.Add(cell);
                    squareList.Add(cell);
                }
            }
            SetupFromGameState(GameStateFactory.StandardStartGameState());
        }

        public void SetupFromGameState(GameState gameState)
        {
            CurrentGameState = gameState;
            foreach (var piece in gameState.GamePieceList)
            {
                var gameSquare = FindSquare(piece.Xcoord, piece.Ycoord);

                gameSquare.Piece = piece;

                var selectedPiece = piece;
                var selectedPanel = gameSquare;
                gameSquare.MouseLeftButtonDown += (sender, e) => {
                    PieceLeftButtonDown(selectedPanel, selectedPiece, e);
                };
            }
        }

        public void ClearState()
        {
            foreach (var square in squareList)
            {
                square.ClearPiece();
            }
        }

        public GameSquare FindSquare(int x, int y)
        {
            int convertedY = (BoardHeight - 1) - y;

            var square = Board.Children.Cast<GameSquare>().Single(e => Grid.GetColumn(e) == x && Grid.GetRow(e) == convertedY);
            return square;
        }

        private void PieceLeftButtonDown(GameSquare gameSquare, GamePiece gamePiece, MouseButtonEventArgs e)
        {
            foreach(var square in squareList)
            {
                square.GameSquareState = GameSquareState.Standard;
            }

            gameSquare.GameSquareState = GameSquareState.PlayerSelected;
            SelectedGamePiece = gamePiece;
            var possibleMoves = CurrentGameState.CalculateAvailableMoves().Where(m => m.StartGamePiece == gamePiece);
            foreach(var possibleMove in possibleMoves)
            {
                var possibleSquare = FindSquare(possibleMove.EndGamePiece.Xcoord, possibleMove.EndGamePiece.Ycoord);
                possibleSquare.GameSquareState = GameSquareState.PossibleMove;
            }
        }
    }
}
