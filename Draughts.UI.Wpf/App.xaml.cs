using Draughts.UI.Wpf.Services;
using Draughts.UI.Wpf.Setup;
using RichTea.CommandLineParser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Draughts.UI.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AiLoader AiLoader { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Console.WriteLine("Draughts.UI.Wpf loaded.");

            var commandLineInvoker = new CommandLineParserInvoker().AddParameterParser(new GameParameterParser());
            commandLineInvoker.GetCommand(typeof(SetupService), e.Args).Invoke();

            StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }
    }
}
