using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InternalShared
{
    public interface IPlugin
    {
        FrameworkElement GetPluginView();
        void AcceptHostInterface(IHost hostInterface);
        void ChangeLanguagePlugin(object chosenCulture);
    }
}
