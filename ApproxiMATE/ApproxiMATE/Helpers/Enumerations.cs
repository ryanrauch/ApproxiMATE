using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE
{
    public enum FriendStatus
    {
        NotRegistered = 0,
        Initiated = 1,
        Mutual = 2,
        PendingRequest = 3,
        Available = 4,
        Blocked = 5
    };

    public enum FriendRequestType
    {
        Normal = 0,
        Blocked = 1
    };
}
