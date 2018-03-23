using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace ApproxiMATE
{
    public class CustomMap : Map
    {
        public List<Position> ShapeCoordinates { get; set; }

        public CustomMap()
        {
            ShapeCoordinates = new List<Position>();
        }
    }
}
