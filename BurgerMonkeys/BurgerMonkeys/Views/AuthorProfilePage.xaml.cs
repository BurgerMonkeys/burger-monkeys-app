using System.Threading.Tasks;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class AuthorProfilePage
    {
        public AuthorProfilePage(Author author)
        {
            InitializeComponent();

            var postService = DependencyService.Get<IPostService>();
            BindingContext = new AuthorProfileViewModel(author, postService);
        }
    }
}
