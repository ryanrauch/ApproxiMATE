using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApproxiMATE.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //public INavigation Navigation { get; set; }
        public Func<BaseViewModel, Task> OnNavigationRequest { get; set; }
        public async Task NavigateToAsync<TViewModel>(TViewModel targetViewModel) where TViewModel : BaseViewModel
        {
            await OnNavigationRequest?.Invoke(targetViewModel);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public BaseViewModel()
        //{
        //}
    }
}
