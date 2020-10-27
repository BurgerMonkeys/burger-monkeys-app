using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class PostReadPage : ContentPage
    {
        public PostReadPage(Post post)
        {
            InitializeComponent();
            var postService = DependencyService.Get<IPostService>();
            BindingContext = new PostReadViewModel(postService, post);
        }
    }
}
