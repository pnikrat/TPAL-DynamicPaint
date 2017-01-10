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
using System.Windows.Shapes;

namespace Dynamic_Paint.Views
{
    /// <summary>
    /// Interaction logic for DiscardChangesView.xaml
    /// </summary>
    public partial class DiscardChangesView : Window
    {
        public DiscardChangesView()
        {
            InitializeComponent();
        }

        public static new MessageBoxResult ShowDialog()
        {
            var instance = new DiscardChangesView();
            var result = ((Window)instance).ShowDialog();

            if (result == null)
                return MessageBoxResult.No;

            return result.Value ? MessageBoxResult.Yes : MessageBoxResult.No;
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
