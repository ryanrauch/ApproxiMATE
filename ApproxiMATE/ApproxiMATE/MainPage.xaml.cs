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

namespace ApproxiMATE
{
	public partial class MainPage : ContentPage
	{
        private static readonly Distance INITIAL_DISTANCE = Distance.FromKilometers(1.25d);
		public MainPage()
		{
			InitializeComponent();
            MapMain.MapStyle = MapStyle.FromJson(Constants.GoogleMapStyleSilverLimited);
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
                string box = CoordinateFunctions.GetBoundingBox(position);
                MapMain.Polygons.Add(GetPolygon(box, Color.FromRgba(128, 0, 0, 128), Color.Transparent));
                for (int lat = -5; lat < 6; ++lat)
                {
                    for(int lon = -5; lon < 6; ++lon)
                    {
                        MapMain.Polygons.Add(GetPolygon(CoordinateFunctions.GetBoundingBoxNearby(box, lat, lon), Color.Transparent, Color.FromRgba(128, 0, 0, 128)));
                    }
                }
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

        //public GroundOverlay GetGroundOverlay(string box)
        //{
        //    GroundOverlay overlay = new GroundOverlay();
        //    var southWest = new Position(CoordinateFunctions.GetLatitudeCeilingFromBox(box), 
        //                                 CoordinateFunctions.GetLongitudeCeilingFromBox(box));
        //    var northEast = new Position(CoordinateFunctions.GetLatitudeFloorFromBox(box), 
        //                                 CoordinateFunctions.GetLongitudeFloorFromBox(box));
        //    overlay.Bounds = new Bounds(southWest, northEast);
        //    overlay.Transparency = 0.5f;
        //    return overlay;
        //}
        public async Task OnDebugMapButtonClicked(object sender, EventArgs e)
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
