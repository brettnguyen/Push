using System;
using System.Diagnostics;
using Xamarin.Essentials;

namespace Push.ViewModels
{
	public class HomeViewModel : BaseViewModel
    {
        private string selectedDifficulty;
        public string SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set
            {
                if (selectedDifficulty != value)
                {
                    selectedDifficulty = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"SelectedDifficulty set to: {value}");
                }
            }
        }

        public HomeViewModel()
		{
            SelectedDifficulty = Preferences.Get("SelectedDifficulty", "Beginner");
        }
	}
}

