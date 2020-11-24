using BurgerMonkeys.Model;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class AuthorProfilePage : ContentPage
    {
        public AuthorProfilePage(Author author)
        {
            InitializeComponent();

            BindingContext = new AuthorProfileViewModel(author);
        }
    }
}
