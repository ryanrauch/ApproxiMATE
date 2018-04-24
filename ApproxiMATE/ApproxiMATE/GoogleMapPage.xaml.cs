using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace ApproxiMATE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GoogleMapPage : ContentPage
	{
        private readonly Distance _distance = Distance.FromKilometers(2.0d);
		public GoogleMapPage ()
		{
			InitializeComponent ();
            MapMain.MapStyle = MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);
		}
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var position = await Utilities.GetCurrentGeolocationGooglePositionAsync();
#if DEBUG
            position = new Position(30.3993258177538, -97.723581124856);
            MapMain.MoveToRegion(MapSpan.FromCenterAndRadius(position, _distance));
#else
            MapMain.MoveToRegion(MapSpan.FromCenterAndRadius(position, _distance));
#endif
        }
    }
}