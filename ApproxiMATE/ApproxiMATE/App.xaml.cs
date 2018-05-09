﻿using ApproxiMATE.Helpers;
using ApproxiMATE.Models;
using ApproxiMATE.Services;
using ApproxiMATE.Services.Interfaces;
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
            //InitializeAutofac();
            RequestService = new JwtRequestService();
            Hexagonal = new HexagonalEquilateralScale();
            approxiMATEService = new ApproxiMATEwebApiService();
            CredentialService = new AccountServiceXamarinAuth();

            //MainPage = new NavigationPage(new StartupPage());
            if (!IsUserLoggedIn)
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new NavigationPage(new StartupPage());

            //MainPage = new ApproxiMATE.BindableMapPage();
            //MainPage = new ApproxiMATE.MainPage();
            //MainPage = new ApproxiMATE.MapPage();
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
            //var position = await LocationServices.GetGeolocation();
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            /*

            await RequestLocationPermission();
            var position = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
            if (MainPage is BindableMapPage)
            {
                ((BindableMapPage)MainPage).MyPosition = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                ((BindableMapPage)MainPage).PinCollection.Add(
                    new Xamarin.Forms.Maps.Pin()
                    {
                        Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude),
                        Type = Xamarin.Forms.Maps.PinType.Generic,
                        Label = "Current Loc"
                    });
            }

            */
            //});
            //ApproxiMATE.MapPage map = new MapPage();
            //map.SetDisplayRegion(position, Xamarin.Forms.Maps.Distance.FromMiles(0.1));
            //MainPage = map;
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
