using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Repositories;
using BurgerMonkeys.Services;
using LiteDB;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BurgerMonkeys
{
    public partial class App : Xamarin.Forms.Application
    {
        public static LiteDatabase Database = new LiteDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("bmapp.db")); 

        public App()
        {
            InitializeComponent();

            Register();
            var navigation = new Xamarin.Forms.NavigationPage(new MainPage());
            navigation.On<iOS>().SetPrefersLargeTitles(true);
            //navigation.BarBackgroundColor = Color.FromHex("#f0f0f0");
            //navigation.BarTextColor = Color.Black;

            MainPage = navigation;
        }

        void Register()
        {
            var postRepository = new PostRepository();
            DependencyService.RegisterSingleton<IPostRepository>(postRepository);
            DependencyService.RegisterSingleton<IPostService>(new PostService(postRepository));

            var authorsRepository = new AuthorRepository();
            DependencyService.RegisterSingleton<IAuthorRepository>(authorsRepository);
            DependencyService.RegisterSingleton<IAuthorService>(new AuthorService(authorsRepository));

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
