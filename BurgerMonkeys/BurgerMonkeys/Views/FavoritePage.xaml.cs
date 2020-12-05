using System.Threading.Tasks;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class FavoritePage
    {
        public FavoritePage()
        {
            InitializeComponent();

            var postService = DependencyService.Get<IPostService>();
            BindingContext = new FavoriteViewModel(postService);
        }
    }
}
