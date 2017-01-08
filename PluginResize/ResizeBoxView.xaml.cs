using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PluginResize
{
    /// <summary>
    /// Interaction logic for ResizeBoxView.xaml
    /// </summary>
    public partial class ResizeBoxView : Window
    {
        private static int _width;
        private static int _height;

        public ResizeBoxView()
        {
            InitializeComponent();
        }

        public static MessageBoxResult ShowDialog(out int[] canvasSize)
        {
            var instance = new ResizeBoxView();
            var result = ((Window)instance).ShowDialog();

            canvasSize = instance.GetCanvasSize();

            if (result == null)
                return MessageBoxResult.Cancel;

            return result.Value ? MessageBoxResult.OK : MessageBoxResult.Cancel;
        }

        public int[] GetCanvasSize()
        {
            bool parseWidthResult;
            bool parseHeightResult;
            parseWidthResult = Int32.TryParse(WidthBox.Text, out _width);
            parseHeightResult = Int32.TryParse(HeightBox.Text, out _height);
            if (parseHeightResult && parseWidthResult)
            {
                int[] size = { _width, _height };
                return size;
            }
            else
            {
                return null;
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); 
        }
    }
}
