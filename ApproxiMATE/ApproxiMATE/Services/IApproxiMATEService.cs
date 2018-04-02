using ApproxiMATE.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApproxiMATE.Services
{
    public interface IApproxiMATEService
    {
        Task<List<ZoneState>> GetZoneStatesAsync();
        Task<HttpResponseMessage> InitializeClientAsync(string userName, string password, bool persistent);
    }
}