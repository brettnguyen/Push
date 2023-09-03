using System;
using System.Collections.Generic;
using Push.ViewModels;
using Xamarin.Forms;

namespace Push.Views
{	
	public partial class Home : ContentPage
	{
       
        public Home ()
		{
			InitializeComponent ();
            BindingContext = App.homeViewModel;
        }

        public async void ChangeDiffculty(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("Reps");
        }

        public async void Fight(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("BattleField");
        }

        public async void Train(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("Sets");
        }
    }
}

