using ApproxiMATE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Services
{
    public class FriendService
    {
        private readonly IRequestService _requestService;

        public FriendService(IRequestService requestService)
        {
            _requestService = requestService;
        }
    }
}
