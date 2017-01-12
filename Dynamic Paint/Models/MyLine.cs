using System.Windows;
using System.Windows.Media;

namespace Dynamic_Paint.Models
{
    public class MyLine : CanvasShapeViewModel
    {
        public MyLine(double x1, double y1, int strokeThickness, Brush strokeColor)
        {
            StrokeThickness = strokeThickness;
            StrokeColor = strokeColor;
            Top = y1;
            Left = x1;
            ShapeData = new LineGeometry(new Point(x1, y1), new Point(x1, y1));
        }

        public override void UpdatePosition(double x2, double y2)
        {
            ShapeData = new LineGeometry(new Point(Left, Top), new Point(x2, y2));
        }
    }
}
