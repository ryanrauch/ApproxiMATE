using ApproxiMATE.Models;
//using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using ApproxiMATE.Helpers;
using ApproxiMATE.Views;

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

                App.Hexagonal = new HexagonalEquilateral(position.Latitude, position.Longitude);

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
                foreach (ZoneRegion region in regions.Where(r=>r.Type.Equals((int)Region)
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
