using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ApproxiMATE
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty DisplayPositionProperty = BindableProperty.Create(
                propertyName: "DisplayPosition",
                returnType: typeof(Position),
                declaringType: typeof(CustomMap),
                defaultValue: new Position(30.400992, -97.723013),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: DisplayPositionPropertyChanged
            );
        private static void DisplayPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomMap)bindable;
            control.DisplayPosition = (Position)newValue;
        }

        public Position DisplayPosition { get; set; }
        public List<Position> ShapeCoordinates { get; set; }

        public CustomMap()
        {
            DisplayPosition = new Position(30.400992, -97.723013);
            ShapeCoordinates = new List<Position>();
        }

        public CustomMap(Position position)
        {
            DisplayPosition = position;
            ShapeCoordinates = new List<Position>();
        }
    }
}
