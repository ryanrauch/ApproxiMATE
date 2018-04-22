using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using ApproxiMATE.Controls;
using ApproxiMATE.iOS;
using Foundation;
using Google.Maps;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomGoogleMap), typeof(CustomGoogleMapRenderer))]
namespace ApproxiMATE.iOS
{
    public class CustomGoogleMapRenderer : MapRenderer
    {
        private MapView _mapView;
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
            {
                return;
            }
            var formsElement = Element as CustomGoogleMap;
            if (Control == null)
            {
                _mapView = new MapView();
                _mapView.MapType = MapViewType.Normal;
                _mapView.MapStyle = MapStyle.FromJson(Constants.GoogleMapStyleSilver, null);
                SetNativeControl(_mapView);
            }

        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("VisibleRegion"))
            {
                NativeMap.InfoTapped += InfoTappedFunction;
                NativeMap.TappedMarker = TapperMarkerFunction;
            }
        }
        private void InfoTappedFunction(object sender, GMSMarkerEventEventArgs e)
        {
            //throw new NotImplementedException();
        }
        private bool TapperMarkerFunction(MapView map, Marker marker)
        {
            map.MarkerInfoWindow = new GMSInfoFor(markerInfoWindow);
            return false;
        }
        private UIView markerInfoWindow(UIView view, Marker marker)
        {
            var v = new UIView(new RectangleF(0, 0, 200, 30));
            v.BackgroundColor = UIColor.Orange;
            v.Layer.CornerRadius = 10;

            var text1 = new UITextView(new RectangleF(60, 5, 180, 50));
            //text1.BackgroundColor = UIColor.Blue;
            text1.TextColor = UIColor.White;
            text1.Editable = false;
            text1.Font = UIFont.FromName("Helvetica-Bold", 10);
            text1.Text = marker.Title;
            text1.TextAlignment = UITextAlignment.Center;

            //https:--//github.com/amay077/Xamarin.Forms.GoogleMaps/issues/70
            //var img2 = new UIImageView(new RectangleF(245, 5, 50, 50));
            //img2.BackgroundColor = UIColor.Gray;
            //img2.Image = UIImage.FromBundle("info.png");
            //img2.ContentMode = UIViewContentMode.ScaleAspectFill;
            //img2.Layer.CornerRadius = 5;
            //img2.ClipsToBounds = true;

            v.AddSubview(text1);
            return v;
        }
    }
}