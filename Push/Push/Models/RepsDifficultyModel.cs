using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Push.Models
{
	public class RepsDifficultyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isChosen;
        public bool IsChosen
        {
            get { return isChosen; }
            set
            {
                isChosen = value;
                OnPropertyChanged();
            }
        }

        public string Diffculty { get; set; }
    }
}

