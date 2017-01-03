using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dynamic_Paint.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            StatusBarText = "Test text";
            _canDraw = true;
        }

        private bool _canDraw;
        private string _statusBarText;
        private RelayCommand _drawLineCommand;

        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                if (value == _statusBarText)
                    return;

                _statusBarText = value;
                base.OnPropertyChanged("StatusBarText");
            }
        }

        public ICommand DrawLineCommand
        {
            get
            {
                if (_drawLineCommand == null)
                {
                    _drawLineCommand = new RelayCommand(param => this.DrawLine(), param => this._canDraw);
                }
                return _drawLineCommand;
            }
        }

        public void DrawLine()
        {
            StatusBarText = "Drawing a line";
        }
    }
}
