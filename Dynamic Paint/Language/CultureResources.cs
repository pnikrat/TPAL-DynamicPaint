using Dynamic_Paint.Properties;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dynamic_Paint.Language
{
    public class CultureResources
    {
        private static readonly ObjectDataProvider ResourceProvider = Application.Current.Resources["Resources"] as ObjectDataProvider;
        

        public static void ChangeCulture(CultureInfo culture)
        {
            Resources.Culture = culture;
            ResourceProvider.Refresh();
        }

        public Resources GetResourceInstance() => new Resources();
    }
}
