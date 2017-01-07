using Dynamic_Paint.Language;
using Dynamic_Paint.Models;
using Dynamic_Paint.Properties;
using Dynamic_Paint.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

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
            Coordinates = "X: 0, Y: 0";
            _isDrawing = false;

            for (int i = 2; i <= 9; i++)
                _strokeThicknessOptions.Add(i.ToString());
            _selectedStrokeThickness = "5";
            _selectedStrokeThicknessNumeric = 5;
        }

        private bool _english;
        private bool _polish;
        private string _currentCulture;

        private string _statusBarText;
        private string _coordinates;

        private ObservableCollection<string> _strokeThicknessOptions = new ObservableCollection<string>();
        private string _selectedStrokeThickness;
        private int _selectedStrokeThicknessNumeric;

        private bool _drawingLine;
        private bool _drawingRectangle;
        private bool _drawingEllipse;

        private ObservableCollection<CanvasShapeViewModel> _sceneObjects = new ObservableCollection<CanvasShapeViewModel>();
        public CanvasShapeViewModel _currentlyDrawnShapeRef;
        private bool _isDrawing;
        private bool _isDrawingToolChosen;

        private RelayCommand _drawLineCommand;
        private RelayCommand _drawRectangleCommand;
        private RelayCommand _drawEllipseCommand;
        private RelayCommand _undoCommand;

        private RelayCommand _mouseDownCommand;
        private RelayCommand _mouseUpCommand;
        private RelayCommand _mouseMoveCommand;

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

        public string Coordinates
        {
            get { return _coordinates; }
            set
            {
                _coordinates = value;
                base.OnPropertyChanged("Coordinates");
            }
        }

        public ObservableCollection<string> StrokeThicknessOptions
        {
            get { return _strokeThicknessOptions; }
            set
            {
                _strokeThicknessOptions = value;
                base.OnPropertyChanged("StrokeThicknessOptions");
            }
        }

        public string SelectedStrokeThickness
        {
            get { return _selectedStrokeThickness; }
            set
            {
                _selectedStrokeThickness = value;
                _selectedStrokeThicknessNumeric = Int32.Parse(_selectedStrokeThickness); 
                base.OnPropertyChanged("SelectedStrokeThickness");
            }
        }

        public bool DrawingLine
        {
            get { return _drawingLine; }
            set
            {
                _drawingLine = value;
                base.OnPropertyChanged("DrawingLine");
            }
        }

        public bool DrawingRectangle
        {
            get { return _drawingRectangle; }
            set
            {
                _drawingRectangle = value;
                base.OnPropertyChanged("DrawingRectangle");
            }
        }

        public bool DrawingEllipse
        {
            get { return _drawingEllipse; }
            set
            {
                _drawingEllipse = value;
                base.OnPropertyChanged("DrawingEllipse");
            }
        }

        public ObservableCollection<CanvasShapeViewModel> SceneObjects
        {
            get { return _sceneObjects; }
            set
            {
                _sceneObjects = value;
                base.OnPropertyChanged("SceneObjects");
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
            DrawingLine = _isDrawingToolChosen = true;
            DrawingRectangle = false;
            DrawingEllipse = false;
        }

        public ICommand DrawRectangleCommand
        {
            get
            {
                if (_drawRectangleCommand == null)
                {
                    _drawRectangleCommand = new RelayCommand(param => this.DrawRectangle());
                }
                return _drawRectangleCommand;
            }
        }

        public void DrawRectangle()
        {
            StatusBarText = Properties.Resources.StatusDrawingRectangle;
            DrawingLine = false;
            DrawingRectangle = _isDrawingToolChosen = true;
            DrawingEllipse = false;
        }

        public ICommand DrawEllipseCommand
        {
            get
            {
                if (_drawEllipseCommand == null)
                {
                    _drawEllipseCommand = new RelayCommand(param => this.DrawEllipse());
                }
                return _drawEllipseCommand;
            }
        }

        public void DrawEllipse()
        {
            StatusBarText = Properties.Resources.StatusDrawingEllipse;
            DrawingLine = false;
            DrawingRectangle = false;
            DrawingEllipse = _isDrawingToolChosen = true;
        }

        public ICommand UndoCommand
        {
            get
            {
                if (_undoCommand == null)
                {
                    _undoCommand = new RelayCommand(param => this.Undo(), param => this.CanUndo());
                }
                return _undoCommand;
            }
        }

        public void Undo()
        {
            SceneObjects.RemoveAt(SceneObjects.Count - 1);
        }

        public bool CanUndo()
        {
            if (SceneObjects.Count == 0)
                return false;
            else
                return true;
        }

        public ICommand MouseDownCommand
        {
            get
            {
                if (_mouseDownCommand == null)
                {
                    _mouseDownCommand = new RelayCommand(param => this.MouseDown(param));
                }
                return _mouseDownCommand;
            }
        }

        public void MouseDown(object parent)
        {
            IInputElement canvas = (IInputElement)parent;
            if (canvas.IsMouseOver && _isDrawingToolChosen)
            {
                _isDrawing = true;
                Point mousePos = Mouse.GetPosition(canvas);
                _currentlyDrawnShapeRef = null;
                CanvasShapeViewModel shape = new CanvasShapeViewModel();
                if (_drawingLine)
                    shape = new MyLine(mousePos.X, mousePos.Y, _selectedStrokeThicknessNumeric);
                else if (_drawingRectangle)
                    shape = new MyRectangle(mousePos.X, mousePos.Y, _selectedStrokeThicknessNumeric);
                else
                    shape = new MyEllipse(mousePos.X, mousePos.Y, _selectedStrokeThicknessNumeric);
                _currentlyDrawnShapeRef = shape;
                SceneObjects.Add(shape);
            }
        }

        public ICommand MouseUpCommand
        {
            get
            {
                if (_mouseUpCommand == null)
                {
                    _mouseUpCommand = new RelayCommand(param => this.MouseUp(param));
                }
                return _mouseUpCommand;
            }
        }

        public void MouseUp(object parent)
        {
            _isDrawing = false;
            IInputElement canvas = (IInputElement)parent;
            if (canvas.IsMouseOver && _isDrawing)
            {                
                Point mousePos = Mouse.GetPosition(canvas);
                _currentlyDrawnShapeRef.UpdatePosition(mousePos.X, mousePos.Y);
            }
        }

        public ICommand MouseMoveCommand
        {
            get
            {
                if (_mouseMoveCommand == null)
                {
                    _mouseMoveCommand = new RelayCommand(param => this.MouseMove(param));
                }
                return _mouseMoveCommand;
            }
        }

        public void MouseMove(object parent)
        {
            Point mousePos = Mouse.GetPosition((IInputElement)parent);
            if (_currentlyDrawnShapeRef != null && _isDrawing && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _currentlyDrawnShapeRef.UpdatePosition(mousePos.X, mousePos.Y);
            }

            Coordinates = "X: " + ((int)mousePos.X).ToString() + ", Y: " + ((int)mousePos.Y).ToString();
        }

        public ICommand ChangeLanguageCommand
        {
            get
            {
                if (_changeLanguageCommand == null)
                    _changeLanguageCommand = new RelayCommand(param => this.ChangeLanguage(param));
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
