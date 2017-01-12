using System.Windows;

namespace InternalShared
{
    /// <summary>
    /// Interfejs przez który aplikacja komunikuje się z pluginami
    /// </summary>
    public interface IPlugin
    {
        FrameworkElement GetPluginView();
        void AcceptHostInterface(IHost hostInterface);
        void ChangeLanguagePlugin(object chosenCulture);
    }
}
