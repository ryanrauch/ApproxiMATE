using ApproxiMATE.Models;
using ApproxiMATE.Models.DataContracts;
using ApproxiMATE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMATE.Services
{
    public class FriendService
    {
        private readonly IRequestService _requestService;

        public FriendService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Obsolete]
        public async Task<ObservableCollection<FriendRequestContract>> GetAllFriends()
        {
            var all = await _requestService.GetAsync<ObservableCollection<FriendRequestContract>>("FriendRequest");
            return all;
        }

        [Obsolete("Use IRequestService inside of viewmodel")]
        public async Task<ObservableCollection<FriendRequestContract>> GetFriendRequest(string guid)
        {
            var data = await _requestService.GetAsync<ObservableCollection<FriendRequestContract>>("FriendRequest/" + guid);
            return data;
        }
    }
}
