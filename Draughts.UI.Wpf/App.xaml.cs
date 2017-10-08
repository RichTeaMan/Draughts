using Draughts.UI.Wpf.Services;
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
            Console.WriteLine($"'{e.Args.Count()}'program arguments:");
            foreach(var arg in e.Args)
            {
                Console.WriteLine(arg);
            }

            AiLoader = new AiLoader();

            foreach(var arg in e.Args)
            {
                if(File.Exists(arg))
                {
                    AiLoader.LoadFromJsonfile(arg);
                }
            }
        }
    }
}
