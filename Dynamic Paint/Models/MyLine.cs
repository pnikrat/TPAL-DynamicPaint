using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dynamic_Paint.Models
{
    public class MyLine : CanvasShapeViewModel
    {
        public MyLine(double x1, double y1, double x2, double y2)
        {
            Top = y1;
            Left = x1;
            ShapeData = new LineGeometry(new Point(x1, y1), new Point(x2, y2));
        }

        public override void UpdatePosition(double x2, double y2)
        {
            ShapeData = new LineGeometry(new Point(Left, Top), new Point(x2, y2));
        }
    }
}
