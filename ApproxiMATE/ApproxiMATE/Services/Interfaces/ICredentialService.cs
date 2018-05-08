namespace ApproxiMATE.Services.Interfaces
{
    public interface ICredentialService
    {
        string Password { get; }
        string UserName { get; }

        void DeleteCredentials();
        void SaveCredentials(string userName, string password);
    }
}