using Draughts.Service;
using Draughts.UI.Wpf.Services;
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
using System.Windows.Threading;

namespace Draughts.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IGamePlayer WhitePlayer { get; set; }

        public IGamePlayer BlackPlayer { get; set; }

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private GameMatch gameMatch;

        private AiLoader aiLoader;

        public MainWindow()
        {
            InitializeComponent();

            aiLoader = ((App)App.Current).AiLoader;

            WhitePlayer = aiLoader.LoadedGamePlayers[0];
            BlackPlayer = new HumanPlayer() { DraughtsBoard = Board };

            gameMatch = new GameMatch(GameStateFactory.StandardStartGameState(), WhitePlayer, BlackPlayer);

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (gameMatch.GameMatchOutcome == GameMatchOutcome.InProgress)
            {
                await Task.Run(() =>
                {
                    gameMatch.CompleteTurn();
                });
                Board.ClearState();
                Board.SetupFromGameState(gameMatch.GameState);
            }
            else
            {
                MessageBox.Show($"Game over in {gameMatch.TurnCount} turns: {gameMatch.GameMatchOutcome}.");
                dispatcherTimer.Stop();
            }
        }
    }
}
