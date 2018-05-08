using ApproxiMATE.Services.Interfaces;
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
        public String VersionNumber { get; set; }

		public LoginPage ()
		{
			InitializeComponent();
            VersionNumber = DependencyService.Get<IAppVersionProvider>().Version;
            String loadingText = String.Format("Checking User Credentials [v{0}]", VersionNumber);
            LabelLoadingCredentials.Text = loadingText;
		}

        public async Task<Boolean> LoginProcess(string un, string pw)
        {
            var loginResult = await App.approxiMATEService.InitializeClientAsync(un, pw, false);
            if (loginResult.IsSuccessStatusCode)
            {
                if (SwitchSavePW.IsToggled)
                {
                    App.CredentialService.SaveCredentials(un, pw);
                }
                App.IsUserLoggedIn = true;
                //System.Threading.Thread.Sleep(3000);
                return true;
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
                App.CredentialService.DeleteCredentials();
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
                //await DisplayAlert("Location Permissions", "Please enable location services.", "OK");
                await Navigation.PushAsync(new IssuePage("Please enable location services."));
            }

            string un = App.CredentialService.UserName;
            string pw = null;
            if (un != null)
                pw = App.CredentialService.Password;
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

        private Boolean CheckVersionNumber()
        {
            String dbversion = String.Format("{0}.{1}.{2}", App.AppOptions.Version,
                                                            App.AppOptions.VersionMajor,
                                                            App.AppOptions.VersionMinor);
            if(VersionNumber.Equals(dbversion))
                return true;
            if (VersionNumber.Split('.').Count().Equals(3))
            {
                int va = Int32.Parse(VersionNumber.Split('.')[0]),
                    vb = Int32.Parse(VersionNumber.Split('.')[1]),
                    vc = Int32.Parse(VersionNumber.Split('.')[2]);
                if (va > App.AppOptions.Version)
                    return true;
                if (va < App.AppOptions.Version)
                    return false;
                if (vb > App.AppOptions.Version)
                    return true;
                if (vb < App.AppOptions.Version)
                    return false;
                if (vc > App.AppOptions.Version)
                    return true;
                if (vc < App.AppOptions.Version)
                    return false;
            }
            return false;
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
                if(!CheckVersionNumber())
                {
                    Navigation.InsertPageBefore(new IssuePage("Please update the application to continue use"), this);
                    await Navigation.PopAsync();
                    return;
                }
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