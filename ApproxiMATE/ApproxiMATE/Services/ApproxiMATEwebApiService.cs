using ApproxiMATE.Models;
using ApproxiMATE.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMATE.Services
{
    public class ApproxiMATEwebApiService : IApproxiMATEService
    {
        public readonly HttpClient client;
        //public ApplicationUser AppUser { get; set; } //moved to App() class

        private bool _initialized { get; set; } = false;
        private string _jwtToken { get; set; }

        public ApproxiMATEwebApiService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //var result = InitializeClient(userName, password, persistent).Result;
            //if(result.StatusCode != HttpStatusCode.OK)
            //    throw new Exception("HttpClient was not initialized.");
            //_jwtToken = result.Content.ToString();
            _initialized = false;
        }
        private void AddJwtHeader()
        {
            if(!client.DefaultRequestHeaders.Contains("Authorization"))
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwtToken);
        }
        private void RemoveHeaders()
        {
            client.DefaultRequestHeaders.Clear();
        }

        public async Task<List<FriendRequestOld>> GetFriendRequestsAsync(Guid userId)
        {
            var items = new List<FriendRequestOld>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/FriendRequests/" + userId.ToString());
            items = JsonConvert.DeserializeObject<List<FriendRequestOld>>(response);
            return items;
        }

        public async Task<ApplicationUser> InitializeAppUserAsync(string userName)
        {
            var users = new List<ApplicationUser>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ApplicationUsers");
            users = JsonConvert.DeserializeObject<List<ApplicationUser>>(response);
            return users.Find(x => String.Equals(x.userName,userName, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<Boolean> RegisterUserAsync(UserRegister user)
        {
            RemoveHeaders();
            var parameters = new Dictionary<string, string>
            {
                { "UserName", user.UserName },
                { "Email", user.Email },
                { "Password", user.Password },
                { "DateofBirth", user.DateofBirth.ToString() },
                { "FirstName", user.FirstName },
                { "LastName", user.LastName },
                { "PhoneNumber", user.PhoneNumber }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(Constants.ApproxiMATEwebApiBase + "api/Registration", content);
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<UserPhoneResult>> PostUserPhoneNumberResultsAsync(UserPhoneNumbers numbers)
        {
            var items = new List<UserPhoneResult>();
            if (App.AppUser != null && !App.AppUser.id.Equals(numbers.UserId))
                return items;
            AddJwtHeader();
            var json = JsonConvert.SerializeObject(numbers);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Constants.ApproxiMATEwebApiBase + "api/UserPhoneNumbers", content);
            if(response.IsSuccessStatusCode)
            {
                var jsonresult = response.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<List<UserPhoneResult>>(jsonresult);
            }
            return items;
        }

        public async Task<HttpResponseMessage> InitializeClientAsync (string userName, string password, bool persistent)
        {
            var parameters = new Dictionary<string, string>
            {
                { "username", userName },
                { "password", password },
                { "persistent", persistent.ToString() }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(Constants.ApproxiMATEwebApiBase + "api/token", content).ConfigureAwait(false);
            if(response.IsSuccessStatusCode)
            {
                _jwtToken = await response.Content.ReadAsStringAsync();
                App.AppUser = await InitializeAppUserAsync(userName);
                _initialized = true;
            }
            return response;
        }
        public async Task<Boolean> DeleteFriendRequestAsync(FriendRequestOld data)
        {
            return await PutFriendRequestAsync(data);
        }
        public async Task<FriendLocationBox> PostFriendLocationBoxAsync(FriendLocationBoxRequest data)
        {
            var box = new FriendLocationBox();
            if (App.AppUser != null && data.UserId != App.AppUser.id)
                return box;
            AddJwtHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Constants.ApproxiMATEwebApiBase + "api/FriendLocationBox", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonresult = response.Content.ReadAsStringAsync().Result;
                box = JsonConvert.DeserializeObject<FriendLocationBox>(jsonresult);
            }
            return box;
        }

        public async Task<Boolean> PostFriendRequestAsync(FriendRequestOld data)
        {
            if (App.AppUser != null && data.InitiatorId != App.AppUser.id)
                return false;
            AddJwtHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Constants.ApproxiMATEwebApiBase + "api/FriendRequests", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        //actually used to remove/delete a friendrequest
        public async Task<Boolean> PutFriendRequestAsync(FriendRequestOld data)
        {
            if (App.AppUser != null && data.InitiatorId != App.AppUser.id)
                return false;
            AddJwtHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Constants.ApproxiMATEwebApiBase + "api/FriendRequests/" + data.InitiatorId, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task PutApplicationUserAsync(ApplicationUser data)
        {
            if (App.AppUser != null && data.id != App.AppUser.id)
                return;
            AddJwtHeader();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Constants.ApproxiMATEwebApiBase + "api/ApplicationUsers/" + data.id.ToString(), content);
            if (response.IsSuccessStatusCode)
                App.AppUser = data;
            return;
        }

        public async Task PutCurrentLocationAsync(CurrentLocation currentLocation)
        {
            if (App.AppUser != null && App.AppUser.id.ToString() != currentLocation.UserId)
                return;
            AddJwtHeader();
            var content = new StringContent(JsonConvert.SerializeObject(currentLocation), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Constants.ApproxiMATEwebApiBase + "api/CurrentLocation/" + currentLocation.UserId, content);
            if (response.IsSuccessStatusCode)
            {
                App.AppUser.currentLatitude = currentLocation.Latitude;
                App.AppUser.currentLongitude = currentLocation.Longitude;
                App.AppUser.currentTimeStamp = DateTime.Now;
            }
        }
        public async Task<List<ZoneRegion>> GetZoneRegionsAsync()
        {
            var items = new List<ZoneRegion>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ZoneRegions");
            items = JsonConvert.DeserializeObject<List<ZoneRegion>>(response);
            return items;
        }

        public async Task<List<ZoneRegionPolygon>> GetZoneRegionPolygonsAsync(int regionId)
        {
            var items = new List<ZoneRegionPolygon>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ZoneRegionPolygons/" + regionId.ToString());
            items = JsonConvert.DeserializeObject<List<ZoneRegionPolygon>>(response);
            return items;
        }
        public async Task<ZoneRegion> GetZoneRegionAsync(int regionId)
        {
            var item = new ZoneRegion();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ZoneRegions/" + regionId.ToString());
            item = JsonConvert.DeserializeObject<ZoneRegion>(response);
            return item;
        }

        public async Task<List<ZoneState>> GetZoneStatesAsync()
        {
            var items = new List<ZoneState>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ZoneStates/");
            items = JsonConvert.DeserializeObject<List<ZoneState>>(response);
            return items;
        }

        public async Task<List<ApplicationOption>> GetApplicationOptionsAsync()
        {
            var items = new List<ApplicationOption>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ApplicationOptions/");
            items = JsonConvert.DeserializeObject<List<ApplicationOption>>(response);
            return items;
        }
        /*
        public async Task<List<Region>> GetRegionsAsync()
        {
            var items = new List<Region>();
            var uri = new Uri(Constants.WebAPIURL + "Regions");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<List<Region>>(content);
            }
            return items;
        }*/
    }
}
