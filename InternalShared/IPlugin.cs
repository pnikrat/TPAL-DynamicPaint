using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalShared
{
    public interface IPlugin
    {
        void AcceptHostInterface(IHost hostInterface);
        void ChangeLanguagePlugin(object chosenCulture);
    }
}
