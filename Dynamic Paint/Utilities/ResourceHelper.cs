using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dynamic_Paint.Utilities
{
    public class ResourceHelper
    {
        public ResourceHelper(string resourceName, Assembly assembly)
        {
            ResourceManager = new ResourceManager(resourceName, assembly);
        }

        private ResourceManager ResourceManager { get; set; }

        public string GetResourceName(string value, CultureInfo cultureToSearch)
        {
            DictionaryEntry entry = ResourceManager.GetResourceSet(cultureToSearch, true, true)
                .OfType<DictionaryEntry>().FirstOrDefault(dictionaryEntry => dictionaryEntry.Value.ToString() == value);
            try
            {
                return entry.Key.ToString();
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        public string GetResourceValue(string name, CultureInfo cultureToSearch)
        {
            string value = ResourceManager.GetString(name, cultureToSearch);
            return !string.IsNullOrEmpty(value) ? value : null;
        }
    }
}
