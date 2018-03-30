/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MapKit;
using UIKit;

namespace ApproxiMATE.iOS
{
    public class BindableMapDelegate : MKMapViewDelegate
    {
        public override MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            switch (overlay)
            {
                case MKPolygon polygon:
                    var prenderer = new MKPolygonRenderer(polygon)
                    {
                        FillColor = UIColor.Red,
                        //StrokeColor = UIColor.Blue,
                        Alpha = 0.4f,
                        //LineWidth = 9
                    };
                    return prenderer;
                default:
                    throw new Exception($"Not supported: {overlay.GetType()}");
            }
            //return base.OverlayRenderer(mapView, overlay);
        }
    }
}
*/