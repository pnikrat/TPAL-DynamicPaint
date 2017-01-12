using PluginImage.Properties;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PluginImage.Language
{
    public class CultureResources
    {
        //somehow find assembly resources!!
        private static readonly ObjectDataProvider ResourceProvider = Application.Current.Resources["Resources"] as ObjectDataProvider;
        

        public static void ChangeCulture(CultureInfo culture)
        {
            Resources.Culture = culture;
            ResourceProvider.Refresh();
        }

        public Resources GetResourceInstance() => new Resources();
    }
}
