using BurgerMonkeys.Abstractions;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
	public class BasePage : ContentPage
	{
		protected override async void OnAppearing()
		{
			if (BindingContext is IInitialize viewModel)
				await viewModel.InitializeAsync().ConfigureAwait(false);
		}
	}
}
