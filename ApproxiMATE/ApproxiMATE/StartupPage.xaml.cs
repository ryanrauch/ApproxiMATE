using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMATE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartupPage : ContentPage
	{
        private Boolean _isBusy { get; set; }
		public StartupPage ()
		{
			InitializeComponent ();
            _isBusy = false;
		}

        public async void ButtonOpenMap_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BindableMapPage());
        }

        public async void ButtonPermission_OnClicked(object sender, EventArgs e)
        {
            if (_isBusy)
                return;
            _isBusy = true;
            ((Button)sender).IsEnabled = false;

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            await DisplayAlert("Pre - Results", status.ToString(), "OK");

            if (status != PermissionStatus.Granted)
            {
                await Utilities.CheckPermissionsAsync(Permission.Location);
                await DisplayAlert("Results", status.ToString(), "OK");
            }
            _isBusy = false;
            ((Button)sender).IsEnabled = true;
        }

        public async void ButtonLocation_OnClicked(object sender, EventArgs e)
        {
            if (_isBusy)
                return;
            _isBusy = true;
            ((Button)sender).IsEnabled = false;

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                await DisplayAlert("Debug", "passed checkpermissionstatusasync -" + status.ToString(), "OK");
                if (status != PermissionStatus.Granted)
                {
                    if (Device.RuntimePlatform == Device.Android 
                        && await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                    await DisplayAlert("Debug -" + results.ToString(), "passed requestpermissionsasync -" + status.ToString(), "OK");
                    status = results[Permission.LocationWhenInUse];
                }
                if (status == PermissionStatus.Granted)
                {
                    var results = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
                    LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                LabelGeolocation.Text = "Error: " + ex;
            }

            _isBusy = false;
            ((Button)sender).IsEnabled = true;
        }
	}
}