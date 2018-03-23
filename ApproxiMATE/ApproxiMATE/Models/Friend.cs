using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public User UserId { get; set; }
        public User SecondUserId { get; set; }
        public int Status { get; set; }
        public DateTime LastActivity { get; set; }
    }
}