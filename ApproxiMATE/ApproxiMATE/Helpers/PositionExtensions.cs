/*using System;
using System.Collections.Generic;
using System.Text;
using XMaps = Xamarin.Forms.Maps;
using GMaps = Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public static class PositionExtensions
    {
        public static GMaps.Position ToGoogleMapsPosition(this XMaps.Position position)
        {
            return new GMaps.Position(position.Latitude, position.Longitude);
        }
        public static XMaps.Position ToXamarinMapsPosition(this GMaps.Position position)
        {
            return new XMaps.Position(position.Latitude, position.Longitude);
        }
    }
}
*/