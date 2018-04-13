using ApproxiMATE.Models;
using Plugin.Geolocator.Abstractions;
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
    public partial class TabbedPageMain : TabbedPage
    {
        public TabbedPageMain ()
        {
            InitializeComponent();
        }
        /*
        public async Task<Position> UpdateLocationBox()
        {
            var current = await Utilities.GetCurrentGeolocationAsync();
            LabelCurrentLatitude.Text = current.Latitude.ToString();
            LabelCurrentLongitude.Text = current.Longitude.ToString();
            LabelCurrentBox.Text = String.Format("{0}x{1} to {2}x{3}",
                                                 (Math.Floor(current.Latitude * 100) / 100).ToString("F"),
                                                 (Math.Floor(current.Longitude * 100) / 100).ToString("F"),
                                                 (Math.Floor(current.Latitude * 100) / 100 + 0.01).ToString("F"),
                                                 (Math.Floor(current.Longitude * 100) / 100 + 0.01).ToString("F"));
            return current;
        }

        public async void ButtonRefresh_OnClicked(object sender, EventArgs e)
        {
            await UpdateLocationBox();
        }

        public async void ButtonUpdateDB_OnClicked(object sender, EventArgs e)
        {
            var current = await UpdateLocationBox();
            if (current != null)
            {
                await App.approxiMATEService
                         .PutCurrentLocationAsync(new CurrentLocation()
                         {
                             Latitude = current.Latitude,
                             Longitude = current.Longitude,
                             UserId = App.AppUser.id.ToString()
                         });
            }
        }
        */
    }
}