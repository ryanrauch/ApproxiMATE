using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ApproxiMATE
{
	public partial class App : Application
	{
	    public static double ScreenHeight;
	    public static double ScreenWidth;

		public App ()
		{
			InitializeComponent();

			//MainPage = new ApproxiMATE.MainPage();
            MainPage = new ApproxiMATE.MapPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
