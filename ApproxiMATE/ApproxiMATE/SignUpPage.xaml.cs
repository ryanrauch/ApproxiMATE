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
            var user = new User()
            {
                Password = EntryPassword.Text,
                Email = EntryEmail.Text,
                PhoneNumber = EntryPhone.Text,
                DateofBirth = DatePickerDOB.Date,
                FirstName = EntryFirstName.Text,
                LastName = EntryLastName.Text
            };

            // Sign up logic goes here
            var signUpSucceeded = AreDetailsValid(user);
            if (signUpSucceeded)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = "Sign up failed";
            }
        }

        public bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.FirstName) 
                    && !string.IsNullOrWhiteSpace(user.Password) 
                    && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@")
                    && !string.IsNullOrWhiteSpace(user.LastName)
                    && !string.IsNullOrWhiteSpace(user.PhoneNumber)
                    && user.DateofBirth < DateTime.Now.Date);
        }
    }
}