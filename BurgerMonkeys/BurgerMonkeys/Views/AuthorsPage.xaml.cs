using System;
using System.Threading.Tasks;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class AuthorsPage : ContentPage
    {
        public AuthorsPage()
        {
            InitializeComponent();

            BindingContext = new AuthorsViewModel();
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
