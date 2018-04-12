using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApproxiMATE
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            App.AccountService.DeleteCredentials();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
        public async Task UpdateLocationBox()
        {
            var current = await Utilities.GetCurrentGeolocationAsync();
            LabelCurrentLatitude.Text = current.Latitude.ToString();
            LabelCurrentLongitude.Text = current.Longitude.ToString();
            LabelCurrentBox.Text = String.Format("{0}x{1} to {2}x{3}",
                                                 (Math.Floor(current.Latitude * 100) / 100).ToString("F"),
                                                 (Math.Floor(current.Longitude * 100) / 100).ToString("F"),
                                                 (Math.Floor(current.Latitude * 100) / 100 + 0.01).ToString("F"),
                                                 (Math.Floor(current.Longitude * 100) / 100 + 0.01).ToString("F"));
        }

        public async void ButtonRefresh_OnClicked(object sender, EventArgs e)
        {
            await UpdateLocationBox();
        }

        public async void ButtonUpdateDB_OnClicked(object sender, EventArgs e)
        {

        }
    }
}
