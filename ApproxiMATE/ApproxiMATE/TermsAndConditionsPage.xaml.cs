using ApproxiMATE.Models;
using ApproxiMATE.Services;
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
	public partial class TermsAndConditionsPage : ContentPage
	{
        private WebView _webView { get; set; }
		public TermsAndConditionsPage ()
		{
			InitializeComponent ();
            BindingContext = this;
            //_webView = new WebView
            //{
            //    Source = "https://baconipsum.com/?paras=5&type=all-meat&start-with-lorem=1",
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand
            //};
            //Content = new StackLayout
            //{
            //    Children = { _webView }
            //};
        }
        private String _eulaSource { get; set; } = String.Empty;
        public String EulaSource
        {
            get { return _eulaSource; }
            set { _eulaSource = value; OnPropertyChanged("EulaSource"); }
        }

        //private ApplicationOption _appOption { get; set; } = new ApplicationOption();
        //public ApplicationOption AppOption
        //{
        //    get { return _appOption; }
        //    set { _appOption = value; OnPropertyChanged("AppOption"); }
        //}
        protected override async void OnAppearing()
        {
            var options = await App.approxiMATEService.GetApplicationOptionsAsync();
            var current = options.OrderByDescending(x => x.OptionsDate).FirstOrDefault();
            EulaSource = current.EndUserLicenseAgreementSource;
            base.OnAppearing();
        }

        public void Switch_OnToggled(Object sender, ToggledEventArgs e)
        {
            ButtonAccept.IsEnabled = SwitchEULA.IsToggled && SwitchTerms.IsToggled;
        }

        public async void ButtonAccept_OnClicked(Object sender, EventArgs e)
        {
            App.AppUser.termsAndConditionsDate = DateTime.Now;
            await App.approxiMATEService.PutApplicationUserAsync(App.AppUser);
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync();
        }
    }
}