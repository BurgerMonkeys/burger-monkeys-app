using System.Collections.Generic;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class AuthorPostsPage
    {
        public AuthorPostsPage(IEnumerable<Post> posts)
        {
            InitializeComponent();

            var postService = DependencyService.Get<IPostService>();
            BindingContext = new AuthorPostsViewModel(posts, postService);
        }
    }
}
