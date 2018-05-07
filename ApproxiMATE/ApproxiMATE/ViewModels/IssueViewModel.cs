using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace ApproxiMATE.ViewModels
{
    public class IssueViewModel : BaseViewModel
    {
        private String _issue { get; set; }
        public String Issue
        {
            get { return _issue; }
            set
            {
                _issue = value;
                OnPropertyChanged("Issue");
            }
        }
        public IssueViewModel(/*INavigation navigation,*/ String issue)
        {
            //Navigation = navigation;
            Issue = issue;
        }
    }
}
