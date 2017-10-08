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

        public Brush DarkColourBrush { get; set; } = new SolidColorBrush(Color.FromRgb(140, 140, 140));

        public Brush LightColourBrush { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        public DraughtsBoard()
        {
            InitializeComponent();

            foreach (var width in Enumerable.Range(0, BoardWidth))
            {
                foreach (var height in Enumerable.Range(0, BoardHeight))
                {
                    var cell = new Grid();
                    if (width % 2 == 0 && height % 2 == 0)
                    {
                        cell.Background = LightColourBrush;
                    }
                    else if (width % 2 != 0 && height % 2 != 0)
                    {
                        cell.Background = LightColourBrush;
                    }
                    else
                    {
                        cell.Background = DarkColourBrush;
                    }
                    Grid.SetColumn(cell, width);
                    Grid.SetRow(cell, height);
                    Board.Children.Add(cell);
                }
            }

        }
    }
}
