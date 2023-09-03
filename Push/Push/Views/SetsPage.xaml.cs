using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Push.Views
{	
	public partial class SetsPage : ContentPage
	{	
		public SetsPage ()
		{
			InitializeComponent ();
		}

        async void Back(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("..");

        }
    }
}

