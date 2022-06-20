using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeAnimator
{
    public class Square
    {
        public double x;
        public double y;
        public double side;
        public double moveXCoordinate = 1 / Math.Sqrt(2);
        public double moveYCoordinate = 1 / Math.Sqrt(2);

        public Square(double x, double y, double Side)
        {
            this.x = x; this.y = y; this.side = Side;
        }
    }
}
