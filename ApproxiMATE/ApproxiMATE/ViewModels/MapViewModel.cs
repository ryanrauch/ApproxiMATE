using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace ApproxiMATE
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel()
        {
            UpdatePolygons();
            UpdateLocation();
            //PolygonCollection.CollectionChanged += PolygonCollection_CollectionChanged;
        }

        //private void PolygonCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    OnPropertyChanged("PolygonCollection");
        //}

        public async void UpdateLocation()
        { 
            var position = await Utilities.GetCurrentGeolocationAsync();
            MyPosition = new Position(position.Latitude, position.Longitude);
            PinCollection.Add(new Pin()
            {
                Position = MyPosition,
                Label = "Ryan",
                Type = PinType.Generic
            });
        }
        public void UpdatePolygons()
        {
            //ObservableCollection<Position> rockrose = new ObservableCollection<Position>();
            PolygonCollection.Add(new Position(30.39983, -97.723719));
            PolygonCollection.Add(new Position(30.40182, -97.722989));
            PolygonCollection.Add(new Position(30.402172, -97.724245));
            PolygonCollection.Add(new Position(30.403236, -97.72374));
            PolygonCollection.Add(new Position(30.402606, -97.721659));
            PolygonCollection.Add(new Position(30.399562, -97.723011));

            //TODO: look into xamarin and mvvmlight
            //also look into collectionchanged event??


            //OnPropertyChanged("PolygonCollection");
            //PolygonCollection = rockrose;
        }

        private ObservableCollection<Position> _polygonCollection { get; set; } = new ObservableCollection<Position>();
        public ObservableCollection<Position> PolygonCollection
        {
            get { return _polygonCollection; }
            set { _polygonCollection = value; OnPropertyChanged("PolygonCollection"); }
        }

        private Position _myPosition = new Position(30.400992, -97.723013);
        public Position MyPosition
        {
            get { return _myPosition; }
            set { _myPosition = value; OnPropertyChanged("MyPosition"); }
        }

        private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> PinCollection
        {
            get { return _pinCollection; }
            set { _pinCollection = value; OnPropertyChanged("PinCollection"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
