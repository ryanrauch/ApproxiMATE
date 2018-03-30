using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ApproxiMATE.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            //TODO: this does not seem to be working??
            //https://www.bingmapsportal.com
            Xamarin.FormsMaps.Init("AmTIxyUr5Xz_peUhdPSNr-Y_xRStWkb2LE0Bosx-TsVZbYLYbWOJJET_KvjRLwEU");

            LoadApplication(new ApproxiMATE.App());
        }
    }
}
