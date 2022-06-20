using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeAnimator
{
    public class Bullet
    {
        Ellipse ell = new Ellipse();
        public double x;
        public double y;
        public double radius = 2;
        public double moveXCoordinate;
        public double moveYCoordinate;
        public Canvas canvas;
        public Bullet(Canvas canvas,double x, double y, double moveX, double moveY)
        {
            this.canvas = canvas;
            this.x = x;
            this.y = y;
            this.moveXCoordinate = 10*moveX;
            this.moveYCoordinate = 10*moveY;
        }
        public void Draw()
        {
            ell.Width = radius*2;
            ell.Height = radius*2;
            ell.Stroke = Brushes.Red;
            canvas.Children.Add(ell);
            Canvas.SetLeft(ell, x - radius);
            Canvas.SetTop(ell, y - radius);

        }
    }
}
