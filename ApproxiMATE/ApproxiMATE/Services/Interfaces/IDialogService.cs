using System.Threading.Tasks;

namespace ApproxiMATE.Services.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
