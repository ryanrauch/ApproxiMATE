using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApproxiMATE.iOS;
using ApproxiMATE.Services;
using ApproxiMATE.Services.Interfaces;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]
namespace ApproxiMATE.iOS
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public String Version => NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
    }
}