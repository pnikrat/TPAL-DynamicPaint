using Dynamic_Paint.Language;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Dynamic_Paint.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            _english = true;
            _polish = false;
        }

        private bool _english;
        private bool _polish;
        private string _statusBarText;
        private RelayCommand _drawLineCommand;
        private RelayCommand _changeLanguageCommand;
       

        public bool English
        {
            get { return _english; }
            set
            {
                _english = value;
                base.OnPropertyChanged("English");
            }
        }

        public bool Polish
        {
            get { return _polish; }
            set
            {
                _polish = value;
                base.OnPropertyChanged("Polish");
            }
        }

        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
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
                    _drawLineCommand = new RelayCommand(param => this.DrawLine());
                }
                return _drawLineCommand;
            }
        }

        public void DrawLine()
        {
            StatusBarText = Properties.Resources.StatusDrawingLine;
        }

        public ICommand ChangeLanguageCommand
        {
            get
            {
                if (_changeLanguageCommand == null)
                    _changeLanguageCommand = new RelayCommand(param => ChangeLanguage(param));
                return _changeLanguageCommand;
            }
        }

        public void ChangeLanguage(object chosenCulture)
        {
            string ChosenCultureString = (string)chosenCulture;
            if (ChosenCultureString == "pl")
            {
                Polish = true;
                English = false;
            }
            else
            {
                English = true;
                Polish = false;
            }
            CultureInfo ChosenCultureInfo = CultureInfo.CreateSpecificCulture(ChosenCultureString);
            CultureResources.ChangeCulture(ChosenCultureInfo);
            CultureInfo.DefaultThreadCurrentCulture = ChosenCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = ChosenCultureInfo;

            base.OnPropertyChanged("StatusBarText");
        }
    }
}
