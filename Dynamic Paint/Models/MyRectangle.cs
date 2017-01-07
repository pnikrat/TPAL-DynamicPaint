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

        public MyRectangle(double x1, double y1, double x2, double y2)
        {
            base.Top = originY = y1;
            base.Left = originX = x1;

            Rect temp = new Rect();
            temp.Location = new Point(Left, Top);
            temp.Size = new Size(Math.Abs(Left - x2), Math.Abs(Top - y2));
            
            ShapeData = new RectangleGeometry(temp);
        }

        public override void UpdatePosition(double x2, double y2)
        {
            Rect temp = new Rect();
            if (x2 < originX && y2 < originY)
            {
                double prevX = base.Left;
                double prevY = base.Top;
                base.Left = x2;
                base.Top = y2;
                temp.Location = new Point(Left, Top);
                temp.Size = new Size(Math.Abs(Left - originX), Math.Abs(Top - originY));
            }
            else if (x2 < originX)
            {
                double prevX = base.Left;
                base.Left = x2;
                temp.Location = new Point(Left, Top);                
                temp.Size = new Size(Math.Abs(Left - originX), Math.Abs(Top - y2));
            }
            else if (y2 < originY)
            {
                double prevY = base.Top;
                base.Top = y2;
                temp.Location = new Point(Left, Top);
                temp.Size = new Size(Math.Abs(Left - x2), Math.Abs(Top - originY));
            }
            else
            {
                temp.Location = new Point(Left, Top);
                temp.Size = new Size(Math.Abs(Left - x2), Math.Abs(Top - y2));
            }

            ShapeData = new RectangleGeometry(temp);
        }
    }
}
