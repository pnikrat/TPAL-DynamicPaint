using Dynamic_Paint.Language;
using Dynamic_Paint.Models;
using Dynamic_Paint.Properties;
using Dynamic_Paint.Utilities;
using InternalShared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dynamic_Paint.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IHost
    {
        public MainWindowViewModel()
        {
            _english = true;
            _polish = false;
            _currentCulture = "en";
            StatusBarText = Properties.Resources.StatusDefaultText;
            Coordinates = "X: 0, Y: 0";
            _isDrawing = false;
            _canvasWidth = 1024;
            _canvasHeight = 768;
            _canvasBackground = new SolidColorBrush(Color.FromRgb(255,255,255));

            for (int i = 2; i <= 9; i++)
                _strokeThicknessOptions.Add(i.ToString());
            _selectedStrokeThickness = "5";
            _selectedStrokeThicknessNumeric = 5;
            _selectedColor = Color.FromRgb(0, 0, 0);
            _selectedColorBrush = new SolidColorBrush(_selectedColor);
            WindowTitle = Properties.Resources.UntitledText;

            helper = new ResourceHelper("Dynamic_Paint.Properties.Resources", GetType().Assembly);
            _workSaved = true;
            WorkSaved = false;

            string executingPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginsPath = System.IO.Path.Combine(executingPath, "plugins");
            if (!Directory.Exists(pluginsPath))
                Directory.CreateDirectory(pluginsPath);
            _pluginsCatalog = new DirectoryCatalog(pluginsPath, "Plugin*.dll");
            _pluginsContainer = new CompositionContainer(_pluginsCatalog);
            _pluginsAreLoaded = false;
        }

        private IPlugin _pluginView;
        private DirectoryCatalog _pluginsCatalog;
        private CompositionContainer _pluginsContainer;
        private bool _pluginsAreLoaded;
        [ImportMany(typeof(ResourceDictionary))]
        public IEnumerable<ResourceDictionary> _resourceDictionaries { get; set; }

        private ResourceHelper helper;
        private bool _workSaved;

        private bool _english;
        private bool _polish;
        private string _currentCulture;

        private string _appTitle = "Dynamic Paint";
        private string _windowTitle;

        private string _statusBarText;
        private string _coordinates;

        private int _canvasWidth;
        private int _canvasHeight;
        private Brush _canvasBackground;

        private string _pathToLoadedFile;

        private ObservableCollection<string> _strokeThicknessOptions = new ObservableCollection<string>();
        private string _selectedStrokeThickness;
        private int _selectedStrokeThicknessNumeric;

        private Color _selectedColor;
        private Brush _selectedColorBrush;

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

        private RelayCommand _clearCanvasCommand;
        private RelayCommand _saveCanvasToFileCommand;

        private RelayCommand _changeLanguageCommand;

        private RelayCommand _importPluginsCommand;

        private RelayCommand _exitAppCommand;

        [Import(typeof(IPlugin))]
        public IPlugin PluginView
        {
            get { return _pluginView; }
            set
            {
                _pluginView = value;
                base.OnPropertyChanged("PluginView");
            }
        }

        public bool WorkSaved
        {
            get { return _workSaved; }
            set
            {
                string stripString = " - " + _appTitle;
                if (value != _workSaved && (value))
                    WindowTitle = _pathToLoadedFile;             
                else if (value != _workSaved)
                    WindowTitle = WindowTitle.Remove(WindowTitle.IndexOf(stripString), stripString.Length) + "*";
                _workSaved = value;
            }
        }

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

        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value + " - " + _appTitle;
                base.OnPropertyChanged("WindowTitle");
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

        public int CanvasWidth
        {
            get { return _canvasWidth; }
            set
            {
                _canvasWidth = value;
                base.OnPropertyChanged("CanvasWidth");
            }
        }

        public int CanvasHeight
        {
            get { return _canvasHeight; }
            set
            {
                _canvasHeight = value;
                base.OnPropertyChanged("CanvasHeight");
            }
        }

        public Brush CanvasBackground
        {
            get { return _canvasBackground; }
            set
            {
                _canvasBackground = value;
                base.OnPropertyChanged("CanvasBackground");
            }
        }

        public string PathToLoadedFile
        {
            get { return _pathToLoadedFile; }
            set
            {
                _pathToLoadedFile = value;
                WindowTitle = value;
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

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                _selectedColorBrush = new SolidColorBrush(_selectedColor);
                base.OnPropertyChanged("SelectedColor");
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
            WorkSaved = false;
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
                WorkSaved = false;
                _isDrawing = true;
                Point mousePos = Mouse.GetPosition(canvas);
                _currentlyDrawnShapeRef = null;
                CanvasShapeViewModel shape = new CanvasShapeViewModel();
                if (_drawingLine)
                    shape = new MyLine(mousePos.X, mousePos.Y, _selectedStrokeThicknessNumeric, _selectedColorBrush);
                else if (_drawingRectangle)
                    shape = new MyRectangle(mousePos.X, mousePos.Y, _selectedStrokeThicknessNumeric, _selectedColorBrush);
                else
                    shape = new MyEllipse(mousePos.X, mousePos.Y, _selectedStrokeThicknessNumeric, _selectedColorBrush);
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

        public ICommand ClearCanvasCommand
        {
            get
            {
                if (_clearCanvasCommand == null)
                    _clearCanvasCommand = new RelayCommand(param => this.ClearCanvas());
                return _clearCanvasCommand;
            }
        }

        public void ClearCanvas()
        {
            for (int i = _sceneObjects.Count - 1; i >= 0; i--)
                _sceneObjects.RemoveAt(i);
            string untitledText = helper.GetResourceValue("UntitledText", CultureInfo.CreateSpecificCulture(_currentCulture));
            WindowTitle = untitledText;
            WorkSaved = false;
        }

        public ICommand SaveCanvasToFileCommand
        {
            get
            {
                if (_saveCanvasToFileCommand == null)
                    _saveCanvasToFileCommand = new RelayCommand(param => this.SaveCanvasToFile(param));
                return _saveCanvasToFileCommand;
            }
        }

        public void SaveCanvasToFile(object currentCanvas)
        {
            Visual temp = (Visual)currentCanvas;

            RenderTargetBitmap target = new RenderTargetBitmap(_canvasWidth, _canvasHeight, 96d, 96d, PixelFormats.Default);
            target.Render(temp);

            string ext = System.IO.Path.GetExtension(_pathToLoadedFile);
            BitmapEncoder imageEncoder;
            switch (ext) {
                case ".png":
                    imageEncoder = new PngBitmapEncoder();
                    break;
                case ".jpg":
                    imageEncoder = new JpegBitmapEncoder();
                    break;
                case ".bmp":
                    imageEncoder = new BmpBitmapEncoder();
                    break;
                default:
                    imageEncoder = new JpegBitmapEncoder();
                    break;
            }
            imageEncoder.Frames.Add(BitmapFrame.Create(target));

            using (var fs = File.OpenWrite(_pathToLoadedFile))
            {
                imageEncoder.Save(fs);
            }
            WorkSaved = true;
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
            string updatedStatusBarTextKey = helper.GetResourceName(StatusBarText, CultureInfo.CreateSpecificCulture(_currentCulture));
            string updatedUntitledTextKey = helper.GetResourceName(WindowTitle.Replace(" - Dynamic Paint", ""),
                    CultureInfo.CreateSpecificCulture(_currentCulture));


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
            if (_pluginsAreLoaded)
                _pluginView.ChangeLanguagePlugin(chosenCulture);
            CultureInfo.DefaultThreadCurrentCulture = ChosenCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = ChosenCultureInfo;


            StatusBarText = helper.GetResourceValue(updatedStatusBarTextKey, CultureInfo.CreateSpecificCulture(_currentCulture));
            try
            {
                WindowTitle = helper.GetResourceValue(updatedUntitledTextKey, CultureInfo.CreateSpecificCulture(_currentCulture));
            }
            catch (Exception e)
            {
                return;
            }
        }

        public ICommand ImportPluginsCommand
        {
            get
            {
                if (_importPluginsCommand == null)
                    _importPluginsCommand = new RelayCommand(param => this.ImportPlugins());
                return _importPluginsCommand;
            }
        }

        public void ImportPlugins()
        {
            _pluginsContainer.ComposeParts(this);
            _pluginView.AcceptHostInterface(this as IHost);
            foreach (var resourceDictionary in _resourceDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
            _pluginsAreLoaded = true;
        }

        public ICommand ExitAppCommand
        {
            get
            {
                if (_exitAppCommand == null)
                    _exitAppCommand = new RelayCommand(param => this.ExitApp());
                return _exitAppCommand;
            }
        }

        public void ExitApp()
        {
            if (WorkSaved)
                Application.Current.MainWindow.Close();
        }

        public void SetCanvasWidth(int width)
        {
            CanvasWidth = width;
        }

        public void SetCanvasHeight(int height)
        {
            CanvasHeight = height;
        }
    }
}
