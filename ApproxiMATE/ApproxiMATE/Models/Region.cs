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
}
