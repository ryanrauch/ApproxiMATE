using ApproxiMATE.Models;
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

        public async Task<ApplicationUser> InitializeAppUserAsync(string userName)
        {
            var users = new List<ApplicationUser>();
            AddJwtHeader();
            var response = await client.GetStringAsync(Constants.ApproxiMATEwebApiBase + "api/ApplicationUsers");
            users = JsonConvert.DeserializeObject<List<ApplicationUser>>(response);
            return users.Find(x => String.Equals(x.userName,userName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<HttpResponseMessage> InitializeClientAsync (string userName, string password, bool persistent)
        {
            var parameters = new Dictionary<string, string>
            {
                { "username", userName },
                { "password", password },
                { "isPersistent", persistent.ToString() }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(Constants.ApproxiMATEwebApiBase + "token", content).ConfigureAwait(false);
            if(response.IsSuccessStatusCode)
            {
                _jwtToken = await response.Content.ReadAsStringAsync();
                App.AppUser = await InitializeAppUserAsync(userName);
                _initialized = true;
            }
            return response;
        }

        public async Task PutApplicationUserAsync(ApplicationUser data)
        {
            if (App.AppUser != null && data.id != App.AppUser.id)
                return;
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Constants.ApproxiMATEwebApiBase + "api/ApplicationUsers/" + data.id.ToString(), content);
            if (response.IsSuccessStatusCode)
                App.AppUser = data;
            return;
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
