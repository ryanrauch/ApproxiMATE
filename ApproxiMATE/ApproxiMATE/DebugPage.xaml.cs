using ApproxiMATE.Models;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
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
	public partial class DebugPage : ContentPage
	{
		public DebugPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            //await UpdateLocationPermission();
            base.OnAppearing();
        }

        public async Task UpdateLocationPermission()
        {
            var loc = await Utilities.CheckPermissionsAsync(Permission.Location);
            var opened = await Utilities.CheckPermissionsAsync(Permission.LocationWhenInUse);
            var always = await Utilities.CheckPermissionsAsync(Permission.LocationAlways);
            LabelLocationPermission.Text = String.Format("LocationWhenInUse:{0} LocationAlways:{1} Location:{2}", 
                                                         opened.ToString(), 
                                                         always.ToString(),
                                                         loc.ToString());
            //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            //LabelLocationPermission.Text = status.ToString();
        }

        public async Task UpdateContactsPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            LabelContactsPermission.Text = status.ToString();
        }

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

        public async void ButtonContactMultiple_OnClicked(object sender, EventArgs e)
        {
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            var multi = contacts.First(c => c.Numbers.Count > 1);
            if (multi != null)
            {
                LabelContactMultiple.Text = String.Join(" -- ", multi.Numbers);
            }
            else
            {
                LabelContactMultiple.Text = "No contacts with multiple numbers found.";
            }
        }

        public async void ButtonContactRyan_OnClicked(object sender, EventArgs e)
        {
            string search = "4767";
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            var ryan = contacts.FirstOrDefault(c => (!String.IsNullOrEmpty(c.Number) && c.Number.Contains(search))
                                                    || (c.Numbers != null && c.Numbers.Count > 0 && c.Numbers.Exists(n=>n.Contains(search))));
            if (ryan != null)
            {
                //LabelContactRyan.Text = String.IsNullOrEmpty(ryan.Number) ? ryan.Numbers[0] : ryan.Number;
                string parsed = ryan.Number.Split(new string[] { "stringValue=" }, StringSplitOptions.None)[1].Split(',')[0];
                LabelContactRyan.Text = String.Format("{0} -- {1} -- {2} -- {3}", ryan.Name, ryan.Number, ryan.Email, parsed);
            }
            else
            {
                LabelContactRyan.Text = search + " was not found in contacts.";
            }
        }

        public async void ButtonContactsRead_OnClicked(object sender, EventArgs e)
        {
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            LabelContactsRead.Text = contacts.Count.ToString();
        }

        public async void ButtonRefreshContactsPermission_OnClicked(object sender, EventArgs e)
        {
            await UpdateContactsPermission();
        }

        public async void ButtonRefreshLocationPermission_OnClicked(object sender, EventArgs e)
        {
            await UpdateLocationPermission();
        }

        public async void ButtonRefreshCoordinates_OnClicked(object sender, EventArgs e)
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
    }
}