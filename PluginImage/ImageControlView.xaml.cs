using InternalShared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace PluginImage
{
    /// <summary>
    /// Interaction logic for ImageControlView.xaml
    /// </summary>     
    [Export(typeof(IPlugin))]
    [ExportMetadata("Name", "ImagePlugin")]
    public partial class ImageControlView : UserControl, IPlugin
    {
        public ImageControlView()
        {
            InitializeComponent();
        }

        public void AcceptHostInterface(IHost hostInterface)
        {
            var vm = this.DataContext as ImageControlViewModel;
            vm.hostInterface = hostInterface;
        }

        public void ChangeLanguagePlugin(object chosenCulture)
        {
            throw new NotImplementedException();
        }

        public FrameworkElement GetPluginView()
        {
            return this as FrameworkElement;
        }
    }
}
