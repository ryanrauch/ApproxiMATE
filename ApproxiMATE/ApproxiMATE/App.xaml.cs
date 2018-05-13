using ApproxiMATE.Helpers;
using ApproxiMATE.Models;
using ApproxiMATE.Services;
using ApproxiMATE.Services.Interfaces;
using ApproxiMATE.ViewModels.Base;
using ApproxiMATE.Views;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ApproxiMATE
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static ApplicationUser AppUser { get; set; }
        public static ApplicationOption AppOptions { get; set; }
	    public static double ScreenHeight { get; set; }
	    public static double ScreenWidth { get; set; }
        public static ApproxiMATEwebApiService approxiMATEService { get; set; }      //TODO: convert back to interface
        public static ICredentialService CredentialService { get; set; }    //TODO: convert to interface
        public static IHexagonal Hexagonal { get; set; }
        public static IHeatGradient HeatGradient { get; set; }

        public static IRequestService RequestService { get; set; }

		public App ()
		{
			InitializeComponent();


            //var navigationService = ViewModelLocator.Resolve<INavigationService>();
            //navigationService.InitializeAsync().ConfigureAwait(true);

            InitApp();
            MainPage = new NavigationPage(new MainMapView());
            return;
            //MainPage = new NavigationPage(new StartupPage());
            if (!IsUserLoggedIn)
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new NavigationPage(new StartupPage());

            //MainPage = new ApproxiMATE.BindableMapPage();
            //MainPage = new ApproxiMATE.MainPage();
            //MainPage = new ApproxiMATE.MapPage();
        }

        private void InitApp()
        {
            RequestService = new JwtRequestService();
            approxiMATEService = new ApproxiMATEwebApiService();
            CredentialService = new AccountServiceXamarinAuth();
            //Hexagonal = new HexagonalEquilateralScale();
            //Hexagonal = ViewModelLocator.Resolve<IHexagonal>();
        }

        protected override async void OnStart ()
		{
            // Handle when your app starts
            base.OnStart();
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
        
        //public static void InitializeAutofac()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<HexagonalEquilateralScale>().As<IHexagonal>();
        //    builder.RegisterType<HeatGradient>().As<IHeatGradient>().SingleInstance();
        //    Container = builder.Build();
        //}

        /*
        private async Task RequestLocationPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                //{
                //    //await DisplayAlert("Need location", "Gunna need that location", "OK");
                //}

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }

            if (status == PermissionStatus.Granted)
            {
            }
            else if (status != PermissionStatus.Unknown)
            {
                //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
            }
        }
        */
    }
}
