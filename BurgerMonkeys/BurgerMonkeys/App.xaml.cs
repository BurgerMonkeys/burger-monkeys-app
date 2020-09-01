using BurgerMonkeys.Services;
using Xamarin.Forms;

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
            DependencyService.RegisterSingleton<IPostService>(new PostService());
            DependencyService.RegisterSingleton<IWpService>(new WpService());
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
