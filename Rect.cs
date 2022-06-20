using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeAnimator
{
    public class Rect
    {
        //Rectangle rec = new Rectangle();
        public double x;
        public double y;
        public double height;
        public double width;
        public double moveXCoordinate = 1 / Math.Sqrt(2);
        public double moveYCoordinate = 1 / Math.Sqrt(2);

        public Rect(double x, double y, double Height, double Width)
        {
            this.x = x; this.y = y; this.height = Height; this. width = Width;
        }
    }

}
