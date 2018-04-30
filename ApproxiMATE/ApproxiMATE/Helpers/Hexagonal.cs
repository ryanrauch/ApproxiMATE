using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public class Hexagonal : IHexagonal
    {
        private const double HEIGHT = 0.1;
        private const double WIDTH = 0.1;
        private const double HALF_HEIGHT = HEIGHT / 2;
        private const double HALF_WIDTH = WIDTH / 2;
        private const double QUARTER_WIDTH = WIDTH / 4;
        private const double EVEN_LONGITUDE_REPEAT = WIDTH + HALF_WIDTH;
        private const double EVEN_LATITUDE_REPEAT = HEIGHT;
        private double _latitude { get; set; }
        private double _longitude { get; set; }

        public Position CenterLocation => new Position(Math.Round(_latitude, 1), 
                                                       Math.Floor(_longitude / EVEN_LONGITUDE_REPEAT) * EVEN_LONGITUDE_REPEAT);
        public Position ExactLocation => new Position(_latitude, _longitude);

        public Hexagonal(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }
        /*
        // i is valid within [0,5]
        public Position HexagonalCorner(Position center, int i)
        {
            return HexagonalCorner(center, WIDTH / 2, i);
        }
        // i is valid within [0,5]
        public Position HexagonalCorner(Position center, double size, int i)
        {
            var angle_rad = Math.PI / 180 * (60 * i); // 60*i=angle_degrees
            return new Position(center.Latitude + size * Math.Sin(angle_rad),
                                center.Longitude + size * Math.Cos(angle_rad));
        }
        public Polygon HexagonalPolygonMath(Position center)
        {
            Polygon poly = new Polygon();
            for(int i = 0; i < 6; ++i)
            {
                Position p = HexagonalCorner(center, i);
                Debug.WriteLine("latitude: " + p.Latitude + ", longitude:" + p.Longitude);
                poly.Positions.Add(p);
            }
            return poly;
        }
        */
        public Polygon HexagonalPolygon(Position center)
        {
            Polygon poly = new Polygon();
            //poly.Positions.Add(new Position(center.Latitude + HALF_HEIGHT, center.Longitude - QUARTER_WIDTH)); //starts at top-left
            //poly.Positions.Add(new Position(center.Latitude + HALF_HEIGHT, center.Longitude + QUARTER_WIDTH));
            //poly.Positions.Add(new Position(center.Latitude, center.Longitude + HALF_WIDTH));
            //poly.Positions.Add(new Position(center.Latitude - HALF_HEIGHT, center.Longitude + QUARTER_WIDTH));
            //poly.Positions.Add(new Position(center.Latitude - HALF_HEIGHT, center.Longitude - QUARTER_WIDTH));
            //poly.Positions.Add(new Position(center.Latitude, center.Longitude - HALF_WIDTH));
            poly.Positions.Add(new Position(center.Latitude + HALF_HEIGHT, center.Longitude - QUARTER_WIDTH)); //starts at top-left
            poly.Positions.Add(new Position(center.Latitude + HALF_HEIGHT, center.Longitude + HALF_WIDTH));
            poly.Positions.Add(new Position(center.Latitude, center.Longitude + HALF_WIDTH + QUARTER_WIDTH)); //extend extra 1/4width
            poly.Positions.Add(new Position(center.Latitude - HALF_HEIGHT, center.Longitude + HALF_WIDTH));
            poly.Positions.Add(new Position(center.Latitude - HALF_HEIGHT, center.Longitude - QUARTER_WIDTH));
            poly.Positions.Add(new Position(center.Latitude, center.Longitude - HALF_WIDTH));
            return poly;
        }
        public Polygon HexagonalPolygon(Position center, int column, int row)
        {
            if(column % 2 == 0)
                return HexagonalPolygon(new Position(center.Latitude + row * HEIGHT, 
                                                     center.Longitude + column * WIDTH));
            return HexagonalPolygon(new Position(center.Latitude + (row * HEIGHT) - HALF_HEIGHT,
                                                 center.Longitude + column * WIDTH));
        }
    }
}
