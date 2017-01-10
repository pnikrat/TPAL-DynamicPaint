using InternalShared;
using PluginResize.Language;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace PluginResize
{
    /// <summary>
    /// Interaction logic for ResizeControl.xaml
    /// </summary>
    [Export(typeof(IPlugin))]
    [ExportMetadata("Name", "ResizePlugin")]
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

        public FrameworkElement GetPluginView()
        {
            return this as FrameworkElement;
        }
    }
}
