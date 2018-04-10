using ApproxiMATE.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ApproxiMATE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BindableMapPage : ContentPage
	{
        public BindableMapPage()
        {
            InitializeComponent();
            //BindingContext = new MapViewModel();  //MVVM probably needs a framework to access the view from model?
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var position = await Utilities.GetCurrentGeolocationAsync();
                MyPosition = new Position(position.Latitude, position.Longitude);
                PinCollection.Add(new CustomPin()
                {
                    Id = "Ryan",
                    Position = MyPosition,
                    Label = "Ryan",
                    Type = PinType.Generic,
                    Url = "http://www.ryanrauch.com/"
                });

                PolygonCollection.Add(new Position(30.39983, -97.723719));
                PolygonCollection.Add(new Position(30.40182, -97.722989));
                PolygonCollection.Add(new Position(30.402172, -97.724245));
                PolygonCollection.Add(new Position(30.403236, -97.72374));
                PolygonCollection.Add(new Position(30.402606, -97.721659));
                PolygonCollection.Add(new Position(30.399562, -97.723011));

                //UpdateMapGrid();
                //UpdateLocation();
                //UpdatePolygons();
                //MapContent.PropertyChanged += MapContent_PropertyChanged; //this has not worked yet, but used to change grids when zoomed by user
                //UpdateRestService();
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "OK");
            }
        }

        /*private void MapContent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = (Map)sender;
            if (e.PropertyName == "VisibleRegion") //look into this?
            {
                if (m.VisibleRegion != null)
                {
                    //
                    int i = 0;
                    //re-draw map here??
                }
            }
        }
        */
        private Distance _myZoomLevel { get; set; } = new Distance();
        public Distance MyZoomLevel
        {
            get { return _myZoomLevel; }
            set { _myZoomLevel = value; OnPropertyChanged("MyZoomLevel"); }
        }

        private ObservableCollection<Position> _polygonCollection { get; set; } = new ObservableCollection<Position>();
        public ObservableCollection<Position> PolygonCollection
        {
            get { return _polygonCollection; }
            set { _polygonCollection = value; OnPropertyChanged("PolygonCollection"); }
        }

        private Position _myPosition { get; set; } = new Position(30.400992, -97.723013);
        public Position MyPosition
        {
            get { return _myPosition; }
            set { _myPosition = value; OnPropertyChanged("MyPosition"); }
        }

        private ObservableCollection<Pin> _pinCollection { get; set; } = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> PinCollection
        {
            get { return _pinCollection; }
            set { _pinCollection = value; OnPropertyChanged("PinCollection"); }
        }

        /*public async void UpdateMapGrid()
        {
            //this replaces UpdateLocation() and UpdatePolygons()
            var position = await Utilities.GetCurrentGeolocationAsync();
            MyPosition = new Position(position.Latitude, position.Longitude);
            PinCollection.Add(new CustomPin()
            {
                Position = MyPosition,
                Label = "Ryan",
                Type = PinType.Generic
            });

            //double x = Math.Floor(MyPosition.Latitude * 100) / 100;
            //double y = Math.Floor(MyPosition.Longitude * 100) / 100;

            double x = position.Latitude;    //maybe Math.Floor isn't working?
            double y = position.Longitude;

            //x = 30.400992;  // still nothing??
            //y = -97.723013;

            PolygonCollection.Add(new Position(x, y));
            PolygonCollection.Add(new Position(x + 0.01, y));
            PolygonCollection.Add(new Position(x + 0.01, y + 0.01));
            PolygonCollection.Add(new Position(x, y + 0.01));
            //await DisplayAlert("MyPosition", x.ToString() + " " + y.ToString(), "OK");
        }*/

        public async void UpdateLocation()
        {
            var position = await Utilities.GetCurrentGeolocationAsync();
            MyPosition = new Position(position.Latitude, position.Longitude);
            PinCollection.Add(new CustomPin()
            {
                Id = "Ryan",
                Position = MyPosition,
                Label = "Ryan",
                Type = PinType.Generic,
                Url = "http://www.ryanrauch.com/"
            });
        }

        /*public async void UpdateRestService()
        {
            try
            {
                RestService svc = new RestService();
                var regions = await svc.GetRegionsAsync();
                await DisplayAlert(regions.Count.ToString(), "regions count", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "OK");
            }
        }*/

        public void UpdatePolygons()
        {
            //ObservableCollection<Position> rockrose = new ObservableCollection<Position>();
            PolygonCollection.Add(new Position(30.39983, -97.723719));
            PolygonCollection.Add(new Position(30.40182, -97.722989));
            PolygonCollection.Add(new Position(30.402172, -97.724245));
            PolygonCollection.Add(new Position(30.403236, -97.72374));
            PolygonCollection.Add(new Position(30.402606, -97.721659));
            PolygonCollection.Add(new Position(30.399562, -97.723011));
            //6 original coordinates
            
            /*
            // Intersection removing an interior polygon
            PolygonCollection.Add(new Position(30.401807, -97.722484));
            PolygonCollection.Add(new Position(30.402455, -97.722248));
            PolygonCollection.Add(new Position(30.402561, -97.722623));
            PolygonCollection.Add(new Position(30.402302, -97.722784));
            */
            //TODO: look into xamarin and mvvmlight
            //also look into collectionchanged event??


            //OnPropertyChanged("PolygonCollection");
            //PolygonCollection = rockrose;
        }
        /*public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/

        /*private void AddMapTypeSelector()
        {
            int typesWidth = 260, typesHeight = 30, distanceFromBottom = 60;
            mapTypes = new UISegmentedControl(new CGRect((View.Bounds.Width - typesWidth) / 2, View.Bounds.Height - distanceFromBottom, typesWidth, typesHeight));
            mapTypes.BackgroundColor = UIColor.White;
            mapTypes.Layer.CornerRadius = 5;
            mapTypes.ClipsToBounds = true;
            mapTypes.InsertSegment("Road", 0, false);
            mapTypes.InsertSegment("Satellite", 1, false);
            mapTypes.InsertSegment("Hybrid", 2, false);
            mapTypes.SelectedSegment = 0; // Road is the default
            mapTypes.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
            mapTypes.ValueChanged += (s, e) => {
                switch (mapTypes.SelectedSegment)
                {
                    case 0:
                        mapView.MapType = MKMapType.Standard;
                        break;
                    case 1:
                        mapView.MapType = MKMapType.Satellite;
                        break;
                    case 2:
                        mapView.MapType = MKMapType.Hybrid;
                        break;
                }
            };
        }*/
    }
}