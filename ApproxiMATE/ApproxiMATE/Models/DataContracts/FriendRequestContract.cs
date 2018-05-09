using ApproxiMATE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models.DataContracts
{
    public class FriendRequestContract
    {
        public String InitiatorId { get; set; }
        public String TargetId { get; set; }
        public DateTime TimeStamp { get; set; }
        public FriendRequestType? Type { get; set; }
    }
}
