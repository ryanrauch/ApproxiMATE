using ApproxiMATE.Models;
//using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using ApproxiMATE.Helpers;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

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
            //position = new Position(30.3993258177538, -97.723581124856);
            position = new Position(0.00, 0.00);
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
                //DrawHexagons(App.Hexagonal.CenterLocation, 1);

                var regions = await App.approxiMATEService.GetZoneRegionsAsync();
                foreach (ZoneRegion region in regions.Where(r=>r.Type.Equals((int)RegionType.SocialDistrict)))
                {
                    var poly = await App.approxiMATEService.GetZoneRegionPolygonsAsync(region.RegionId);
                    MapMain.Polygons.Add(GetPolygon(poly, region));
                }
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
            //MapMain.Polygons.Clear();
            DrawHexagons(App.Hexagonal.CenterLocation, CalculateLayerFromCameraPositionZoom(e.Position.Zoom));
            
            return;
            for (int i = 0; i < 8; ++i)
            {
                DrawHexagons(App.Hexagonal.CenterLocation, (int)Math.Pow(3, i));
            }
            //DrawHexagons(App.Hexagonal.CenterLocation, 1);
            ////DrawHexagons(App.Hexagonal.CenterLocation, 2);
            //DrawHexagons(App.Hexagonal.CenterLocation, 3);
            //DrawHexagons(App.Hexagonal.CenterLocation, 9);
            //DrawHexagons(App.Hexagonal.CenterLocation, 27);
        }
        private int CalculateLayerFromCameraPositionZoom(double zoom)
        {
            if (zoom > 13)
                return 1;
            else if (zoom > 12.25)
                return 3;
            else if (zoom > 11)
                return 9;
            else if (zoom > 9.25)
                return 27;
            else if (zoom > 8)
                return 81;
            else if (zoom > 6)
                return 243;
            else if (zoom > 4.5)
                return 729;
            return 2187;
        }

        private void DrawHexagons(Position center, int layer)
        {
            int step = (layer * 3) % 15;
            // possibly detach clicked event before clearing?
            //MapMain.Polygons.Clear();
            if (App.Hexagonal is HexagonalEquilateralScale)
            {
                ((HexagonalEquilateralScale)App.Hexagonal).SetLayer(layer);
            }
            for (int col = -6; col < 7; ++col)
            {
                for (int row = -6; row < 7; ++row)
                {
                    Polygon hexPoly = App.Hexagonal.HexagonalPolygon(center, col, row);
                    if (hexPoly.Tag.ToString().Equals("T"))
                        hexPoly.FillColor = App.HeatGradient.SteppedColor(step);
                    else
                        hexPoly.FillColor = App.HeatGradient.SteppedColor(0);
                    if (step.Equals(App.HeatGradient.Min))
                        hexPoly.StrokeColor = App.HeatGradient.SteppedColor(step + 1);
                    else
                        hexPoly.StrokeColor = App.HeatGradient.SteppedColor(step);
                    //step = step + 1 % App.HeatGradient.Max;
                    //hexPoly.Tag = layer.ToString() + Constants.BoundingBoxDelim + col.ToString() + Constants.BoundingBoxDelim + row.ToString();
                    hexPoly.IsClickable = true;
                    hexPoly.Clicked += HexPoly_Clicked;
                    MapMain.Polygons.Add(hexPoly);
                }
            }
        }

        private void HexPoly_Clicked(object sender, EventArgs e)
        {
            string str = ((Polygon)sender).Tag.ToString();
            LabelScale.Text = str;
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
            var permission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (permission.Equals(PermissionStatus.Granted))
            {
                await Navigation.PushAsync(new ContactsPage());
            }
            else
            {
                await Navigation.PushAsync(new IssuePage("Please provide permission to view contacts."));
            }
        }
    }
}
