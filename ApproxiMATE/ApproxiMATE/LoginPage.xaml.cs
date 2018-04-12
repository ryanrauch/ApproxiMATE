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

        public async Task LoginProcess(string un, string pw)
        {
            var loginResult = await App.approxiMATEService.InitializeClientAsync(un, pw, false);
            if (loginResult.IsSuccessStatusCode)
            {
                if (SwitchSavePW.IsToggled)
                {
                    App.AccountService.SaveCredentials(un, pw);
                }
                App.IsUserLoggedIn = true;
                var options = await App.approxiMATEService.GetApplicationOptionsAsync();
                App.AppOptions = options.OrderByDescending(x => x.OptionsDate).FirstOrDefault();
                if (App.AppUser.termsAndConditionsDate < App.AppOptions.OptionsDate)
                {
                    Navigation.InsertPageBefore(new TermsAndConditionsPage(), this);
                }
                else
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                }
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
                App.AccountService.DeleteCredentials();
            }
        }

        protected override async void OnAppearing()
        {
            string un = App.AccountService.UserName;
            string pw = null;
            if (un != null)
                pw = App.AccountService.Password;
            if(un != null && pw != null)
            {
                await LoginProcess(un, pw);
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
            await LoginProcess(un, pw);
        }
    }
}