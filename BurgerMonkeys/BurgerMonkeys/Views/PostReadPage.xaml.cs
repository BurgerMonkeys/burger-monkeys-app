using BurgerMonkeys.Model;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class PostReadPage : ContentPage
    {
        public PostReadPage(Post post)
        {
            InitializeComponent();

            BindingContext = new PostReadViewModel(post);
        }
    }
}
