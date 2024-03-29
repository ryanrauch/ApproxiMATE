﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ApproxiMATE
{
    public class BindableMap : Map
    {
        //////
        private static void ZoomLevelPropertyChanged(BindableObject b, object o, object n)
        {
            if (((BindableMap)b).VisibleRegion != null)
            {
                ((BindableMap)b).ZoomLevel = ((BindableMap)b).VisibleRegion.Radius;
            }
            else
            {
                ((BindableMap)b).ZoomLevel = Distance.FromKilometers(0);
            }
        }

        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create(
                 nameof(ZoomLevel),
                 typeof(Distance),
                 typeof(BindableMap),
                 new Distance(),
                 propertyChanged: ZoomLevelPropertyChanged);

        public Distance ZoomLevel
        {
            get { return (Distance)base.GetValue(ZoomLevelProperty); }
            set { base.SetValue(ZoomLevelProperty, value); }
        }
        //////
        
        private static void MapPinsPropertyChanged(BindableObject b, object o, object n)
        {
            var bindable = (BindableMap)b;
            bindable.CustomPins.Clear();

            var collection = (ObservableCollection<CustomPin>)n;
            foreach (var item in collection)
                bindable.CustomPins.Add(item);
            collection.CollectionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                        case NotifyCollectionChangedAction.Replace:
                        case NotifyCollectionChangedAction.Remove:
                            if (e.OldItems != null)
                                foreach (var item in e.OldItems)
                                    bindable.CustomPins.Remove((CustomPin)item);
                            if (e.NewItems != null)
                                foreach (var item in e.NewItems)
                                    bindable.CustomPins.Add((CustomPin)item);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            bindable.CustomPins.Clear();
                            break;
                    }
                });
            };
        }

        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
                 nameof(CustomPins),
                 typeof(ObservableCollection<CustomPin>),
                 typeof(BindableMap),
                 new ObservableCollection<CustomPin>(),
                 BindingMode.TwoWay,
                 propertyChanged: MapPinsPropertyChanged);

        public IList<CustomPin> CustomPins
        {
            get { return (IList<CustomPin>)base.GetValue(MapPinsProperty); }
            set { base.SetValue(MapPinsProperty, value); }
        }

        private static void PolygonCoordinatesPropertyChanged(BindableObject b, object o, object n)
        {
            var bindable = (BindableMap)b;
            bindable.PolygonCoordinates.Clear();

            var collection = (ObservableCollection<Position>)n;
            foreach (var item in collection)
                bindable.PolygonCoordinates.Add(item);
            collection.CollectionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                        case NotifyCollectionChangedAction.Replace:
                        case NotifyCollectionChangedAction.Remove:
                            if (e.OldItems != null)
                                foreach (var item in e.OldItems)
                                    bindable.PolygonCoordinates.Remove((Position)item);
                            if (e.NewItems != null)
                                foreach (var item in e.NewItems)
                                    bindable.PolygonCoordinates.Add((Position)item);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            bindable.PolygonCoordinates.Clear();
                            break;
                    }
                });
            };
        }

        public static readonly BindableProperty PolygonCoordinatesProperty = BindableProperty.Create(
                nameof(PolygonCoordinates),
                typeof(ObservableCollection<Position>),
                typeof(BindableMap),
                new ObservableCollection<Position>(),
                BindingMode.TwoWay,
                propertyChanged: PolygonCoordinatesPropertyChanged);

        public IList<Position> PolygonCoordinates
        {
            get { return (IList<Position>)base.GetValue(PolygonCoordinatesProperty); }
            set { base.SetValue(PolygonCoordinatesProperty, value); }
        }

        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(
                 nameof(MapPosition),
                 typeof(Position),
                 typeof(BindableMap),
                 new Position(30.400992, -97.723013),
                 BindingMode.TwoWay,
                 propertyChanged: MapPositionPropertyChanged);

        private static void MapPositionPropertyChanged(BindableObject b, object o, object n)
        {
            ((BindableMap)b).MoveToRegion(MapSpan.FromCenterAndRadius(
                          (Position)n,
                          Distance.FromKilometers(0.75)));
        }
        public Position MapPosition
        {
            get { return (Position)base.GetValue(MapPositionProperty); }
            set { base.SetValue(MapPositionProperty, value); }
        }
    }
}
