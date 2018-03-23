using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public Region RegionId { get; set; }
        public int ZoneType { get; set; }
        public string Name { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
    }
}