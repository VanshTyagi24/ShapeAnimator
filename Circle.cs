using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeAnimator
{
    public class Circle
    {
        public double x;
        public double y;
        public double radius;
        public double moveXCoordinate = 1/Math.Sqrt(2);
        public double moveYCoordinate = 1/ Math.Sqrt(2);
        //public double theta = Math.PI/4;

        // I should have a list of all unit vectors after collission for each circle and then 
        // calculate the resultant of them all... THINKKKKKKKK

        public Circle(double x, double y, double radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }
    }
}
