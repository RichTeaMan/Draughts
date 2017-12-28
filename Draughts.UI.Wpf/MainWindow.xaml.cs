using Draughts.Service;
using Draughts.UI.Wpf.Services;
using Draughts.UI.Wpf.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Draughts.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private GameMatch gameMatch;

        private AiLoader aiLoader;

        public MainWindow()
        {
            InitializeComponent();

            aiLoader = ((App)App.Current).AiLoader;

            Board.WhitePlayer = SetupService.FetchWhitePlayer();
            Board.BlackPlayer = SetupService.FetchBlackPlayer();

            gameMatch = new GameMatch(GameStateFactory.StandardStartGameState(), Board.WhitePlayer, Board.BlackPlayer);

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            if (gameMatch.GameMatchOutcome == GameMatchOutcome.InProgress)
            {
                await Task.Run(() =>
                {
                    gameMatch.CompleteTurn();
                });
                Board.ClearState();
                Board.SetupFromGameState(gameMatch.GameState);
                dispatcherTimer.Start();
            }
            else
            {
                MessageBox.Show($"Game over in {gameMatch.TurnCount} turns: {gameMatch.GameMatchOutcome}.");
            }
        }
    }
}
