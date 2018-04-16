using System;
using System.Collections.Generic;
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
    }
}