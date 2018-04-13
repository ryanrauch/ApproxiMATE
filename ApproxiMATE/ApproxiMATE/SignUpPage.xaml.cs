using ApproxiMATE.Models;
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
	public partial class SignUpPage : ContentPage
	{
		public SignUpPage ()
		{
			InitializeComponent();
		}
        public async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            var user = new UserRegister
            {
                UserName = EntryUserName.Text,
                Password = EntryPassword.Text,
                Email = EntryEmail.Text,
                PhoneNumber = EntryPhone.Text,
                DateofBirth = DatePickerDOB.Date,
                FirstName = EntryFirstName.Text,
                LastName = EntryLastName.Text
            };

            // Sign up logic goes here
            var signUpValid = AreDetailsValid(user, EntryConfirmPassword.Text);
            if (signUpValid)
            {
                var registered = await App.approxiMATEService.RegisterUserAsync(user);
                if (registered)
                {
                    var rootPage = Navigation.NavigationStack.FirstOrDefault();
                    if (rootPage != null)
                    {
                        App.IsUserLoggedIn = false;
                        Navigation.InsertPageBefore(new LoginPage(), Navigation.NavigationStack.First());
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    messageLabel.Text = "Registration Failed.";
                }
            }
            else
            {
                messageLabel.Text = "Entry Validation Failed";
            }
        }

        public bool AreDetailsValid(UserRegister user, string confirmPW)
        {
            return (!string.IsNullOrWhiteSpace(user.FirstName)
                    && !string.IsNullOrWhiteSpace(user.UserName)
                    && !string.IsNullOrWhiteSpace(user.Password)
                    && !string.IsNullOrWhiteSpace(confirmPW)
                    && confirmPW.Equals(user.Password)
                    && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@")
                    && !string.IsNullOrWhiteSpace(user.LastName)
                    && !string.IsNullOrWhiteSpace(user.PhoneNumber)
                    && user.DateofBirth < DateTime.Now.Date);
        }
    }
}