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
        public async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //var user = new User
            //{
            //    Username = usernameEntry.Text,
            //    Password = passwordEntry.Text
            //};

            //var isValid = AreCredentialsCorrect(user);
            //if (isValid)
            var loginResult = await App.approxiMATEService.InitializeClientAsync(usernameEntry.Text, passwordEntry.Text, false);
            if(loginResult.IsSuccessStatusCode)
            {
                var states = await App.approxiMATEService.GetZoneStatesAsync();
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }

        //public bool AreCredentialsCorrect(User user)
        //{
        //    return user.Username == Constants.Username && user.Password == Constants.Password;
        //}
    }
}