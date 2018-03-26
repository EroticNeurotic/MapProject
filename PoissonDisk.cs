using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class PoissonDisk
    {
        private List<Point> ToAdd;
        private List<Point> PoissonPoints;
        private int Amount;
        private double Height;
        private double Width;
        private double Separation;
        private double Cellsize;
        private Point[,] Grid;
        private Random random;

        public PoissonDisk(double h, double w, int n, double s)
        {
            Amount = n;
            Height = h;
            Width = w;
            Separation = s;
            Cellsize = Separation / Math.Sqrt(2);

            Grid = new Point[
                (int)(Math.Ceiling(Width) / Cellsize),
                (int)(Math.Ceiling(Height) / Cellsize)
                ];

            ToAdd = new List<Point>();
            PoissonPoints = new List<Point>();

            random = new Random();
        }

        public List<Point> GetPoisson()
        {

            Point FirstPoint = new Point(random.Next(0, (int)Width), random.Next(0, (int)Height));
            ToAdd.Add(FirstPoint);
            PoissonPoints.Add(FirstPoint);

            Point GridReference = GridCoordinate(FirstPoint);
            Grid[(int)GridReference.x, (int)GridReference.y] = FirstPoint;

            while (ToAdd.Count != 0 && PoissonPoints.Count < Amount)
            {
                Point point = randompop();

                for (int count = 0; count < Amount; count++)
                {
                    Point newPoint = GenerateRandomPointFrom(point);

                    if (InGrid(newPoint))
                    {
                        if (!Neighbours(newPoint))
                        {
                            ToAdd.Add(newPoint);
                            PoissonPoints.Add(newPoint);

                            GridReference = GridCoordinate(newPoint);
                            Grid[(int)GridReference.x, (int)GridReference.y] = newPoint;
                        }
                    }
                }
            }

            return PoissonPoints;
        }

        private Point randompop()
        {

            int randomindex = random.Next(0, ToAdd.Count - 1);
            Point returnpoint = ToAdd[randomindex];
            //ToAdd.RemoveAt(randomindex);
            return returnpoint;
        }

        private Point GridCoordinate(Point point)
        {
            double X = (int)(point.x / Cellsize);
            double Y = (int)(point.y / Cellsize);

            return new Point(X, Y);
        }

        private Point GenerateRandomPointFrom(Point point)
        {

            double Rand1 = random.Next(1, 100);
            Rand1 /= 100;

            double Rand2 = random.Next(1, 100);
            Rand2 /= 100;

            double radius = Separation * (Rand1 + 1);
            double angle = 2 * Math.PI * Rand2;

            double newX = point.x + radius * Math.Cos(angle);
            double newY = point.y + radius * Math.Sin(angle);

            return new Point(newX, newY);
        }

        private bool InGrid(Point point)
        {
            if (point.x > 0 && point.x < Width && point.y > 0 && point.y < Height) { return true; }
            return false;
        }

        private bool Neighbours(Point point)
        {
            Point GridReference = GridCoordinate(point);
            List<Point> CheckGrid = surroundings(GridReference);

            foreach (Point checkPoint in CheckGrid)
            {
                if (checkPoint != null)
                {
                    if (Point.distance(point, checkPoint) < Separation)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<Point> surroundings(Point gridpoint)
        {
            List<Point> surroundinggrid = new List<Point>();

            for (int count = -2; count <= 2; count++)
            {
                for (int index = -2; index <= 2; index++)
                {
                    try
                    {
                        surroundinggrid.Add(Grid[(int)gridpoint.x + index, (int)gridpoint.y + count]);
                    }
                    catch
                    {
                    }
                }
            }

            return surroundinggrid;
        }
    }

    public class Point
    {
        public double x;
        public double y;

        public Point(double X, double Y)
        {
            x = X;
            y = Y;
        }

        public static double distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        }
    }
}
