using Dynamic_Paint.Language;
using Dynamic_Paint.Properties;
using Dynamic_Paint.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            _currentCulture = "en";
            StatusBarText = Properties.Resources.StatusDefaultText;
        }

        private bool _english;
        private bool _polish;
        private string _currentCulture;
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
            ResourceHelper helper = new ResourceHelper("Dynamic_Paint.Properties.Resources", GetType().Assembly);
            string updatedStatusBarTextKey = helper.GetResourceName(StatusBarText, CultureInfo.CreateSpecificCulture(_currentCulture));

            string ChosenCultureString = (string)chosenCulture;
            CultureInfo ChosenCultureInfo = CultureInfo.CreateSpecificCulture(ChosenCultureString);

            if (ChosenCultureString == "pl")
            {
                Polish = true;
                English = false;
                _currentCulture = "pl";
            }
            else
            {
                English = true;
                Polish = false;
                _currentCulture = "en";
            }

            CultureResources.ChangeCulture(ChosenCultureInfo);
            CultureInfo.DefaultThreadCurrentCulture = ChosenCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = ChosenCultureInfo;
        
            StatusBarText = helper.GetResourceValue(updatedStatusBarTextKey, CultureInfo.CreateSpecificCulture(_currentCulture));
            base.OnPropertyChanged("StatusBarText");
        }
    }
}
