using Dynamic_Paint.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
