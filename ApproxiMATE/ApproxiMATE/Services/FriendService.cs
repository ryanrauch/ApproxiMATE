using ApproxiMATE.Models;
using ApproxiMATE.Models.DataContracts;
using ApproxiMATE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMATE.Services
{
    public class FriendService
    {
        private readonly IRequestService _requestService;

        public IList<FriendRequestInfo> EntireCollection { get; }
        public IList<FriendRequestInfo> Mutual { get; }
        public IList<FriendRequestInfo> IncomingPending { get; }
        public IList<FriendRequestInfo> OutgoingPending { get; }
        public IList<FriendRequestInfo> Blocked { get; }

        public FriendService(IRequestService requestService)
        {
            _requestService = requestService;
        }
        //TODO: this should go inside of a ViewModel
        //      that way can bind to each individual list
        //      and call .refresh() when necessary
        public async Task PopulateLists()
        {
            var all = await _requestService.GetAsync<FriendRequestContract>("FriendRequest");
        }

    }
}
