using BurgerMonkeys.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BurgerMonkeys
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            Register();

            var navigation = new Xamarin.Forms.NavigationPage(new MainPage());
            navigation.On<iOS>().SetPrefersLargeTitles(true);
            navigation.BarBackgroundColor = Color.FromHex("#f0f0f0");
            navigation.BarTextColor = Color.Black;

            MainPage = navigation;
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
