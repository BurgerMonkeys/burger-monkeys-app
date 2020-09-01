using System;
using BurgerMonkeys.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BurgerMonkeys
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Register();
            MainPage = new NavigationPage(new MainPage());
        }

        void Register()
        {
            DependencyService.Register<IPostService, PostService>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
