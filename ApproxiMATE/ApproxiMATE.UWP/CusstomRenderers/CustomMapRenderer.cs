using ApproxiMATE;
using ApproxiMATE.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace ApproxiMATE.UWP
{
    class CustomMapRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                // Unsubscribe
            }
            if(e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MapControl;
                
                //nativeMap.MapServiceToken = "AmTIxyUr5Xz_peUhdPSNr-Y_xRStWkb2LE0Bosx-TsVZbYLYbWOJJET_KvjRLwEU";

                var coordinates = new List<BasicGeoposition>();
                foreach(var position in formsMap.ShapeCoordinates)
                {
                    coordinates.Add(new BasicGeoposition()
                    {
                        Latitude = position.Latitude,
                        Longitude = position.Longitude
                    });
                }
                var polygon = new MapPolygon();
                polygon.FillColor = Windows.UI.Color.FromArgb(128, 255, 0, 0);
                polygon.StrokeColor = Windows.UI.Color.FromArgb(128, 0, 0, 255);
                polygon.StrokeThickness = 5;
                polygon.Path = new Geopath(coordinates);
                nativeMap.MapElements.Add(polygon);
            }
        }
    }
}
