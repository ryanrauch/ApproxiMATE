using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public class HexagonalEquilateral : IHexagonal
    {
        // coordinates are calculated based off of the diagram that can be seen here:
        // http:--//mathcentral.uregina.ca/QQ/database/QQ.09.07/h/martin4.html
        private const double RADIUS = 0.05;
        private const double HALFRADIUS = 0.025;
        private const double WIDTH = 0.075;
        private readonly double _height = Math.Sqrt(3) * RADIUS;
        private readonly double _halfHeight = Math.Sqrt(3) * HALFRADIUS;

        private double _latitude { get; set; }
        private double _longitude { get; set; }

        Position IHexagonal.CenterLocation => new Position(Math.Floor(_latitude / _height) * _height + _halfHeight, 
                                                           Math.Floor(_longitude * 10) / 10 + RADIUS);

        Position IHexagonal.ExactLocation => new Position(_latitude, _longitude);

        public Polygon HexagonalPolygon(Position center)
        {
            String debugInfo = String.Format("Lat: {0}\nLon: {1}\n", center.Latitude, center.Longitude);
            double lat_top = center.Latitude + _halfHeight;
            double lat_bottom = center.Latitude - _halfHeight;
            double lon_left = center.Longitude - HALFRADIUS;
            double lon_right = center.Longitude + HALFRADIUS;
            // positions start with top-left, rotating clockwise
            Polygon poly = new Polygon();
            poly.Positions.Add(new Position(lat_top, lon_left));
            poly.Positions.Add(new Position(lat_top, lon_right));
            poly.Positions.Add(new Position(center.Latitude, center.Longitude + RADIUS));
            poly.Positions.Add(new Position(lat_bottom, lon_right));
            poly.Positions.Add(new Position(lat_bottom, lon_left));
            poly.Positions.Add(new Position(center.Latitude, center.Longitude - RADIUS));
            return poly;
        }

        public Polygon HexagonalPolygon(Position center, int column, int row)
        {
            if (column % 2 == 0)
                return HexagonalPolygon(new Position(center.Latitude + row * _height, 
                                                     center.Longitude + column * WIDTH));
            return HexagonalPolygon(new Position(center.Latitude + (row * _height) - _halfHeight, 
                                                 center.Longitude + column * WIDTH));
        }

        public HexagonalEquilateral(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }
    }
}
