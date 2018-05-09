using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class FriendRequestInfo
    {
        public String InitiatorId { get; set; }
        public String TargetId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
