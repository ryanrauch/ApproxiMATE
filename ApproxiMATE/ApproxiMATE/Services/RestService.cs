using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApproxiMATE.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }
    }
}
