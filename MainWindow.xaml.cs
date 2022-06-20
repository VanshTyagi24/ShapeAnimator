using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;
using System.Media;

namespace ShapeAnimator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Spaceship sh;
        public MainWindow()
        {
            InitializeComponent();
            UpdateScore();
            pop.LoadAsync();
            gameoversound.LoadAsync();
            
        }
        double x1;
        double y1;
        double x2;
        double y2;
        int Score=0;
        bool drag = false;
        Ellipse ellipse = new Ellipse();
        Rectangle re = new Rectangle();
        Rectangle sq = new Rectangle();
        SoundPlayer pop = new SoundPlayer("Audio/burst.wav");
        SoundPlayer gameoversound = new SoundPlayer("Audio/gameover.wav");
        DispatcherTimer dt;
        int shape = 4;
        int level = 1;
        Window over;
        Window rules;
        bool gameOver = false;

        //List<Ellipse> circles = new List<Ellipse>();
        //Dictionary<Ellipse, Point> circles = new Dictionary<Ellipse, Point>();
        //Dictionary<Ellipse, int> moveX = new Dictionary<Ellipse, int>();
        //Dictionary<Circle, int> dic = new Dictionary<Circle, int>();

        List<Circle> circles = new List<Circle>();
        List<Rect> rectangles = new List<Rect>();
        List<Square> squares = new List<Square>();
        List<Bullet> bullets = new List<Bullet>();
        private void ShapeCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            drag = false;
            x1 = e.GetPosition(this).X;
            y1 = e.GetPosition(this).Y;

            if (shape == 1)
            {
                ShapeCanvas.Children.Remove(ellipse);
                double radius = Distance(x1, y1, x2, y2);
                Circle c = new Circle(x2, y2, radius);
                circles.Add(c);
                Draw(c);
            }
            if (shape == 2)
            {
                //ShapeCanvas.Children.Remove(r);
                re.Width = 2 * Distance(x2, y2, x1, y2);
                re.Height = 2 * Distance(x2, y2, x2, y1);
                Rect rectangle = new Rect(x2, y2, re.Height, re.Width);
                rectangles.Add(rectangle);
                re.Stroke = Brushes.Red;
                //ShapeCanvas.Children.Add(r);
                Canvas.SetLeft(re, x2 - re.Width/2 );
                Canvas.SetTop(re, y2 - re.Height/2);

            }
            if (shape == 3)
            {
                //ShapeCanvas.Children.Remove(r);
                sq.Width = 2 * Distance(x2, y2, x1, y2);
                sq.Height = sq.Width;
                Square square = new Square(x2, y2, sq.Height);
                squares.Add(square);
                sq.Stroke = Brushes.Green;
                //ShapeCanvas.Children.Add(r);
                Canvas.SetLeft(sq, x2 - sq.Width / 2);
                Canvas.SetTop(sq, y2 - sq.Height / 2);

            }

            // startbtn.IsEnabled = true;
            /*
            ellipse = new Ellipse();
            ellipse.Width = c.radius*2;
            ellipse.Height = c.radius*2;
            ellipse.Stroke = Brushes.Blue;
            ellipse.Fill = Brushes.Aqua;
            ellipse.Opacity = 1;
            //ellipse.SetValue(Canvas.LeftProperty, x1);
            ShapeCanvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, c.x-c.radius);
            Canvas.SetTop(ellipse, c.y-c.radius);
            */


            //Point centre = new Point();
            //circles.Add(ellipse,centre);
            //moveX.Add(ellipse, 1);
            //Console.WriteLine(centre.X);
        }

        private void ShapeCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            x2 = e.GetPosition (this).X;
            y2 = e.GetPosition (this).Y;
            drag = true;
            if (shape == 1)
            {
                ellipse = new Ellipse();
                ShapeCanvas.Children.Add(ellipse);
                
            }
            if (shape == 2)
            {
                re = new Rectangle();
                ShapeCanvas.Children.Add(re);
            }
            if(shape == 3)
            {
                sq = new Rectangle();
                ShapeCanvas.Children.Add(sq);
            }
            
        }
        private void ShapeCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == true)
            {
                x1 = e.GetPosition(this).X;
                y1 = e.GetPosition(this).Y;
                if(shape==1)
                    TempCircleDraw();
                if (shape == 2)
                    TempRectangleDraw();
                if (shape == 3)
                    TempSquareDraw();
            }

        }

        
        

        private void startbtn_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Start()
        {
            dt = new DispatcherTimer();
            dt.Tick += Dt_Tick;
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dt.Start();
            stopbtn.IsEnabled = true;
            startbtn.IsEnabled = false;
            
        }

        private void AutoGenerateCircles()
        {
            Random rnd = new Random();
            double radius = rnd.NextDouble()*30 + 15;
            double x;
            double y;
            do {
                 x = rnd.NextDouble() * (ShapeCanvas.ActualWidth - radius);
                 y = rnd.NextDouble() * (ShapeCanvas.ActualHeight - radius);
            }
            while (Math.Abs(sh.X-x)<150 && Math.Abs(sh.Y - y) < 150);
            Circle c = new Circle(x, y, radius);
            circles.Add(c);
            Draw(c);
        }
        private void AutoGenerateRectangles()
        {
            Random rnd = new Random();
            double width = rnd.NextDouble() * 50 + 20;
            double height = rnd.NextDouble() * 50 + 20;
            double x;
            double y;
            do
            {
                x = rnd.NextDouble() * (ShapeCanvas.ActualWidth - width);
                y = rnd.NextDouble() * (ShapeCanvas.ActualHeight - height);
            }
            while (Math.Abs(sh.X - x) < 150 && Math.Abs(sh.Y - y) < 150);
            Rect r = new Rect(x, y, width,height);
            rectangles.Add(r);
            drawRectangle(r);
        }
        private void AutoGenerateSquares()
        {
            Random rnd = new Random();
            double side = rnd.NextDouble() * 50 + 20;
            
            double x;
            double y;
            do
            {
                x = rnd.NextDouble() * (ShapeCanvas.ActualWidth - side);
                y = rnd.NextDouble() * (ShapeCanvas.ActualHeight - side);
            }
            while (Math.Abs(sh.X - x) < 150 && Math.Abs(sh.Y - y) < 150);
            Square s = new Square(x, y, side);
            squares.Add(s);
            drawSquare(s);
        }
        public void Draw(Circle c)
        {
            //double radius = Distance(x1, y1, x2, y2);
            ellipse = new Ellipse();
            ellipse.Width = c.radius * 2;
            ellipse.Height = c.radius * 2;
            ImageBrush ib = new ImageBrush();
            Image image = new Image();
            image.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("images/circleship.png");
            ib.ImageSource = image.Source;
            ellipse.Fill = ib;
            ellipse.Stroke = Brushes.Transparent;
           // ellipse.Fill = Brushes.Aqua;
            //ellipse.Opacity = 1;
            //ellipse.SetValue(Canvas.LeftProperty, x1);
            ShapeCanvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, c.x - c.radius);
            Canvas.SetTop(ellipse, c.y - c.radius);
        }

        public void drawRectangle(Rect r)
        {
            re = new Rectangle();
            re.Width = r.width;
            re.Height = r.height;

            ImageBrush ib = new ImageBrush();
            Image image = new Image();
            image.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("images/rectship.png");
            ib.ImageSource = image.Source;
            re.Fill = ib;
            re.Stroke = Brushes.Transparent;
            
            ShapeCanvas.Children.Add(re);
            Canvas.SetLeft(re, r.x - re.Width / 2);
            Canvas.SetTop(re, r.y - re.Height / 2);
        }

        public void drawSquare(Square s)
        {
            sq = new Rectangle();
            sq.Width = s.side;
            sq.Height = s.side;
            ImageBrush ib = new ImageBrush();
            Image image = new Image();
            image.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("images/squareship.png");
            ib.ImageSource = image.Source;
            sq.Fill = ib;
            sq.Stroke = Brushes.Transparent;
            
            ShapeCanvas.Children.Add(sq);
            Canvas.SetLeft(sq, s.x - sq.Width / 2);
            Canvas.SetTop(sq, s.y - sq.Height / 2);
        }
        public void ReDraw()
        {
            ShapeCanvas.Children.Clear();
            int i = 0;
            foreach (var c in circles)
            {
                Draw(c);
            }
            foreach(Rect r in rectangles)
            {
                drawRectangle(r);
            }
            foreach (Square s in squares)
            {
                drawSquare(s);
            }

            sh.Draw();
            foreach(Bullet bullet in bullets)
            {
                bullet.Draw();
            }
        }

        private void Dt_Tick(object? sender, EventArgs e)
        {

            
            int i = 0;
            foreach (Circle c in circles)
            {
                i++;
                for (int j = i; j < circles.Count; j++)
                {

                    double cDist = Math.Sqrt(Math.Pow((circles[j].x - c.x), 2) + Math.Pow((circles[j].y - c.y), 2));
                    
                    
                    
                    if (cDist <= c.radius + circles[j].radius)
                    {
                        
                        //Vansh's special algorithm
                        double slope = (circles[j].y - c.y) / (circles[j].x - c.x);
                        //double perpendicularSlope = -1 / slope;
                        double theta = Math.Atan(slope);

                        double tempX = Math.Cos(theta);
                        double tempY = Math.Sin(theta);
                        if(Distance(c.x+tempX , c.y+tempY , circles[j].x-tempX, circles[j].y-tempY) > cDist)
                        {
                            c.moveYCoordinate =tempY;
                            c.moveXCoordinate = tempX;
                            circles[j].moveYCoordinate = -tempX;
                            circles[j].moveXCoordinate = -tempY;
                        }
                        else
                        {
                            c.moveYCoordinate = -tempY;
                            c.moveXCoordinate = -tempX;
                            circles[j].moveYCoordinate = tempY;
                            circles[j].moveXCoordinate = tempX;
                        }
                        
                        circles[j].x += circles[j].moveXCoordinate;
                        circles[j].y += circles[j].moveYCoordinate;
                        

                    }
                    

                }

                foreach(Rect r in rectangles)
                {
                    if(Math.Abs(r.x - c.x) < r.width / 2 + c.radius  
                       &&
                       Math.Abs(r.y - c.y) < r.height / 2 + c.radius)
                    {
                        double cDist = Distance(r.x, r.y, c.x, c.y);
                        double slope = (r.y - c.y) / (r.x - c.x);
                        double theta = Math.Atan(slope);
                        double tempX = Math.Cos(theta);
                        double tempY = Math.Sin(theta);
                        
                        if (Distance(r.x+tempX, r.y+tempY, c.x-tempX, c.y-tempY)>cDist)
                        {
                            r.moveXCoordinate = tempX;
                            r.moveYCoordinate = tempY;
                            c.moveXCoordinate = -tempX;
                            c.moveYCoordinate = -tempY;
                        }
                        else
                        {
                            r.moveXCoordinate = -tempX;
                            r.moveYCoordinate = -tempY;
                            c.moveXCoordinate = tempX;
                            c.moveYCoordinate = tempY;
                        }
                    }
                    
                }
                foreach (Square s in squares)
                {
                    if (Math.Abs(s.x - c.x) < s.side / 2 + c.radius
                       &&
                       Math.Abs(s.y - c.y) < s.side / 2 + c.radius)
                    {
                        double cDist = Distance(s.x, s.y, c.x, c.y);
                        double slope = (s.y - c.y) / (s.x - c.x);
                        double theta = Math.Atan(slope);
                        double tempX = Math.Cos(theta);
                        double tempY = Math.Sin(theta);

                        if (Distance(s.x + tempX, s.y + tempY, c.x - tempX, c.y - tempY) > cDist)
                        {
                            s.moveXCoordinate = tempX;
                            s.moveYCoordinate = tempY;
                            c.moveXCoordinate = -tempX;
                            c.moveYCoordinate = -tempY;
                        }
                        else
                        {
                            s.moveXCoordinate = -tempX;
                            s.moveYCoordinate = -tempY;
                            c.moveXCoordinate = tempX;
                            c.moveYCoordinate = tempY;
                        }
                    }
                }
                if (c.x + c.radius + c.moveXCoordinate >= ShapeCanvas.ActualWidth || c.x - c.radius + c.moveXCoordinate <= 0)
                {
                    c.moveXCoordinate = -c.moveXCoordinate;
                }
                if (c.y + c.radius + c.moveYCoordinate >= ShapeCanvas.ActualHeight || c.y - c.radius + c.moveYCoordinate <= 0)
                {
                    c.moveYCoordinate = -c.moveYCoordinate;
                }
                c.x += c.moveXCoordinate;
                c.y += c.moveYCoordinate;

                //just to make sure circle is inside canvas
                //if(c.x > ShapeCanvas.ActualWidth || c.x <0) c.x += (c.moveXCoordinate/Math.Abs(c.moveXCoordinate)) * c.radius;
                //if (c.y > ShapeCanvas.ActualHeight || c.y < 0) c.y += (c.moveYCoordinate / Math.Abs(c.moveYCoordinate)) * c.radius;
            }
            int p = 0;
            foreach (Rect r in rectangles)
            {
                
                p++;
                for(int k=p; k<rectangles.Count; k++)
                {
                    if(Math.Abs(r.x - rectangles[k].x) < r.width/2+rectangles[k].width/2
                        &&
                        Math.Abs(r.y - rectangles[k].y) < r.height / 2 + rectangles[k].height / 2
                        )
                    {

                        double cDist = Distance(r.x, r.y, rectangles[k].x, rectangles[k].y);
                        double slope = (r.y - rectangles[k].y) / (r.x - rectangles[k].x);
                        double theta = Math.Atan(slope);
                        double tempX = Math.Cos(theta);
                        double tempY = Math.Sin(theta);

                        if (Distance(r.x + tempX, r.y + tempY, rectangles[k].x - tempX, rectangles[k].y - tempY) > cDist)
                        {
                            r.moveXCoordinate = tempX;
                            r.moveYCoordinate = tempY;
                            rectangles[k].moveXCoordinate = -tempX;
                            rectangles[k].moveYCoordinate = -tempY;
                        }
                        else
                        {
                            r.moveXCoordinate = -tempX;
                            r.moveYCoordinate = -tempY;
                            rectangles[k].moveXCoordinate = tempX;
                            rectangles[k].moveYCoordinate = tempY;
                        }
                        
                        //rectangles[k].x += rectangles[k].moveXCoordinate;
                        //rectangles[k].y += rectangles[k].moveYCoordinate;
                    }
                }
                foreach (Square s in squares)
                {
                    if (Math.Abs(s.x - r.x) < s.side / 2 + r.width / 2
                       &&
                       Math.Abs(s.y - r.y) < s.side / 2 + r.height/2)
                    {
                        double cDist = Distance(s.x, s.y, r.x, r.y);
                        double slope = (s.y - r.y) / (s.x - r.x);
                        double theta = Math.Atan(slope);
                        double tempX = Math.Cos(theta);
                        double tempY = Math.Sin(theta);

                        if (Distance(s.x + tempX, s.y + tempY, r.x - tempX, r.y - tempY) > cDist)
                        {
                            s.moveXCoordinate = tempX;
                            s.moveYCoordinate = tempY;
                            r.moveXCoordinate = -tempX;
                            r.moveYCoordinate = -tempY;
                        }
                        else
                        {
                            s.moveXCoordinate = -tempX;
                            s.moveYCoordinate = -tempY;
                            r.moveXCoordinate = tempX;
                            r.moveYCoordinate = tempY;
                        }
                    }
                }
                if (r.x + r.width/2 > ShapeCanvas.ActualWidth || r.x - r.width / 2 < 0)
                {
                    r.moveXCoordinate = -r.moveXCoordinate;
                }
                if (r.y + r.height/2 > ShapeCanvas.ActualHeight || r.y - r.height / 2 < 0)
                {
                    r.moveYCoordinate = -r.moveYCoordinate;
                }
                r.x += r.moveXCoordinate;
                r.y += r.moveYCoordinate;
            }
            int sp = 0;
            foreach (Square s in squares)
            {

                sp++;
                for (int k = sp; k < squares.Count; k++)
                {
                    if (Math.Abs(s.x - squares[k].x) < s.side / 2 + squares[k].side / 2
                        &&
                        Math.Abs(s.y - squares[k].y) < s.side / 2 + squares[k].side / 2
                        )
                    {
                        double cDist = Distance(s.x, s.y, squares[k].x, squares[k].y);
                        double slope = (s.y - squares[k].y) / (s.x - squares[k].x);
                        double theta = Math.Atan(slope);
                        double tempX = Math.Cos(theta);
                        double tempY = Math.Sin(theta);

                        if (Distance(s.x + tempX, s.y + tempY, squares[k].x - tempX, squares[k].y - tempY) > cDist)
                        {
                            s.moveXCoordinate = tempX;
                            s.moveYCoordinate = tempY;
                            squares[k].moveXCoordinate = -tempX;
                            squares[k].moveYCoordinate = -tempY;
                        }
                        else
                        {
                            s.moveXCoordinate = -tempX;
                            s.moveYCoordinate = -tempY;
                            squares[k].moveXCoordinate = tempX;
                            squares[k].moveYCoordinate = tempY;
                        }
                    }
                }
                if (s.x + s.side / 2 > ShapeCanvas.ActualWidth || s.x - s.side / 2 < 0)
                {
                    s.moveXCoordinate = -s.moveXCoordinate;
                }
                if (s.y + s.side / 2 > ShapeCanvas.ActualHeight || s.y - s.side / 2 < 0)
                {
                    s.moveYCoordinate = -s.moveYCoordinate;
                }
                s.x += s.moveXCoordinate;
                s.y += s.moveYCoordinate;
            }
            CheckSpaceShipCollision();
            if (gameOver == true)
            {
                GameOver();
                return;
            }
            CheckBulletCollision();
            ReDraw();
            

        }

        //method that burst spaceship on collision with shape
        private void CheckSpaceShipCollision()
        {
            
            foreach (Circle c in circles)
            {

                if (Math.Abs(sh.X - c.x) < sh.width / 2 + c.radius
                       &&
                       Math.Abs(sh.Y - c.y) < sh.height / 2 + c.radius)
                {
                    gameOver = true;
                    
                }


            }
            
            foreach (Rect r in rectangles)
            {

                if (Math.Abs(sh.X - r.x) < sh.width / 2 + r.width / 2
                       &&
                       Math.Abs(sh.Y - r.y) < sh.height / 2 + r.height / 2)
                {
                    gameOver = true;
                   
                }


            }
            
            foreach (Square s in squares)
            {

                if (Math.Abs(sh.X - s.x) < sh.width / 2 + s.side / 2
                       &&
                       Math.Abs(sh.Y - s.y) < sh.height / 2 + s.side / 2)
                {
                    gameOver = true;

                }


            }
        }

        //method that burst shapes on collision with spaceship
        private void SpaceShipCollisionBurst()
        {
            List<Circle> burstCircles = new List<Circle>();
            foreach(Circle c in circles)
            {
                
                if (Math.Abs(sh.X - c.x) < sh.width / 2 + c.radius
                       &&
                       Math.Abs(sh.Y - c.y) < sh.height / 2 + c.radius)
                {
                    burstCircles.Add(c);
                }


            }
            List<Rect> burstRectangles = new List<Rect>();
            foreach (Rect r in rectangles)
            {

                if (Math.Abs(sh.X - r.x) < sh.width / 2 + r.width / 2
                       &&
                       Math.Abs(sh.Y - r.y) < sh.height / 2 + r.height/2)
                {
                    burstRectangles.Add(r);
                }


            }
            List<Square> burstSquares = new List<Square>();
            foreach (Square s in squares)
            {

                if (Math.Abs(sh.X - s.x) < sh.width / 2 + s.side / 2
                       &&
                       Math.Abs(sh.Y - s.y) < sh.height / 2 + s.side / 2)
                {
                    burstSquares.Add(s);
                }


            }
            removeShapes(burstCircles,burstRectangles,burstSquares);

        }
        private void CheckBulletCollision()
        {
           
            foreach(Bullet bullet in bullets)
            {
                int i = 0;
                while (i < circles.Count)
                {
                    if (Distance(bullet.x, bullet.y, circles[i].x, circles[i].y) < bullet.radius + circles[i].radius)
                    {
                        circles.Remove(circles[i]);
                        Score++;
                        pop.Play();

                    }
                    i++;
                }
                i = 0;
                while (i < rectangles.Count)
                {
                    if (Math.Abs(bullet.x - rectangles[i].x) < bullet.radius + rectangles[i].width / 2
                           &&
                           Math.Abs(bullet.y - rectangles[i].y) < bullet.radius + rectangles[i].height / 2)
                    {
                        rectangles.Remove(rectangles[i]);
                        Score++;
                        pop.Play();
                    }
                    i++;
                }
                i = 0;
                while (i < squares.Count)
                {
                    if (Math.Abs(bullet.x - squares[i].x) < bullet.radius + squares[i].side / 2
                           &&
                           Math.Abs(bullet.y - squares[i].y) < bullet.radius + squares[i].side / 2)
                    {
                        squares.Remove(squares[i]);
                        Score++;
                        pop.Play();
                    }
                    i++;
                }
        
            }
            CheckCount();
            UpdateScore();


        }
        private void removeShapes(List<Circle> burstCircles, List<Rect> burstRectangles, List<Square> burstSquares)
        {
            foreach(Circle c in burstCircles)
            {
                circles.Remove(c);
                Score++;
                pop.Play();
            }
            foreach (Rect r in burstRectangles)
            {
                rectangles.Remove(r);
                Score++;
                pop.Play();
            }
            foreach(Square s in burstSquares)
            {
                squares.Remove(s);
                Score++;
                pop.Play();
            }
            UpdateScore();
        }

        Rectangle createRec(Rect r)
        {
            Rectangle r1 = new Rectangle();
            r1.Width = r.width;
            r1.Height = r.height;
            Canvas.SetLeft(r1, r.x);
            Canvas.SetTop(r1, r.y);
            return r1;
        }

        public void TempCircleDraw()
        {


            //ShapeCanvas.Children.Remove(ellipse);
            //ellipse = new Ellipse();
            double radius = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.Stroke = Brushes.Blue;
            //ellipse.Fill = Brushes.Aqua;
            //ellipse.Opacity = 1;

            Canvas.SetLeft(ellipse, x2 - radius);
            Canvas.SetTop(ellipse, y2 - radius);
        }

        public void TempRectangleDraw()
        {
            double Width = 2 * Math.Abs(x2 - x1);
            double Height = 2 * Math.Abs(y2 - y1);
            re.Width = Width;
            re.Height = Height;
            re.Stroke = Brushes.Red;
            Canvas.SetLeft(re, x2 - Width / 2);
            Canvas.SetTop(re, y2 - Height / 2);
        }

        public void TempSquareDraw()
        {
            sq.Width = 2 * Distance(x2, y2, x1, y2);
            sq.Height = sq.Width;
     
            sq.Stroke = Brushes.Green;
            //ShapeCanvas.Children.Add(r);
            Canvas.SetLeft(sq, x2 - sq.Width / 2);
            Canvas.SetTop(sq, y2 - sq.Height / 2);
        }



        public double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        private void stopbtn_Click(object sender, RoutedEventArgs e)
        {
            dt.Stop();
            startbtn.IsEnabled = true;
            stopbtn.IsEnabled = false;  
        }

        private void circle_Click(object sender, RoutedEventArgs e)
        {
            shape = 1;
        }

        private void rectangle_Click(object sender, RoutedEventArgs e)
        {
            shape = 2;
        }

        private void square_Click(object sender, RoutedEventArgs e)
        {
            shape = 3;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sh = new Spaceship(ShapeCanvas, ShapeCanvas.ActualWidth/2-12.5, ShapeCanvas.ActualHeight-25, 30, 50);
            sh.Draw();
            DispatcherTimer shTimer = new DispatcherTimer();
            shTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            shTimer.Tick += ShTimer_Tick;
            shTimer.Start();
        }

        private void ShTimer_Tick(object? sender, EventArgs e)
        {
            if (sh.X + sh.width / 2 > ShapeCanvas.ActualWidth || sh.X - sh.width / 2 < 0) sh.moveXCoordinate = -sh.moveXCoordinate;
            if (sh.Y + sh.height / 2 > ShapeCanvas.ActualHeight || sh.Y - sh.height / 2 < 0) sh.moveYCoordinate = -sh.moveYCoordinate;
            //sh.Speed = ShipSpeed;
           
            sh.X += sh.moveXCoordinate;
            sh.Y += sh.moveYCoordinate;
            List<Bullet> remBullet = new List<Bullet>();
            foreach(Bullet bullet in bullets)
            {
                if (bullet.x > ShapeCanvas.ActualWidth || bullet.x < 0 ||
                    bullet.y > ShapeCanvas.ActualHeight || bullet.y  < 0
                    )
                {
                    remBullet.Add(bullet); 
                }
                
                bullet.x += bullet.moveXCoordinate;
                bullet.y += bullet.moveYCoordinate;
            }
            RemoveBullet(remBullet);
            ReDraw();
        }

        private void RemoveBullet(List<Bullet> remBullet)
        {
            foreach(Bullet bullet in remBullet)
            {
                bullets.Remove(bullet);
            }
        }

        private void ShapeCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Right && sh.moveXCoordinate==0)
            {
                sh.moveXCoordinate = sh.Speed;
                sh.moveYCoordinate = 0;
                //Replace(sh.width, sh.height);
                double temp = sh.width;
                sh.width = sh.height;
                sh.height = temp;
               
            }
            if (e.Key == Key.Left && sh.moveXCoordinate == 0)
            {
                sh.moveXCoordinate = -sh.Speed;
                sh.moveYCoordinate = 0;
                double temp = sh.width;
                sh.width = sh.height;
                sh.height = temp;
            }
            if (e.Key == Key.Up && sh.moveYCoordinate==0)
            {
                sh.moveYCoordinate = -sh.Speed;
                sh.moveXCoordinate = 0;
                double temp = sh.width;
                sh.width = sh.height;
                sh.height = temp;
            }
            if (e.Key == Key.Down && sh.moveYCoordinate == 0)
            {
                sh.moveYCoordinate = sh.Speed;
                sh.moveXCoordinate = 0;
                double temp = sh.width;
                sh.width = sh.height;
                sh.height = temp;
            }
            if(e.Key == Key.F)
            {
                Bullet bullet = new Bullet(ShapeCanvas, sh.X, sh.Y, sh.moveXCoordinate, sh.moveYCoordinate);
                bullet.Draw();
                bullets.Add(bullet);
            }
        }
        public void UpdateScore()
        {
            scoreText.Text = "Score: " + Score;
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            if(sh.Speed <5)
                sh.Speed++;
            sh.UpdateSpeed();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {

            if (sh.Speed > 1) sh.Speed--;
            sh.UpdateSpeed();
        }

        private void Practice_Click(object sender, RoutedEventArgs e)
        {
            level = 0;
            circles.Clear();
            rectangles.Clear();
            squares.Clear();
            ShapeCanvas.Children.Clear();
            circle.IsEnabled = true;
            square.IsEnabled = true;
            rectangle.IsEnabled = true;
            shape = 1;
            Level1.IsEnabled = false;
            Level2.IsEnabled = false;
            Story.IsEnabled = true;
            Score = 0;
            UpdateScore();
        }

        private void Story_Click(object sender, RoutedEventArgs e)
        {
            level = 1;
            circle.IsEnabled = false;
            square.IsEnabled = false;
            rectangle.IsEnabled = false;
            shape = 4;
            Level1.IsEnabled = true;
            Level2.IsEnabled = true;
            Story.IsEnabled = false;
            Score = 0;
            UpdateScore();
        }

        private void Level1_Click(object sender, RoutedEventArgs e)
        {
            resetEnemies();
            level1();
        }
        private void resetEnemies()
        {
            rectangles.Clear();
            squares.Clear();
            circles.Clear();
        }
        
        private void level1()
        {
            level = 1;
            for (int i = 0; i < 5; i++)
            {
                AutoGenerateCircles();
                AutoGenerateRectangles();
                AutoGenerateSquares();
                Level1.IsEnabled = false;
                Level2.IsEnabled = true;

            }
        }

        private void Level2_Click(object sender, RoutedEventArgs e)
        {
            resetEnemies();
            level2();
        }

        private void level2()
        {
            level = 2;
            for (int i = 0; i < 10; i++)
            {
                AutoGenerateCircles();
                AutoGenerateRectangles();
                AutoGenerateSquares();
                Level2.IsEnabled = false;
                Level1.IsEnabled = true;
            }
        }

        private void CheckCount()
        {
            if (circles.Count + rectangles.Count + squares.Count < 5)
            {
                if (level == 1) level1();
                if (level == 2) level2();
            }
               

        }
        private TextBlock createAnimatedText(int size)
        {
            TextBlock text = new TextBlock()
            {
                RenderTransform = new RotateTransform()
            };
            text.FontFamily = new FontFamily("Bebas Neue");
            text.FontSize = size;
            text.Foreground = Brushes.White;
            return text;
        }

        private void TextAnimation(TextBlock text)
        {

            Storyboard storyboard = new Storyboard();
            storyboard.Duration = new Duration(TimeSpan.FromSeconds(1));
            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = storyboard.Duration,

                RepeatBehavior = RepeatBehavior.Forever

            };


            Storyboard.SetTarget(rotateAnimation, text);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            storyboard.Children.Add(rotateAnimation);
            storyboard.Begin();
        }

        private void GameOver()
        {
            dt.Stop();
            resetEnemies();
            gameoversound.LoadAsync();
            over = new Window();
            over.Width = 500;
            over.Height = 500;
            over.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Grid overGrid = new Grid();
            StackPanel overPanel = new StackPanel();
            overPanel.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock overHead = createAnimatedText(35);
            overHead.Padding = new Thickness(5);
            overHead.Margin = new Thickness(0, 100, 0, 0);
            overHead.Text = "GAME OVER!!!!";
            //overHead.Foreground = Brushes.Black;
            TextAnimation(overHead);
            overPanel.Children.Add(overHead);

            TextBlock overWinning = createAnimatedText(25);
            overWinning.Padding = new Thickness(5);
            overWinning.Margin = new Thickness(0, 30, 0, 0);
            overWinning.Text = "SCORE: "+Score +
                "\n\nSHAPE ANIMATOR BY\nVANSH";
            //overWinning.Foreground = Brushes.Black;
            //TextAnimation(overWinning);
            overPanel.Children.Add(overWinning);

            Button contBtn = new Button();
            contBtn.Height = 25;

            contBtn.Width = 100;
            contBtn.Content = "CONTINUE";
            contBtn.Click += ContBtn_Click; 
            contBtn.Margin = new Thickness(10);
            overPanel.Children.Add(contBtn);
            overGrid.Children.Add(overPanel);
            ImageBrush overBG = new ImageBrush();
            Image overImage = new Image();
            overImage.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("images/darkover.jpg");
            overBG.ImageSource = overImage.Source;
            overGrid.Background = overBG;
            over.Content = overGrid;
            over.Title = "GAME OVER";
            over.Closed += closeevnt;
            over.Show();
            gameoversound.Play();
            MainWin.IsEnabled = false;
        }

        private void ContBtn_Click(object sender, RoutedEventArgs e)
        {
            Score = 0;
            UpdateScore();
            Level1.IsEnabled = true;
            Level2.IsEnabled = true;
            gameOver = false;
            startbtn.IsEnabled = true;
            stopbtn.IsEnabled = false;
            MainWin.IsEnabled = true;
            over.Close();
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            ShowRules();
        }

        public void ShowRules()
        {
            MainWin.IsEnabled = false;
            
            rules = new Window();
            rules.Width = 700;
            rules.Height = 500;
            rules.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Grid overGrid = new Grid();
            StackPanel overPanel = new StackPanel();
            overPanel.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock overHead = createAnimatedText(35);
            overHead.Padding = new Thickness(5);
            overHead.Margin = new Thickness(0, 100, 0, 0);
            overHead.Text = "RULES";
            //overHead.Foreground = Brushes.Black;
            TextAnimation(overHead);
            overPanel.Children.Add(overHead);

            TextBlock overText = createAnimatedText(25);
            overText.Padding = new Thickness(5);
            overText.Margin = new Thickness(0, 30, 0, 0);
            overText.Text =  "1. PRESS F key to fire bullet.\n" +
                                "2. Use Arrow Keys to Control the Spaceship.\n"+
                                "3. Shooting an Enemy Ship will give you a point.\n" +
                                "4. You will DIE if you touch Enemy Ship.";
            //overWinning.Foreground = Brushes.Black;
            //TextAnimation(overWinning);
            overPanel.Children.Add(overText);

            Button contBtn = new Button();
            contBtn.Height = 25;

            contBtn.Width = 100;
            contBtn.Content = "CONTINUE";
            contBtn.Click += ContBtn_Click2;
            contBtn.Margin = new Thickness(10);
            overPanel.Children.Add(contBtn);
            overGrid.Children.Add(overPanel);
            ImageBrush overBG = new ImageBrush();
            Image overImage = new Image();
            overImage.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("images/rules.jpg");
            overBG.ImageSource = overImage.Source;
            overGrid.Background = overBG;
            rules.Content = overGrid;
            rules.Title = "Rules";
            rules.Closed += closeevnt;
            rules.Show();
            
        }

        private void closeevnt(object? sender, EventArgs e)
        {
            MainWin.IsEnabled = true;
            
        }

        private void ContBtn_Click2(object sender, RoutedEventArgs e)
        {



            MainWin.IsEnabled = true;
            rules.Close();
        }

        //private void speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    ShipSpeed = (int)speed.Value;
        //}

    }
}