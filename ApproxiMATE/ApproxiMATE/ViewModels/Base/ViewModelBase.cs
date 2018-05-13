using ApproxiMATE.Helpers;
using ApproxiMATE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMATE.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        //protected readonly IDialogService DialogService;
        //protected readonly INavigationService NavigationService;
        //protected readonly IHexagonal hexagonal;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase()
        {
            //DialogService = ViewModelLocator.Resolve<IDialogService>();
            //NavigationService = ViewModelLocator.Resolve<INavigationService>();
            //hexagonal = ViewModelLocator.Resolve<IHexagonal>();
            //GlobalSetting.Instance.BaseEndpoint = ViewModelLocator.Resolve<ISettingsService>().UrlBase;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        //protected virtual void CurrentPageOnAppearing(object sender, EventArgs eventArgs) { }

        //protected virtual void CurrentPageOnDisappearing(object sender, EventArgs eventArgs) { }
    }
}
