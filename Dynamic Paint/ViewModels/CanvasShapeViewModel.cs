using InternalShared;
using System.Windows.Media;

namespace Dynamic_Paint.Models
{
    /// <summary>
    /// ViewModel obiektu rysowanego na Canvas
    /// </summary>
    public class CanvasShapeViewModel : ViewModelBase
    {
        private double _top;

        public double Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                base.OnPropertyChanged("Top");
            }
        }

        private double _left;

        public double Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                base.OnPropertyChanged("Left");
            }
        }

        private Geometry _shapeData;

        public Geometry ShapeData
        {
            get
            {
                return _shapeData;
            }
            set
            {
                _shapeData = value;
                base.OnPropertyChanged("ShapeData");
            }
        }

        private int _strokeThickness;

        public int StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                base.OnPropertyChanged("StrokeThickness");
            }
        }

        private Brush _strokeColor;

        public Brush StrokeColor
        {
            get
            {
                return _strokeColor;
            }
            set
            {
                _strokeColor = value;
                base.OnPropertyChanged("StrokeColor");
            }
        }

        public virtual void UpdatePosition(double x2, double y2)
        {
            
        }
    }
}
