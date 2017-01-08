using InternalShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PluginResize
{
    public class ResizeControlViewModel : ViewModelBase
    {
        public IHost hostInterface { get; set; }
        RelayCommand _resizeCommand;

        public ICommand ResizeCommand
        {
            get
            {
                if (_resizeCommand == null)
                    _resizeCommand = new RelayCommand(param => this.Resize());
                return _resizeCommand;
            }
        } 

        public void Resize()
        {
            hostInterface.SetCanvasHeight(1050);
            hostInterface.SetCanvasWidth(1680);
        }
    }
}
