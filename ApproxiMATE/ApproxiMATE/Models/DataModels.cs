using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
    }
    public class Zone
    {
        public int Id { get; set; }
        public Region RegionId { get; set; }
        public int ZoneType { get; set; }
        public string Name { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
    }
    public class ZoneCoordinate
    {
        public int Id { get; set; }
        public Zone ZoneId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class Friend
    {
        public int Id { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
        public int Status { get; set; }
        public DateTime LastActivity { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int VisibilitySetting { get; set; }
        public Zone CurrentZone { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
