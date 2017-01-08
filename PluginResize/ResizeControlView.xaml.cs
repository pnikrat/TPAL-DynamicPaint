﻿using InternalShared;
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

namespace PluginResize
{
    /// <summary>
    /// Interaction logic for FillControl.xaml
    /// </summary>
    [Export(typeof(IPlugin))]
    public partial class ResizeControlView : UserControl, IPlugin
    {
        public ResizeControlView()
        {
            InitializeComponent();
        }

        public void AcceptHostInterface(IHost hostInterface)
        {
            var vm = this.DataContext as ResizeControlViewModel;
            vm.hostInterface = hostInterface;
        }
    }
}