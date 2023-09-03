using System;
using System.Collections.Generic;
using Push.ViewModels;
using Push.Views;
using Xamarin.Forms;

namespace Push
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Sets", typeof(SetsPage));
            Routing.RegisterRoute("Reps", typeof(RepsDifficultyPage));
            Routing.RegisterRoute("BattleField", typeof(BattleField));
            

        }

    }
}

