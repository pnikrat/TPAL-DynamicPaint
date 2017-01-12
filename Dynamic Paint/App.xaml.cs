using Dynamic_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Dynamic_Paint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //inicjalizacja widoku (View)
            MainWindow window = new MainWindow();

            var viewModel = new MainWindowViewModel();
            //powiązanie widoku z jego ViewModel'em
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
