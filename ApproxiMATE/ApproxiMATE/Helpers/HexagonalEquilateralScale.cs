using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public class HexagonalEquilateralScale : IHexagonal
    {
        private const double ZERORADIUS = 0.005; // 0.005 will create a hexagon with width of 1km (roughly)
        private const double ZEROHALFRADIUS = 0.0025;
        private const double ZEROWIDTH = 0.0075; // RADIUS + HALFRADIUS
        private readonly double ZEROHEIGHT = Math.Sqrt(3) * ZERORADIUS;
        private readonly double ZEROHALFHEIGHT = Math.Sqrt(3) * ZEROHALFRADIUS;

        private readonly List<int> LAYERS = new List<int> { 1, 3, 9, 27, 81, 243, 729, 2187 };
        private int _layer { get; set; }
        private double _flatRadius => _layer * ZERORADIUS;
        private double _flatHalfRadius => _flatRadius / 2;
        private double _flatWidth => _layer * ZEROWIDTH;
        private double _flatHeight => _layer * ZEROHEIGHT;
        private double _flatHalfHeight => _flatHeight / 2;
 
        private double _latitude { get; set; }
        private double _longitude { get; set; }

        Position IHexagonal.CenterLocation => new Position(Math.Floor(_latitude / ZEROHEIGHT) * ZEROHEIGHT,
                                                           Math.Floor(_longitude / ZEROWIDTH) * ZEROWIDTH);

        //Position IHexagonal.CenterLocationLayer => new Position(_latitude, _longitude);

        Position IHexagonal.ExactLocation => new Position(_latitude, _longitude);

        public Polygon HexagonalPolygon(Position center)
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
            poly.Tag = IdentifyHexagonFromCenter(center);
            return poly;
        }

        public Position AdjustCenter(Position center)
        {
            //Position adjusted = new Position(Math.Floor(_latitude / ZEROHEIGHT),
            //                                 Math.Floor(_longitude / ZEROWIDTH) * ZEROWIDTH);
            //double lat = adjusted.Latitude % 1.5;
            //double lon = adjusted.Longitude % 3;
            //if(adjusted.Longitude < 0)
            //{
            //    if(lon < -1.5)
            //    {

            //    }
            //}
            //if(lat == 0)
            //{
            //    // actual center
                
            //}
            //else if(adjusted.Latitude % 1.5 == 1.0)
            //if (adjusted.Longitude % 3 == 0)
            //{
            //    return adjusted;
            //}


            return adjusted;
        }

        public String IdentifyHexagonFromCenter(Position center)
        {
            double lat = center.Latitude / _flatHeight;
            double lon = center.Longitude / _flatWidth;
            String tag = String.Format("L:{0}{1}LAT:{2}{1}LON:{3} Center:{4}",
                                       _layer,
                                       Constants.BoundingBoxDelim,
                                       lat,
                                       lon,
                                       (lat % 1.5 == 0) && (lon % 3 == 0));
            
            if ((lat % 1.5 == 0) && (lon % 3 == 0))
                return "T";
            return "F";
            return tag;
        }

        public Polygon HexagonalPolygon(Position center, int column, int row)
        {
            if (column % 2 == 0)
                return HexagonalPolygon(new Position(center.Latitude + row * _flatHeight,
                                                        center.Longitude + column * _flatWidth));
            // if column number is odd, shift down half-height
            return HexagonalPolygon(new Position(center.Latitude + (row * _flatHeight) - _flatHalfHeight,
                                                    center.Longitude + column * _flatWidth));
        }

        public HexagonalEquilateralScale(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            _layer = LAYERS[0];
        }
        public void SetLayer(int layer)
        {
            if(!LAYERS.Contains(layer))
            {
                throw new ArgumentOutOfRangeException("LAYERS");
            }
            _layer = layer;
        }
    }
}
