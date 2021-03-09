using BurgerMonkeys.Abstractions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace BurgerMonkeys
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void TabView_SelectionChanged(System.Object sender, Xamarin.CommunityToolkit.UI.Views.TabSelectionChangedEventArgs e)
        {
            if (sender is TabView tabView)
            {
                var tabViewItem = tabView.TabItems[e.NewPosition];

                if (tabViewItem.Content.BindingContext is IInitialize viewModel)
                    await viewModel.InitializeAsync().ConfigureAwait(false);
            }
        }
    }
}
