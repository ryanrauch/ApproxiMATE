using ApproxiMATE.Models;
using ApproxiMATE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ApproxiMATE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage ()
		{
			InitializeComponent ();
		    /*
		    var map = new Map(MapSpan.FromCenterAndRadius(new Position(30.387069, -97.703656), Distance.FromMiles(1)))
		    {
		        IsShowingUser = true,
		        HeightRequest = 100,
		        WidthRequest = 960,
		        VerticalOptions = LayoutOptions.FillAndExpand
		    };
		    var stack = new StackLayout {Spacing = 0};
		    stack.Children.Add(map);
		    Content = stack;
            */
		    var customMap = new CustomMap
		    {
		        MapType = MapType.Street,
		        WidthRequest = App.ScreenWidth,
		        HeightRequest = App.ScreenHeight
		    };

            // This outlines Rockrose st (austin,tx)
            customMap.ShapeCoordinates.Add(new Position(30.39983, -97.723719));
            customMap.ShapeCoordinates.Add(new Position(30.40182, -97.722989));
            customMap.ShapeCoordinates.Add(new Position(30.402172, -97.724245));
            customMap.ShapeCoordinates.Add(new Position(30.403236, -97.72374));
            customMap.ShapeCoordinates.Add(new Position(30.402606, -97.721659));
            customMap.ShapeCoordinates.Add(new Position(30.399562, -97.723011));
            // Center over Dogwood on Rockrose
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(30.400992, -97.723013), Distance.FromMiles(0.1)));

            Content = customMap;
            RestService service = new RestService();
            List<Region> Regions = service.GetRegionsAsync().Result;

        }
	}
}