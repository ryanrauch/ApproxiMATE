using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ApproxiMATE;
using ApproxiMATE.iOS;
using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BindableMap), typeof(BindableMapRenderer))]
namespace ApproxiMATE.iOS
{
    public class BindableMapRenderer : MapRenderer
    {
        //BindableMapDelegate _overlayDelegate { get; set; }
        //MKPolygonRenderer polygonRenderer;
        public bool zebra { get; set; } = false;
        public UIView customPinView { get; set; }
        public IList<CustomPin> customPins { get; set; }
        public IList<Pin> mapPins { get; set; }

        public MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;
            if (annotation is MKUserLocation)
                return null;
            
            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
                throw new Exception("GetCustomPin() returned null.");
            annotationView = mapView.DequeueReusableAnnotation(customPin.Id.ToString());
            if(annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Id.ToString());
                annotationView.Image = UIImage.FromFile("pin.png");
                annotationView.CalloutOffset = new CGPoint(0, 0);
                annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                ((CustomMKAnnotationView)annotationView).Id = customPin.Id.ToString();
                ((CustomMKAnnotationView)annotationView).Url = customPin.Url;
            }
            annotationView.CanShowCallout = true;
            return annotationView;
        }

        
        public CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in mapPins)
            {
                if (pin.Position == position)
                {
                    return pin as CustomPin;
                }
            }
            return null;
        }

        MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
        {
            //if (polygonRenderer == null && !Equals(overlayWrapper, null))
            if (overlayWrapper != null)
            {
                var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
                switch(overlay)
                {
                    case MKPolygon polygon:
                        var prenderer = new MKPolygonRenderer(polygon)
                        {
                            FillColor = String.IsNullOrEmpty(polygon.Title) ? zebra ? UIColor.Yellow : UIColor.Purple : UIColor.Red,
                            Alpha = 0.4f
                        };
                        zebra = !zebra;
                        return prenderer;
                    default:
                        throw new Exception(String.Format("GetOverlayRenderer() {0} Not Supported", overlay.GetType()));
                }
                /*
                 MKPolygonRenderer polygonRenderer = new MKPolygonRenderer(overlay as MKPolygon)
                    {
                        FillColor = UIColor.Red,
                        //StrokeColor = UIColor.Black,
                        Alpha = 0.4f,
                        LineWidth = 10
                    };
                    return polygonRenderer;
                }*/
            }
            return null;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveOverlays(nativeMap.Overlays);
                    nativeMap.OverlayRenderer = null;
                    //polygonRenderer = null;

                    //CustomPins section
                    nativeMap.GetViewForAnnotation = null;
                    nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (BindableMap)e.NewElement;
                var nativeMap = Control as MKMapView;

                //_overlayDelegate = new BindableMapDelegate();
                //nativeMap.Delegate = _overlayDelegate;
                nativeMap.OverlayRenderer = GetOverlayRenderer;
                
                CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[formsMap.PolygonCoordinates.Count];

                int index = 0;
                foreach (var position in formsMap.PolygonCoordinates)
                {
                    coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
                    ++index;
                }

                /*CLLocationCoordinate2D[] remCoords = new CLLocationCoordinate2D[4];
                remCoords[0] = new CLLocationCoordinate2D(30.401807, -97.722484);
                remCoords[1] = new CLLocationCoordinate2D(30.402455, -97.722248);
                remCoords[2] = new CLLocationCoordinate2D(30.402561, -97.722623);
                remCoords[3] = new CLLocationCoordinate2D(30.402302, -97.722784);
                */
                var blockOverlay = MKPolygon.FromCoordinates(coords);
                //CustomMKPolygon cmkp = CustomMKPolygon.FromCoordinates(coords);
                blockOverlay.Title = "rockrose";
                //var blockOverlay = MKPolygon.FromCoordinates(coords, new MKPolygon[1] { MKPolygon.FromCoordinates(remCoords) });
                nativeMap.AddOverlay(blockOverlay);

                //add grid here?
                //double x = nativeMap.CenterCoordinate.Latitude;
                //double y = nativeMap.CenterCoordinate.Longitude;
                double x = 30.39, 
                       y = -97.72;
                //x = formsMap.MapPins[0].Position.Latitude;
                //y = formsMap.MapPins[0].Position.Longitude;
                for (int i = 0; i < 10; ++i)
                {
                    CLLocationCoordinate2D[] klick = new CLLocationCoordinate2D[4];
                    klick[0] = new CLLocationCoordinate2D(x, y);
                    klick[1] = new CLLocationCoordinate2D(x, y + 0.01);
                    klick[2] = new CLLocationCoordinate2D(x + 0.01, y + 0.01);
                    klick[3] = new CLLocationCoordinate2D(x + 0.01, y);
                    nativeMap.AddOverlay(MKPolygon.FromCoordinates(klick));
                    //if (formsMap.ZoomLevel.Kilometers > 2.0)
                    //    x += .1;
                    //else
                        x += 0.01;
                }

                
                //Custom Pin Section
                mapPins = formsMap.Pins;
                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
                
            }
        }
        public void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            if(!String.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(customView.Url));
            }
        }
        public void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();
            if (customView.Id == "Xamarin")
            {
                customPinView.Frame = new CGRect(0, 0, 200, 84);
                var image = new UIImageView(new CGRect(0, 0, 200, 84));
                image.Image = UIImage.FromFile("xamarin.png");
                customPinView.AddSubview(image);
                customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
                e.View.AddSubview(customPinView);
            }
        }
        public void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }
    }
}