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
        public CustomMap MapView { get; set; }
		public MapPage ()
		{
			InitializeComponent ();

		    MapView = new CustomMap
		    {
		        MapType = MapType.Street,
		        WidthRequest = App.ScreenWidth,
		        HeightRequest = App.ScreenHeight
		    };

            // This outlines Rockrose st (austin,tx)
            MapView.ShapeCoordinates.Add(new Position(30.39983, -97.723719));
            MapView.ShapeCoordinates.Add(new Position(30.40182, -97.722989));
            MapView.ShapeCoordinates.Add(new Position(30.402172, -97.724245));
            MapView.ShapeCoordinates.Add(new Position(30.403236, -97.72374));
            MapView.ShapeCoordinates.Add(new Position(30.402606, -97.721659));
            MapView.ShapeCoordinates.Add(new Position(30.399562, -97.723011));

            //MapView.MoveToRegion(MapSpan.FromCenterAndRadius(MapView.DisplayPosition, Distance.FromMiles(0.1)));
            SetDisplayRegion(MapView.DisplayPosition, Distance.FromMiles(0.1));
            Content = MapView;

            //RestService service = new RestService();
            //List<Region> Regions = service.GetRegionsAsync().Result;
        }
        public void SetDisplayRegion(Position pos, Distance dist)
        {
            if (MapView == null)
                return;
            MapView.MoveToRegion(MapSpan.FromCenterAndRadius(pos, dist));
            InvalidateMeasure();
        }
	}
}