using System.Threading.Tasks;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class AuthorProfilePage : ContentPage
    {
        public AuthorProfilePage(Author author)
        {
            InitializeComponent();

            var postService = DependencyService.Get<IPostService>();
            BindingContext = new AuthorProfileViewModel(author, postService);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await OnAppearingAsync();
        }

        private async Task OnAppearingAsync()
        {
            if (BindingContext is IInitialize viewModel)
                await viewModel.InitializeAsync();
        }
    }
}
