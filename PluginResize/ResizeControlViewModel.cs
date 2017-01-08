using InternalShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            MessageBoxResult result;
            int[] size;
            result = ResizeBoxView.ShowDialog(out size);
            if (size != null)
            {
                hostInterface.SetCanvasWidth(size[0]);
                hostInterface.SetCanvasHeight(size[1]);
            }
        }
    }
}
