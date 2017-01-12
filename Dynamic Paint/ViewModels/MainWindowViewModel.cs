using Dynamic_Paint.Language;
using Dynamic_Paint.Models;
using Dynamic_Paint.Properties;
using Dynamic_Paint.Utilities;
using Dynamic_Paint.Views;
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dynamic_Paint.ViewModels
{
    /// <summary>
    /// ViewModel okna głównego aplikacji
    /// 
    /// </summary>
    public class MainWindowViewModel : ViewModelBase, IHost
    {
        public MainWindowViewModel()
        {
            _english = true;
            _polish = false;
            _currentCulture = "en";
            StatusBarText = Resources.StatusDefaultText;
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

            WindowTitle = Resources.UntitledText;

            helper = new ResourceHelper("Dynamic_Paint.Properties.Resources", GetType().Assembly);
            //zmiana workSaved z true na false, aby dodać '*' do tytułu rysunku
            _workSaved = true;
            WorkSaved = false;

            //pluginy powinny znajdować się w katalogu 'plugins' oraz zawierać przedrostek 'Plugin'
            string executingPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginsPath = System.IO.Path.Combine(executingPath, "plugins");
            if (!Directory.Exists(pluginsPath))
                Directory.CreateDirectory(pluginsPath);
            _pluginsCatalog = new DirectoryCatalog(pluginsPath, "Plugin*.dll");
            _pluginsContainer = new CompositionContainer(_pluginsCatalog);
            _pluginsAreLoaded = false;
        }

        #region Fields
        [ImportMany(typeof(IPlugin), AllowRecomposition = true)]
        private IEnumerable<Lazy<IPlugin, IPluginMetadata>> _plugins;

        private DirectoryCatalog _pluginsCatalog;
        private CompositionContainer _pluginsContainer;
        private bool _pluginsAreLoaded;

        [ImportMany(typeof(ResourceDictionary))]
        private IEnumerable<ResourceDictionary> _resourceDictionaries { get; set; }

        private ObservableCollection<FrameworkElement> _pluginsViews = new ObservableCollection<FrameworkElement>();
        private List<string> _loadedPluginsNames = new List<string>();

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
        #endregion Fields

        #region Properties
        public ObservableCollection<FrameworkElement> PluginsViews
        {
            get { return _pluginsViews; }
            set
            {
                _pluginsViews = value;
                base.OnPropertyChanged("PluginsViews");
            }
        }

        public bool WorkSaved
        {
            get { return _workSaved; }
            set
            {
                string stripString = " - " + _appTitle;
                //Logika odpowiadająca za '*' w tytule rysunku gdy praca nie jest zapisana
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
        #endregion Properties

        #region Commands and their methods
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
            StatusBarText = Resources.StatusDrawingLine;
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
            StatusBarText = Resources.StatusDrawingRectangle;
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
            StatusBarText = Resources.StatusDrawingEllipse;
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
            //StatusBar oraz tytuł aplikacji to dwa stringi, którym trzeba zmienić język w "ręczny" sposób
            string updatedStatusBarTextKey = helper.GetResourceName(StatusBarText, CultureInfo.CreateSpecificCulture(_currentCulture));
            string updatedUntitledTextKey = helper.GetResourceName(WindowTitle.Replace("* - Dynamic Paint", ""),
                    CultureInfo.CreateSpecificCulture(_currentCulture));


            string ChosenCultureString = (string)chosenCulture;
            CultureInfo ChosenCultureInfo = CultureInfo.CreateSpecificCulture(ChosenCultureString);
            //flagi do checkboxów w menu
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
            //były próby dynamicznej zmiany języka pluginów - nieudane
            //if (_pluginsAreLoaded)
                //_pluginView.ChangeLanguagePlugin(chosenCulture);
            CultureInfo.DefaultThreadCurrentCulture = ChosenCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = ChosenCultureInfo;


            StatusBarText = helper.GetResourceValue(updatedStatusBarTextKey, CultureInfo.CreateSpecificCulture(_currentCulture));
            try
            {
                WindowTitle = helper.GetResourceValue(updatedUntitledTextKey, CultureInfo.CreateSpecificCulture(_currentCulture)) + "*";
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
            //odśwież katalog z DLL
            _pluginsCatalog.Refresh();
            //import
            _pluginsContainer.ComposeParts(this);

            //lambda sprawdzająca czy importowane pluginy już są wgrane
            var newPlugins = _plugins.Where(p => !_loadedPluginsNames.Contains(p.Metadata.Name));
            var values = newPlugins.Select(p => p.Value);
            var names = newPlugins.Select(p => p.Metadata.Name);
            //wgraj i zarejestruj każdy nowy plugin
            foreach (IPlugin plug in values)
            {
                plug.AcceptHostInterface(this as IHost);
                _pluginsViews.Add(plug.GetPluginView());
                
            }
            foreach (string name in names)
            {
                _loadedPluginsNames.Add(name);
            }

            //import ResourceDictionaries ->element próby dynamicznej zmiany języka w pluginach - nieudanej
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
        #endregion Commands and their methods

        public void ExitApp()
        {
            if (!WorkSaved)
            {
                MessageBoxResult result = DiscardChangesView.ShowDialog();
                if (result == MessageBoxResult.No)
                    return;
            }
            Application.Current.MainWindow.Close();
        }
        #region IHost interface implementation
        public void SetCanvasWidth(int width)
        {
            CanvasWidth = width;
            List<CanvasShapeViewModel> copy = _sceneObjects.ToList();
            foreach (CanvasShapeViewModel x in copy)
            {
                if (x.ShapeData.Bounds.Right > width)
                    _sceneObjects.Remove(x);
            }
        }

        public void SetCanvasHeight(int height)
        {
            CanvasHeight = height;
            List<CanvasShapeViewModel> copy = _sceneObjects.ToList();
            foreach (CanvasShapeViewModel x in copy)
            {
                if (x.ShapeData.Bounds.Bottom > height)
                    _sceneObjects.Remove(x);
            }
        }

        public int GetCanvasWidth()
        {
            return CanvasWidth;
        }

        public int GetCanvasHeight()
        {
            return CanvasHeight;
        }

        public Brush GetCanvasBackground()
        {
            return CanvasBackground;
        }

        public void SetCanvasBackground(Brush background)
        {
            CanvasBackground = background;
        }
        #endregion IHost interface implementation
    }
}
