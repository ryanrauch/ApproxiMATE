﻿using ApproxiMATE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMATE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IssuePage : ContentPage
	{
        public IssuePage(String issue)
        {
            InitializeComponent();
            BindingContext = new IssueViewModel(/*Navigation,*/ issue);
        }
	}
}