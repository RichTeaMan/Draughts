using Draughts.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for GameSquare.xaml
    /// </summary>
    public partial class GameSquare : UserControl
    {
        private GamePiece _piece;

        private GameSquareState _gameSquareState;

        private Brush _squareColour;

        public GameSquareState GameSquareState
        {
            get { return _gameSquareState; }
            set
            {
                _gameSquareState = value;
                RecalculateBackgroundColour();
            }
        }

        public Brush SelectedColourBrush { get; set; } = new SolidColorBrush(Color.FromRgb(50, 50, 255));

        public Brush PossibleMoveColourBrush { get; set; } = new SolidColorBrush(Color.FromRgb(255, 50, 50));

        public Brush SquareColour
        {
            get { return _squareColour; }
            set
            {
                _squareColour = value;
                RecalculateBackgroundColour();
            }
        }

        public int Xcoord { get; }

        public int Ycoord { get; }

        public GamePiece Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                string fileName;
                if (_piece.PieceColour == PieceColour.Black && _piece.PieceRank == PieceRank.Minion)
                {
                    fileName = "black";
                }
                else if (_piece.PieceColour == PieceColour.White && _piece.PieceRank == PieceRank.Minion)
                {
                    fileName = "white";
                }
                else if (_piece.PieceColour == PieceColour.Black && _piece.PieceRank == PieceRank.King)
                {
                    fileName = "kingblack";
                }
                else if (_piece.PieceColour == PieceColour.White && _piece.PieceRank == PieceRank.King)
                {
                    fileName = "kingwhite";
                }
                else
                {
                    throw new ApplicationException("Unknown piece.");
                }

                image.Source = new BitmapImage(new Uri($"pack://application:,,,/Draughts.UI.Wpf;component/Resources/{fileName}.png"));
            }
        }

        public GameSquare()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                throw new ApplicationException("This constructor cannot be used at runtime, only design mode.");
            }
        }

        public GameSquare(int x, int y)
        {
            InitializeComponent();

            Xcoord = x;
            Ycoord = y;
        }

        public void ClearPiece()
        {
            image.Source = null;
        }

        private void RecalculateBackgroundColour()
        {
            switch (_gameSquareState)
            {
                case GameSquareState.PlayerSelected:
                    Background = SelectedColourBrush;
                    break;
                case GameSquareState.PossibleMove:
                    Background = PossibleMoveColourBrush;
                    break;
                default:
                    Background = SquareColour;
                    break;
            }
        }
    }
}
