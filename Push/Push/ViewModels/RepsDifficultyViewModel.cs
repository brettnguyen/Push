using System;
using System.Collections.ObjectModel;
using Push.Models;
using Xamarin.Essentials;

namespace Push.ViewModels
{
	public class RepsDifficultyViewModel : BaseViewModel
    {

        private ObservableCollection<RepsDifficultyModel> diffculties;
        public ObservableCollection<RepsDifficultyModel> Diffculties
        {
            get { return diffculties; }
            set
            {

                diffculties = value;
                OnPropertyChanged();
            }
        }

        private RepsDifficultyModel selectedDifficulty;
        public RepsDifficultyModel SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set
            {
                selectedDifficulty = value;
                OnPropertyChanged();
          
                
            }
        }



        private bool show;
        public bool Show
        {
            get { return show; }
            set
            {

                show = value;
                OnPropertyChanged();
            }
        }

        private string popupTitle;
        public string PopupTitle
        {
            get { return popupTitle; }
            set
            {

                popupTitle = value;
                OnPropertyChanged();
            }
        }

        private string popupText;
        public string PopupText
        {
            get { return popupText; }
            set
            {

                popupText = value;
                OnPropertyChanged();
            }
        }




        public RepsDifficultyViewModel()
		{
            Diffculties = new ObservableCollection<RepsDifficultyModel>()
            {
                new RepsDifficultyModel()
                {
                    IsChosen = Preferences.Get("BeginnerIsChosen", true),
                    Diffculty = "Beginner"
                },
                new RepsDifficultyModel()
                {
                    IsChosen = Preferences.Get("AdvancedIsChosen", false),
                    Diffculty = "Advanced"
                },
                new RepsDifficultyModel()
                {
                    IsChosen = Preferences.Get("ProfessionalIsChosen", false),
                    Diffculty = "Professional"
                },
                new RepsDifficultyModel()
                {
                    IsChosen = Preferences.Get("MasterIsChosen", false),
                    Diffculty = "Master"
                }
             };

            Show = false;
        }

        public void ShowPopUp()
        {
            if (Show == false)
            {
                Show = true;
            }
           

            switch (SelectedDifficulty.Diffculty)
            {
                case "Beginner":
                    PopupTitle = SelectedDifficulty.Diffculty;
                    PopupText = "For Beginner users who are just trying to get into fitness and push-ups. On this difficulty to complete a set you must perform 5 reps. To do damage to a boss you must do a minimum of 5 push-ups or the boss will recieve no damage.";
                    break;
                case "Advanced":
                    PopupTitle = SelectedDifficulty.Diffculty;
                    PopupText = "For Advanced users who are use to fitness and push-ups. On this difficulty to complete a set you must perform 10 reps. To do damage to a boss you must do a minimum of 10 push-ups or the boss will recieve no damage.";
                    break;
                case "Professional":
                    PopupTitle = SelectedDifficulty.Diffculty;
                    PopupText = "For Professional users who are verteran fitness goers and push-up sepcialists. On this difficulty to complete a set you must perform 20 reps. To do damage to a boss you must do a minimum of 20 push-ups or the boss will recieve no damage.";
                    break;
                case "Master":
                    PopupTitle = SelectedDifficulty.Diffculty;
                    PopupText = "For Master users who have perfected bodyweight excercise and have full control over push-ups. On this difficulty to complete a set you must perform 40 reps. To do damage to a boss you must do a minimum of 40 push-ups or the boss will recieve no damage.";
                    break;
                default:
                    // handle unexpected cases
                    break;
            }
        }

        public void ChoseDifficulty()
        {
            foreach (var difficulty in Diffculties)
            {
                difficulty.IsChosen = (difficulty == SelectedDifficulty);
                Preferences.Set($"{difficulty.Diffculty}IsChosen", difficulty.IsChosen);

                if (difficulty.IsChosen)
                {
                    Preferences.Set("SelectedDifficulty", difficulty.Diffculty);
                }
            }
            PopupTitle = null;
            PopupText = null;
            Show = false;
        }

        public void Exit()
        {
            
            PopupTitle = null;
            PopupText = null;
            Show = false;
        }
    }
}

