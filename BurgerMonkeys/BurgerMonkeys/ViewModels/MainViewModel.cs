using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;

namespace BurgerMonkeys.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Items { get; set; }

        readonly IPostService _postService;
        readonly IWpService _wpService;

        public ICommand  RefreshCommand { get; }

        public MainViewModel(IPostService postService,
                             IWpService wpService)
        {
            _postService = postService;
            _wpService = wpService;
            Items = new ObservableCollection<Post>();
            RefreshCommand = new Command(ExecuteRefreshCommand);
        }

        private async void ExecuteRefreshCommand()
        {
            await GetPosts();
        }

        public async override Task InitializeAsync()
        {
            if (Items.Any())
                return;
            await GetPosts().ConfigureAwait(false);
        }

        private async Task GetPosts()
        {
            var wpPosts = await _wpService.GetAll().ConfigureAwait(false);
            var items = await _postService.Convert(wpPosts).ConfigureAwait(false);
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
            IsBusy = false;
        }
    }
}
