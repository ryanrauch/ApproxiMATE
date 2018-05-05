using ApproxiMATE.Models;
//using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using ApproxiMATE.Helpers;

namespace ApproxiMATE
{
    public partial class MainPage : ContentPage
	{
        private static readonly Distance INITIAL_DISTANCE = Distance.FromKilometers(6.0d);
		public MainPage()
		{
			InitializeComponent();
            MapMain.MapStyle = MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);
        }
        protected override async void OnAppearing()
        {
            //THIS FUNCTION GETS CALLED EACH TIME NAVIGATION POPS FROM DIFFERENT PAGE
            base.OnAppearing();
            var position = await Utilities.GetCurrentGeolocationGooglePositionAsync();
#if DEBUG
            position = new Position(30.3993258177538, -97.723581124856);
            //position = new Position(0.00, 0.00);
#endif
            // pin for exact user location
            //MapMain.Pins.Add(new Pin
            //{
            //    Label = "Current Location",
            //    Position = position
            //});
            MapMain.MoveToRegion(MapSpan.FromCenterAndRadius(position, INITIAL_DISTANCE));
            try
            {
                MapMain.Polygons.Clear();
                /*
                string box = CoordinateFunctions.GetBoundingBox(position);
                MapMain.Polygons.Add(GetPolygon(box, Color.FromRgba(128, 0, 0, 128), Color.Transparent));
                for (int lat = -5; lat < 6; ++lat)
                {
                    for(int lon = -5; lon < 6; ++lon)
                    {
                        MapMain.Polygons.Add(GetPolygon(CoordinateFunctions.GetBoundingBoxNearby(box, lat, lon), Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                    }
                }*/

                //MapMain.GroundOverlays.Add(GetGroundOverlay(box));

                //MapMain.Pins.Add(GetPin(box));
                MapMain.Pins.Add(GetCurrentLocationPin(position));

                //App.Hexagonal = new HexagonalEquilateral(position.Latitude, position.Longitude);
                App.Hexagonal = new HexagonalEquilateralScale(position.Latitude, position.Longitude);
                if (App.HeatGradient == null)
                    App.HeatGradient = new HeatGradient();
                //int step = 0;
                //MapMain.Polygons.Clear();
                //Position centeredPosition = App.Hexagonal.CenterLocation;
                MapMain.CameraIdled += MapMain_CameraIdled;
                DrawHexagons(App.Hexagonal.CenterLocation, 1);
                //for(int col = -2; col < 3; ++col)
                //{
                //    for(int row = -2; row < 3; ++row)
                //    {
                //        Polygon hexPoly = App.Hexagonal.HexagonalPolygon(centeredPosition, col, row);
                //        hexPoly.FillColor = App.HeatGradient.SteppedColor(step);
                //        if (step.Equals(App.HeatGradient.Min))
                //            hexPoly.StrokeColor = App.HeatGradient.SteppedColor(step + 1);
                //        else
                //            hexPoly.StrokeColor = App.HeatGradient.SteppedColor(step);
                //        ++step;
                //        hexPoly.Tag = col.ToString() + Constants.BoundingBoxDelim + row.ToString();
                //        hexPoly.IsClickable = true;
                //        hexPoly.Clicked += HexPoly_Clicked;
                //        MapMain.Polygons.Add(hexPoly);
                //    }
                //}
                /*
                //original way to draw hexagons:
                Hexagonal hex = new Hexagonal(position.Latitude, position.Longitude);
                HeatGradient heat = new HeatGradient();
                int step = 0;
                for (int col = -2; col < 3; ++col)
                {
                    for(int row = -2; row < 3; ++row)
                    {
                        Polygon hexPoly = hex.HexagonalPolygon(hex.CenterLocation, col, row);
                        //hexPoly.FillColor = Color.FromRgba(0, 255, 0, 64);
                        hexPoly.FillColor = heat.SteppedColor(step);
                        if (step.Equals(heat.Min))
                            hexPoly.StrokeColor = heat.SteppedColor(step + 1); // show border when transparent
                        else
                            hexPoly.StrokeColor = heat.SteppedColor(step);
                        ++step;
                        hexPoly.Tag = col.ToString() + "-" + row.ToString();
                        hexPoly.IsClickable = true;
                        hexPoly.Clicked += HexPoly_Clicked;
                        MapMain.Polygons.Add(hexPoly);
                    }
                }
                */

                var regions = await App.approxiMATEService.GetZoneRegionsAsync();
                foreach (ZoneRegion region in regions.Where(r=>r.Type.Equals((int)RegionType.SocialDistrict)))
                {
                    var poly = await App.approxiMATEService.GetZoneRegionPolygonsAsync(region.RegionId);
                    MapMain.Polygons.Add(GetPolygon(poly, region));
                }


                //Polygon hexPoly = hex.HexagonalPolygon(hex.CenterLocation);
                //hexPoly.FillColor = Color.FromRgba(0, 255, 0, 128);
                //MapMain.Polygons.Add(hexPoly);
                //var northAustin = await App.approxiMATEService.GetZoneRegionPolygonsAsync(1);
                //MapMain.Polygons.Add(GetPolygon(northAustin));
                //var pflugerville = await App.approxiMATEService.GetZoneRegionPolygonsAsync(2);
                //MapMain.Polygons.Add(GetPolygon(pflugerville));

                /*
                string box = CoordinateFunctions.GetBoundingBox(position);
                MapMain.Polygons.Add(GetPolygon(box, Color.FromRgba(128, 0, 0, 128), Color.Transparent));
                string boxN = CoordinateFunctions.GetBoundingBoxNearby(box, 1, 0);
                MapMain.Polygons.Add(GetPolygon(boxN, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxNE = CoordinateFunctions.GetBoundingBoxNearby(box, 1, -1);
                MapMain.Polygons.Add(GetPolygon(boxNE, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxE = CoordinateFunctions.GetBoundingBoxNearby(box, 0, -1);
                MapMain.Polygons.Add(GetPolygon(boxE, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxSE = CoordinateFunctions.GetBoundingBoxNearby(box, -1, -1);
                MapMain.Polygons.Add(GetPolygon(boxSE, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxS = CoordinateFunctions.GetBoundingBoxNearby(box, -1, 0);
                MapMain.Polygons.Add(GetPolygon(boxS, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxSW = CoordinateFunctions.GetBoundingBoxNearby(box, -1, 1);
                MapMain.Polygons.Add(GetPolygon(boxSW, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxW = CoordinateFunctions.GetBoundingBoxNearby(box, 0, 1);
                MapMain.Polygons.Add(GetPolygon(boxW, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                string boxNW = CoordinateFunctions.GetBoundingBoxNearby(box, 1, 1);
                MapMain.Polygons.Add(GetPolygon(boxNW, Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                //MapMain.GroundOverlays.Add(GetGroundOverlay(box));
                */
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "OK");
            }
        }

        private void MapMain_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            double metersPerPixel = 156543.03392 * Math.Cos(e.Position.Target.Latitude * Math.PI / 180) / Math.Pow(2, e.Position.Zoom);
            //double zoomWidth = App.ScreenWidth * metersPerPixel / 1000;
            
            double zoomWidth = metersPerPixel / 10; //1000meters divided by 100px width
            LabelScale.Text = zoomWidth.ToString("F2") + "km " + e.Position.Zoom.ToString("F2");
            MapMain.Polygons.Clear();
            DrawHexagons(App.Hexagonal.CenterLocation, 1);
            DrawHexagons(App.Hexagonal.CenterLocation, 2);
            //DrawHexagons(App.Hexagonal.CenterLocation, 3);
        }

        private void DrawHexagons(Position center, int layer)
        {
            int step = (layer-1) * 10;
            // possibly detach clicked event before clearing?
            //MapMain.Polygons.Clear();
            if (App.Hexagonal is HexagonalEquilateralScale)
            {
                //((HexagonalEquilateralScale)App.Hexagonal).SetZoomScale(scale);
                ((HexagonalEquilateralScale)App.Hexagonal).SetLayer(layer);
            }
            for (int col = -10; col < 11; ++col)
            {
                for (int row = -10; row < 11; ++row)
                {
                    Polygon hexPoly = App.Hexagonal.HexagonalPolygon(center, col, row);
                    hexPoly.FillColor = App.HeatGradient.SteppedColor(0);
                    if (step.Equals(App.HeatGradient.Min))
                        hexPoly.StrokeColor = App.HeatGradient.SteppedColor(step + 1);
                    else
                        hexPoly.StrokeColor = App.HeatGradient.SteppedColor(step);
                    //step = step + 1 % App.HeatGradient.Max;
                    hexPoly.Tag = col.ToString() + Constants.BoundingBoxDelim + row.ToString();
                    hexPoly.IsClickable = true;
                    hexPoly.Clicked += HexPoly_Clicked;
                    MapMain.Polygons.Add(hexPoly);
                }
            }
        }

        private void HexPoly_Clicked(object sender, EventArgs e)
        {
            string str = ((Polygon)sender).Tag.ToString();
        }

        // This function assumes that the List of ZoneRegionPolygon's is already sorted by "Order" Column
        public Polygon GetPolygon(List<ZoneRegionPolygon> coordinates, ZoneRegion region)
        {
            Polygon poly = new Polygon();
            foreach (ZoneRegionPolygon c in coordinates)
            {
                poly.Positions.Add(new Position(c.Latitude, c.Longitude));
            }
            poly.FillColor = Color.FromHex(region.ARGBFill);
            poly.StrokeColor = Color.FromHex(region.ARGBStroke);
            poly.StrokeWidth = region.StrokeWidth;
            return poly;
        }

        public Polygon GetPolygon(string box, Color fill, Color stroke)
        {
            Polygon poly = new Polygon();
            poly.Positions.Add(new Position(CoordinateFunctions.GetLatitudeFloorFromBox(box),
                                            CoordinateFunctions.GetLongitudeFloorFromBox(box)));
            poly.Positions.Add(new Position(CoordinateFunctions.GetLatitudeFloorFromBox(box),
                                            CoordinateFunctions.GetLongitudeCeilingFromBox(box)));
            poly.Positions.Add(new Position(CoordinateFunctions.GetLatitudeCeilingFromBox(box),
                                            CoordinateFunctions.GetLongitudeCeilingFromBox(box)));
            poly.Positions.Add(new Position(CoordinateFunctions.GetLatitudeCeilingFromBox(box),
                                            CoordinateFunctions.GetLongitudeFloorFromBox(box)));
            poly.FillColor = fill;
            poly.StrokeColor = stroke;
            poly.StrokeWidth = 1;
            return poly;
        }
        public Pin GetCurrentLocationPin(Position position)
        {
            var pin = new Pin();
            pin.Position = position;
            pin.Label = "My Location";
            pin.Type = PinType.Generic;
            return pin;
        }
        public Pin GetPin(string box)
        {
            var pin = new Pin();
            //pin.Icon = BitmapDescriptorFactory.FromView(new MapBoxContentView());
            pin.Icon = BitmapDescriptorFactory.FromView(new ContentView
                                                        {
                                                            WidthRequest = 100,
                                                            HeightRequest = 100,
                                                            Content = new Label
                                                                      {
                                                                        Text = box,
                                                                        HorizontalTextAlignment = TextAlignment.Center,
                                                                        VerticalTextAlignment = TextAlignment.Center
                                                                      }
                                                        });
            pin.Position = CoordinateFunctions.GetCenterPositionFromBox(box);
            pin.Type = PinType.Generic;
            pin.Label = box;
            return pin;
        }

        public GroundOverlay GetGroundOverlay(string box)
        {
            GroundOverlay overlay = new GroundOverlay();
            var southWest = new Position(CoordinateFunctions.GetLatitudeCeilingFromBox(box),
                                         CoordinateFunctions.GetLongitudeCeilingFromBox(box));
            var northEast = new Position(CoordinateFunctions.GetLatitudeFloorFromBox(box),
                                         CoordinateFunctions.GetLongitudeFloorFromBox(box));
            //var boxView = new MapBoxContentView();
            //overlay.Icon = BitmapDescriptorFactory.FromView(boxView);
            overlay.Icon = BitmapDescriptorFactory.FromView(new ContentView
            {
                WidthRequest=100,
                HeightRequest=100,
                Content = new Label { Text = box }
            });
            overlay.Bounds = new Bounds(southWest, northEast);
            overlay.Transparency = 0.5f;
            return overlay;
        }

        public async void OnDebugMapButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DebugMapPage());
        }

        public async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            App.AccountService.DeleteCredentials();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }

        public async void OnDebugButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DebugPage());
        }

        public async void OnContactsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactsPage());
        }
    }
}
