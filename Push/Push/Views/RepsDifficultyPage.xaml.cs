using System;
using System.Collections.Generic;
using Push.ViewModels;
using Push.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;

namespace Push.Views
{	
	public partial class RepsDifficultyPage : ContentPage
	{
        private readonly RepsDifficultyViewModel _RepsDifficultyViewModel;
        public RepsDifficultyPage ()
		{
			InitializeComponent ();
            _RepsDifficultyViewModel = new RepsDifficultyViewModel();
            BindingContext = _RepsDifficultyViewModel;
            foreach (var difficulty in _RepsDifficultyViewModel.Diffculties)
            {
                if (Preferences.Get($"{difficulty.Diffculty}IsChosen", false))
                {
                    _RepsDifficultyViewModel.SelectedDifficulty = difficulty;
                    break;
                }
            }
        }

        void Choose(System.Object sender, System.EventArgs e)
        {
            _RepsDifficultyViewModel.ChoseDifficulty();
          
        }

        void ExitPopup(System.Object sender, System.EventArgs e)
        {

            _RepsDifficultyViewModel.Exit();
           
        }

        void OpenPopup(System.Object sender, System.EventArgs e)
        {

            var selectedItem = (sender as StackLayout)?.BindingContext as RepsDifficultyModel;
            if (selectedItem != null)
            {
                _RepsDifficultyViewModel.SelectedDifficulty = selectedItem;
                _RepsDifficultyViewModel.ShowPopUp();
            }
        }



        async void Back(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("..");

            string selectedDifficulty = null;

            foreach (var difficulty in _RepsDifficultyViewModel.Diffculties)
            {
                if (difficulty.IsChosen)
                {
                    selectedDifficulty = difficulty.Diffculty;
                    break; // Found the selected difficulty, no need to continue searching
                }
            }


            App.homeViewModel.SelectedDifficulty = selectedDifficulty ?? "Beginner";
        }
    }
}

