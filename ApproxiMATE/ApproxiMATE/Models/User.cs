using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Visibility { get; set; }
        public Zone CurrentZone { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
