using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    //TODO: maintain a single page between both applications
    //      DO NOT CHANGE THIS FILE WITHOUT CHANGING ApproxiMATEwebApi.Helpers.CoordinateFilter.cs also
    public static class CoordinateFunctions
    {
        public static readonly double BOXWIDTH = 0.01;
        public static double LatitudeBound(double input)
        {
            return Math.Floor(input * 100) / 100;
        }

        public static double LongitudeBound(double input)
        {
            return LatitudeBound(input);
        }
        public static Position GetCenterPositionFromBox(string box)
        {
            return new Position(GetLatitudeFloorFromBox(box) + BOXWIDTH/2, GetLongitudeFloorFromBox(box) + BOXWIDTH/2);
        }
        public static string GetBoundingBox(Position position)
        {
            return GetBoundingBox(position.Latitude, position.Longitude);
        }

        public static string GetBoundingBox(double latitude, double longitude)
        {
            return string.Format("{0}{1}{2}", LatitudeBound(latitude),
                                              Constants.BoundingBoxDelim,
                                              LongitudeBound(longitude));
        }
        public static string GetBoundingBoxNearby(string box, int stepsLatitude, int stepsLongitude)
        {
            double latitude = GetLatitudeFloorFromBox(box) + (stepsLatitude * 0.01d);
            double longitude = GetLongitudeFloorFromBox(box) + (stepsLongitude * 0.01d);
            return GetBoundingBox(latitude,longitude);
        }
        //Latitude x Longitude
        //bounding box example: "30.40x-97.72" [string] [x is defined in constants]
        //bounding box always contains floor values
        public static double GetLatitudeFloorFromBox(string box)
        {
            if (box.Contains(Constants.BoundingBoxDelim.ToString()))
                return Double.Parse(box.Split(Constants.BoundingBoxDelim)[0]);
            return 0;
        }
        public static double GetLatitudeCeilingFromBox(string box)
        {
            return GetLatitudeFloorFromBox(box) + 0.01;
        }
        public static double GetLongitudeFloorFromBox(string box)
        {
            if (box.Contains(Constants.BoundingBoxDelim.ToString()))
                return Double.Parse(box.Split(Constants.BoundingBoxDelim)[1]);
            return 0;
        }
        public static double GetLongitudeCeilingFromBox(string box)
        {
            return GetLongitudeFloorFromBox(box) + 0.01;
        }
    }
}
