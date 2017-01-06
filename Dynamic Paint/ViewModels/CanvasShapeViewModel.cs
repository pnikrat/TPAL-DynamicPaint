using Dynamic_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dynamic_Paint.Models
{
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

        public virtual void UpdatePosition(double x2, double y2)
        {

        }
    }
}
