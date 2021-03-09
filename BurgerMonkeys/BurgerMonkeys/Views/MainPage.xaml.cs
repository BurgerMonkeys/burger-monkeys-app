using System.Threading.Tasks;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Content is TabView tabView)
            {
                var tabViewItem = tabView.TabItems[0];
                await InitializeAsync(tabViewItem).ConfigureAwait(false);
            }
        }

        async void TabView_SelectionChanged(System.Object sender, Xamarin.CommunityToolkit.UI.Views.TabSelectionChangedEventArgs e)
        {
            if (sender is TabView tabView)
            {
                var tabViewItem = tabView.TabItems[e.NewPosition];
                await InitializeAsync(tabViewItem).ConfigureAwait(false);
            }
        }

        private static async Task InitializeAsync(TabViewItem tabViewItem)
        {
            if (tabViewItem.Content.BindingContext is IInitialize viewModel)
                await viewModel.InitializeAsync().ConfigureAwait(false);
        }
    }
}
