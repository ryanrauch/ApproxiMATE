using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public class HexagonalEquilateralScale : IHexagonal
    {
        private const double ZERORADIUS = 0.005;         // 0.050   // 0.005 would create a hexagon with width of 1km (roughly)
        private const double ZEROHALFRADIUS = 0.0025;    // 0.025
        private const double ZEROWIDTH = 0.0075;         // 0.075   // RADIUS + HALFRADIUS
        private readonly double ZEROHEIGHT = Math.Sqrt(3) * ZERORADIUS;
        private readonly double ZEROHALFHEIGHT = Math.Sqrt(3) * ZEROHALFRADIUS;

        private double _scale { get; set; }
        private int _layer { get; set; }
        private bool _flat => _layer % 2 != 0;
        private double _flatRadius => _layer * ZERORADIUS;
        private double _flatHalfRadius => _flatRadius / 2;
        private double _flatWidth => _layer * ZEROWIDTH;
        //private double _flatHalfWidth => _flatWidth / 2;
        private double _flatHeight => _layer * ZEROHEIGHT;
        private double _flatHalfHeight => _flatHeight / 2;

        private double _latitude { get; set; }
        private double _longitude { get; set; }

        Position IHexagonal.CenterLocation => new Position(Math.Floor(_latitude / ZEROHEIGHT) * ZEROHEIGHT,
                                                           Math.Floor(_longitude * 100) / 100);

        Position IHexagonal.ExactLocation => new Position(_latitude, _longitude);

        public Polygon HexagonalPolygon(Position center)
        {
            String debugInfo = String.Format("Lat: {0}\nLon: {1}\n", center.Latitude, center.Longitude);
            if(_flat)
            {
                double lat_top = center.Latitude + _flatHalfHeight;
                double lat_bottom = center.Latitude - _flatHalfHeight;
                double lon_left = center.Longitude - _flatHalfRadius;
                double lon_right = center.Longitude + _flatHalfRadius;
                // positions start with top-left, rotating clockwise
                Polygon poly = new Polygon();
                poly.Positions.Add(new Position(lat_top, lon_left));
                poly.Positions.Add(new Position(lat_top, lon_right));
                poly.Positions.Add(new Position(center.Latitude, center.Longitude + _flatRadius));
                poly.Positions.Add(new Position(lat_bottom, lon_right));
                poly.Positions.Add(new Position(lat_bottom, lon_left));
                poly.Positions.Add(new Position(center.Latitude, center.Longitude - _flatRadius));
                return poly;
            }
            else
            {
                double lat_top = center.Latitude + _flatHalfRadius;
                double lat_bottom = center.Latitude - _flatHalfRadius;
                double lon_left = center.Longitude - _flatHalfHeight;
                double lon_right = center.Longitude + _flatHalfHeight;
                // positions start with bottom-left, rotating clockwise
                Polygon poly = new Polygon();
                poly.Positions.Add(new Position(lat_bottom, lon_left));
                poly.Positions.Add(new Position(lat_top, lon_left));
                poly.Positions.Add(new Position(center.Latitude + _flatRadius, center.Longitude));
                poly.Positions.Add(new Position(lat_top, lon_right));
                poly.Positions.Add(new Position(lat_bottom, lon_right));
                poly.Positions.Add(new Position(center.Latitude - _flatRadius, center.Longitude));
                return poly;
            }
        }

        public Polygon HexagonalPolygon(Position center, int column, int row)
        {
            if (_flat)
            {
                if (column % 2 == 0)
                    return HexagonalPolygon(new Position(center.Latitude + row * _flatHeight,
                                                         center.Longitude + column * _flatWidth));
                return HexagonalPolygon(new Position(center.Latitude + (row * _flatHeight) - _flatHalfHeight,
                                                     center.Longitude + column * _flatWidth));
            }
            else
            {
                //pointy-topped
                if (row % 2 == 0)
                    return HexagonalPolygon(new Position(center.Latitude + row * _flatWidth,
                                                         center.Longitude + column * _flatHeight));
                return HexagonalPolygon(new Position(center.Latitude + row * _flatWidth,
                                                     center.Longitude + column * _flatHeight - _flatHalfHeight));
            }
        }

        public HexagonalEquilateralScale(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            _scale = 24.0d;
            _layer = 1;
        }
        public void SetLayer(int layer)
        {
            _layer = layer;
        }
        public void SetZoomScale(double scale)
        {
            _scale = scale;
            if (_scale >= 12.0 && _scale < 13.0)
                _layer = 3;
            else if (_scale >= 13.0 && _scale < 14.0)
                _layer = 2;
            else if (_scale >= 14.0)
                _layer = 1;
        }
    }
}
