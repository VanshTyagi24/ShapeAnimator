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
    public class Spaceship
    {
        
        Rectangle rectangle = new Rectangle();
        public double X;
        public double Y;
        public double moveXCoordinate =  0;
        public double moveYCoordinate = 1;
        public double width;
        public double height;
        public int Speed = 1;
        Canvas canvas;
        public Spaceship(Canvas canvas, double X, double Y, double width, double height)
        {
            this.canvas = canvas;
            this.X = X;
            this.Y = Y;
            this.width = width;
            this.height = height;
        }

        public String checkDirection()
        {
            String url;
            if (moveXCoordinate > 0) return "images/rocketright.png";
            if (moveXCoordinate < 0) return "images/rocketleft.png";
            if (moveYCoordinate > 0) return "images/rocketdown.png";
            else return "images/rocketup.png";
        }

        public void UpdateSpeed()
        {
            if (moveXCoordinate != 0) moveXCoordinate = Speed * Math.Sign(moveXCoordinate);
            if (moveYCoordinate != 0) moveYCoordinate = Speed * Math.Sign(moveYCoordinate);
            
        }

        public void Draw()
        {
            
            rectangle.Width = width;
            rectangle.Height = height;
            ImageBrush ib = new ImageBrush();
            Image image = new Image();
            image.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(checkDirection());
            ib.ImageSource = image.Source;
            rectangle.Fill = ib;
            rectangle.Stroke = Brushes.Transparent;
            
            canvas.Children.Add(rectangle);
            Canvas.SetLeft(rectangle, X-width/2);
            Canvas.SetTop(rectangle, Y-height/2);

        }
    }
}
