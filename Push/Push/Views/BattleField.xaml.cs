using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Push.Views
{	
	public partial class BattleField : ContentPage
	{	
		public BattleField ()
		{
			InitializeComponent ();
		}

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

