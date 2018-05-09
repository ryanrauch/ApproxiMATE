using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ApproxiMATE.Droid;
using ApproxiMATE.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]
namespace ApproxiMATE.Droid
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string Version
        {
            get
            {
                var context = Android.App.Application.Context;
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);

                return $"{info.VersionName}.{info.VersionCode.ToString()}";
            }
        }
    }
}