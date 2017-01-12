using InternalShared;
using PluginImage.Language;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

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
            CultureResources.ChangeCulture(Properties.Settings.Default.DefaultCulture);
            CultureInfo.DefaultThreadCurrentCulture = Properties.Settings.Default.DefaultCulture;
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
