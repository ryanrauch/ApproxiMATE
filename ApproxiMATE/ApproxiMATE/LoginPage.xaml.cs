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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent();
		}

        public async Task<Boolean> LoginProcess(string un, string pw)
        {
            var loginResult = await App.approxiMATEService.InitializeClientAsync(un, pw, false);
            if (loginResult.IsSuccessStatusCode)
            {
                if (SwitchSavePW.IsToggled)
                {
                    App.AccountService.SaveCredentials(un, pw);
                }
                App.IsUserLoggedIn = true;
                //System.Threading.Thread.Sleep(3000);
                return true;
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
                App.AccountService.DeleteCredentials();
            }
            return false;
        }

        protected override async void OnAppearing()
        {
            //var permissionAlways = await Utilities.CheckPermissionsAsync(Plugin.Permissions.Abstractions.Permission.LocationAlways);
            var permission = await Utilities.CheckPermissionsAsync(Plugin.Permissions.Abstractions.Permission.LocationWhenInUse);
            if(permission != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                //TODO: create error page
                //re-direct to different page.
                //for now do nothing
                await DisplayAlert("Location Permissions", "Please enable location services.", "OK");
            }

            string un = App.AccountService.UserName;
            string pw = null;
            if (un != null)
                pw = App.AccountService.Password;
            if (un != null && pw != null)
            {
                var result = await LoginProcess(un, pw);
                if (result)
                {
                    //Navigation.InsertPageBefore(new TabbedPageMain(), this);
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                StackLayoutLogin.IsVisible = true;
                LabelLoadingCredentials.IsVisible = false;
            }
            base.OnAppearing();
        }

        public async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string un = usernameEntry.Text,
                   pw = passwordEntry.Text;
            var result = await LoginProcess(un, pw);
            if (result)
            {
                var options = await App.approxiMATEService.GetApplicationOptionsAsync();
                App.AppOptions = options.OrderByDescending(x => x.OptionsDate).FirstOrDefault();
                if (App.AppUser.termsAndConditionsDate == null 
                    || App.AppUser.termsAndConditionsDate < App.AppOptions.OptionsDate)
                {
                    Navigation.InsertPageBefore(new TermsAndConditionsPage(), this);
                }
                else
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                }
                await Navigation.PopAsync();
            }
        }
    }
}