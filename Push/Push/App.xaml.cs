using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Push.Services;
using Push.Views;
using Push.ViewModels;

namespace Push
{
    public partial class App : Application
    {
        public static HomeViewModel homeViewModel { get; set; }
        public App ()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            homeViewModel = new HomeViewModel();
            MainPage = new AppShell();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

