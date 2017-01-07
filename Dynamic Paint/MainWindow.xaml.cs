using Dynamic_Paint.Language;
using Dynamic_Paint.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace Dynamic_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CultureResources.ChangeCulture(Properties.Settings.Default.DefaultCulture);
            CultureInfo.DefaultThreadCurrentCulture = Properties.Settings.Default.DefaultCulture;

            InitializeComponent();
        }

        private void OpenCommandBinding_Executed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            CultureInfo lang = CultureInfo.CurrentUICulture;
            if (lang.IetfLanguageTag == "pl-PL")
                ofd.Filter = "Pliki obrazów (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            else
                ofd.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            if (ofd.ShowDialog() == true)
            {
                using (var stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                    var width = bitmapFrame.PixelWidth;
                    var height = bitmapFrame.PixelHeight;

                    var vm = this.DataContext as MainWindowViewModel;
                    vm.CanvasWidth = width;
                    vm.CanvasHeight = height;
                    ImageBrush loadedImage = new ImageBrush();
                    loadedImage.ImageSource = new BitmapImage(new Uri(ofd.FileName));
                    vm.CanvasBackground = loadedImage;
                    vm.ClearCanvasCommand.Execute(null);
                    vm.PathToLoadedFile = ofd.FileName;
                }
            }
        }
    }
}
