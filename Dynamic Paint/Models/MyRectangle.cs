using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Dynamic_Paint.Models
{
    class MyRectangle : CanvasShapeViewModel
    {
        double originX;
        double originY;

        public MyRectangle(double x1, double y1, int strokeThickness, Brush strokeColor)
        {
            StrokeThickness = strokeThickness;
            StrokeColor = strokeColor;
            Top = originY = y1;
            Left = originX = x1;

            Rect temp = new Rect();
            temp.Location = new Point(Left, Top);
            temp.Size = new Size(Math.Abs(Left - x1), Math.Abs(Top - y1));
            
            ShapeData = new RectangleGeometry(temp);
        }

        public override void UpdatePosition(double x2, double y2)
        {
            Rect temp = new Rect();
            if (x2 < originX && y2 < originY) //2 cwiartka
            {
                double prevX = Left;
                double prevY = Top;
                Left = x2;
                Top = y2;
                temp.Location = new Point(Left, Top);
                temp.Size = new Size(Math.Abs(Left - originX), Math.Abs(Top - originY));
            }
            else if (x2 < originX) //2 lub 3 cwiartka
            {
                double prevX = Left;
                Left = x2;
                temp.Location = new Point(Left, Top);                
                temp.Size = new Size(Math.Abs(Left - originX), Math.Abs(Top - y2));
            }
            else if (y2 < originY) //1 lub 2 cwiartka
            {
                double prevY = Top;
                Top = y2;
                temp.Location = new Point(Left, Top);
                temp.Size = new Size(Math.Abs(Left - x2), Math.Abs(Top - originY));
            }
            else // 4 cwiartka
            {
                temp.Location = new Point(Left, Top);
                temp.Size = new Size(Math.Abs(Left - x2), Math.Abs(Top - y2));
            }

            ShapeData = new RectangleGeometry(temp);
        }
    }
}
