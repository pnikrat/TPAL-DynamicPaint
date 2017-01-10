using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PluginResize
{
    [Export(typeof(ResourceDictionary))]
    public partial class ResourceDictionaryResize : ResourceDictionary
    {
        private static readonly ResourceDictionaryResize _instance = new ResourceDictionaryResize();

        public static ResourceDictionaryResize Instance
        {
            get { return _instance; }
        }

        public ResourceDictionaryResize()
        {
            InitializeComponent();
        }
    }
}
