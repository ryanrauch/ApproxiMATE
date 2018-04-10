using ApproxiMATE.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApproxiMATE.Services
{
    public interface IApproxiMATEService
    {
        //ApplicationUser AppUser;

        Task<HttpResponseMessage> InitializeClientAsync(string userName, string password, bool persistent);
        Task<ApplicationUser> InitializeAppUserAsync(string userName);

        Task PutApplicationUserAsync(ApplicationUser data);


        Task<List<ZoneState>> GetZoneStatesAsync();
        Task<List<ApplicationOption>> GetApplicationOptionsAsync();
    }
}