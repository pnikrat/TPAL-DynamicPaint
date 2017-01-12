using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Dynamic_Paint.Models
{
    class MyEllipse : CanvasShapeViewModel
    {
        double originX;
        double originY;

        public MyEllipse(double x1, double y1, int strokeThickness, Brush strokeColor)
        {
            StrokeThickness = strokeThickness;
            StrokeColor = strokeColor;
            Top = originY = y1;
            Left = originX = x1;

            double radiusX = Math.Abs(x1 - x1) / 2;
            double radiusY = Math.Abs(y1 - y1) / 2;
            Point center = new Point(radiusX + Left, radiusY + Top);
            ShapeData = new EllipseGeometry(center, radiusX, radiusY);
        }

        public override void UpdatePosition(double x2, double y2)
        {
            double radiusX = Math.Abs(originX - x2) / 2;
            double radiusY = Math.Abs(originY - y2) / 2;
            Point center;

            if (x2 < originX && y2 < originY) //2 cwiartka
                center = new Point(Left - radiusX, Top - radiusY);
            else if (x2 < originX) //2 lub 3 cwiartka
                center = new Point(Left - radiusX, Top + radiusY);
            else if (y2 < originY) // 1 lub 2 cwiartka
                center = new Point(Left + radiusX, Top - radiusY);
            else //4 cwiartka
                center = new Point(Left + radiusX, Top + radiusY);

            ShapeData = new EllipseGeometry(center, radiusX, radiusY);
        }
    }
}
