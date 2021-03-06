﻿using Dynamic_Paint.Language;
using Dynamic_Paint.ViewModels;
using Dynamic_Paint.Views;
using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dynamic_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Dla uproszczenia obsługi komendy z menu 'File' zostały obsłużone w widoku 'View'
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Domyślny język - angielski
            CultureResources.ChangeCulture(Properties.Settings.Default.DefaultCulture);
            CultureInfo.DefaultThreadCurrentCulture = Properties.Settings.Default.DefaultCulture;

            InitializeComponent();
        }

        private void NewCommandBinding_Executed(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            if (!vm.WorkSaved)
            {
                MessageBoxResult result = DiscardChangesView.ShowDialog();
                if (result == MessageBoxResult.No)
                    return;
            }
            Brush background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            vm.CanvasBackground = background;
            vm.PathToLoadedFile = "";
            vm.ClearCanvasCommand.Execute(null);
        }

        private void OpenCommandBinding_Executed(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            if (!vm.WorkSaved)
            {
                MessageBoxResult result = DiscardChangesView.ShowDialog();
                if (result == MessageBoxResult.No)
                    return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            CultureInfo lang = CultureInfo.CurrentUICulture;
            if (lang.IetfLanguageTag == "pl-PL")
                ofd.Filter = "Pliki obrazów (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            else
                ofd.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            if (ofd.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //wczytywanie obrazka na tło Canvas
                    var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                    var width = bitmapFrame.PixelWidth;
                    var height = bitmapFrame.PixelHeight;

                    vm.CanvasWidth = width;
                    vm.CanvasHeight = height;
                    ImageBrush loadedImage = new ImageBrush();

                    // prevents WPF bug in BitmapImage from file locking
                    var bitmap = new BitmapImage();
                    var tempstream = File.OpenRead(ofd.FileName);
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = tempstream;
                    bitmap.EndInit();
                    loadedImage.ImageSource = bitmap;
                    tempstream.Close();
                    tempstream.Dispose();

                    vm.CanvasBackground = loadedImage;
                    vm.ClearCanvasCommand.Execute(null);
                    vm.PathToLoadedFile = ofd.FileName;
                    vm.WorkSaved = true;
                }
            }
        }

        private void SaveCommandBinding_Executed(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            if (vm.PathToLoadedFile == null || vm.PathToLoadedFile == "")
                SaveAsCommandBinding_Executed(sender, e);
            else
            {
                Visual x = (Visual)this.FindName("SceneControl");
                vm.SaveCanvasToFileCommand.Execute(x);
            }
        }

        private void SaveAsCommandBinding_Executed(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".png";
            CultureInfo lang = CultureInfo.CurrentUICulture;
            if (lang.IetfLanguageTag == "pl-PL")
                sfd.Filter = "Pliki obrazów (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            else
                sfd.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
            if (sfd.ShowDialog() == true)
            {
                var vm = this.DataContext as MainWindowViewModel;
                vm.PathToLoadedFile = sfd.FileName;
                Visual x = (Visual)this.FindName("SceneControl");
                vm.SaveCanvasToFileCommand.Execute(x);
            }
        }
    }
}
