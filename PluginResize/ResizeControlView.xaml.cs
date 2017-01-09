using InternalShared;
using PluginResize.Language;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows.Controls;

namespace PluginResize
{
    /// <summary>
    /// Interaction logic for ResizeControl.xaml
    /// </summary>
    [Export(typeof(IPlugin))]
    public partial class ResizeControlView : UserControl, IPlugin
    {
        public ResizeControlView()
        {
            CultureResources.ChangeCulture(Properties.Settings.Default.DefaultCulture);
            CultureInfo.DefaultThreadCurrentCulture = Properties.Settings.Default.DefaultCulture;
            InitializeComponent();
        }

        public void AcceptHostInterface(IHost hostInterface)
        {
            var vm = this.DataContext as ResizeControlViewModel;
            vm.hostInterface = hostInterface;
        }

        public void ChangeLanguagePlugin(object chosenCulture)
        {
            var vm = this.DataContext as ResizeControlViewModel;
            vm.ChangeLanguage(chosenCulture);
        }
    }
}
