using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public class Hexagonal
    {
        private const double HEIGHT = 0.1;
        private const double WIDTH = 0.1;
        private const double EVEN_LONGITUDE_REPEAT = 0.15;
        private const double EVEN_LATITUDE_REPEAT = 0.1;
        private double _latitude { get; set; }
        private double _longitude { get; set; }
        public Position CenterLocation
        {
            get
            {
                return new Position(Math.Floor(_latitude * 10) / 10, 
                                    Math.Floor(_longitude / 0.15));
            }
        }
        public Position ExactLocation
        {
            get
            {
                return new Position(_latitude, _longitude);
            }
        }
        public Hexagonal()
        { }
        public Hexagonal(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }
        // i is valid within [0,5]
        public Position HexagonalCorner(Position center, int i)
        {
            return HexagonalCorner(center, WIDTH / 2, i);
        }
        // i is valid within [0,5]
        public Position HexagonalCorner(Position center, double size, int i)
        {
            var angle_rad = Math.PI / 180 * (60 * i); // 60*i=angle_degrees
            return new Position(center.Latitude + size * Math.Cos(angle_rad),
                                center.Longitude + size * Math.Sin(angle_rad));
        }
        public Polygon HexagonalPolygon(Position center)
        {
            Polygon poly = new Polygon();
            for(int i = 0; i < 6; ++i)
            {
                poly.Positions.Add(HexagonalCorner(center, i));
            }
            return poly;
        }
    }
}
