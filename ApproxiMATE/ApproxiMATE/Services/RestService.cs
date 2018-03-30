using ApproxiMATE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMATE.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
            var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

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
        }
    }
}
