using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class ZoneCoordinate
    {
        public int Id { get; set; }
        public Zone ZoneId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
