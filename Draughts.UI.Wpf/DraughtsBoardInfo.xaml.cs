﻿using Draughts.Service;
using Draughts.UI.Wpf.Extensions;
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
    /// Interaction logic for DraughtsBoardInfo.xaml
    /// </summary>
    public partial class DraughtsBoardInfo : UserControl
    {
        public DraughtsBoardInfo()
        {
            InitializeComponent();
        }

        public void UpdateFromGameMatch(GameMatch gameMatch)
        {
            DataContext = gameMatch;

            TurnCountLbl.Text = gameMatch.TurnCount.ToString();

            if (PieceColour.White == gameMatch.CurrentTurn)
            {
                PlayerTurnLbl.Text = "White player's turn";
            }
            else if (PieceColour.Black == gameMatch.CurrentTurn)
            {
                PlayerTurnLbl.Text = "Black player's turn";
            }

            WhitePlayerType.Text = gameMatch.WhiteGamePlayer.PlayerSummary();
            BlackPlayerType.Text = gameMatch.BlackGamePlayer.PlayerSummary();
        }
    }
}
