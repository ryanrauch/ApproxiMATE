using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class ZoneState
    {
        public int StateId { get; set; }
        public String Description { get; set; }         //"Texas"
        public String ShortDescription { get; set; }    //"TX"
    }

    public class ZoneCity
    {
        public int CityId { get; set; }
        public String Description { get; set; }
        public virtual ZoneState State { get; set; }
    }
}
