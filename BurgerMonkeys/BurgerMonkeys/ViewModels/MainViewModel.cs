using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using Xamarin.Forms;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.Tools;

namespace BurgerMonkeys.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Items { get; set; }
        public List<Post> AllItems { get; set; }

        readonly IPostService _postService;
        readonly IWpService _wpService;

        private string _searchText;
        public string SearchText {
            get => _searchText;
            set {
                SetProperty(ref _searchText, value);
                SearchAsync();
            }
        }

        public ICommand RefreshCommand { get; }

        public ICommand SearchCommand { get; }

        public MainViewModel(IPostService postService,
                             IWpService wpService)
        {
            _postService = postService;
            _wpService = wpService;
            Items = new ObservableCollection<Post>();
            RefreshCommand = new Command(ExecuteRefreshCommand);
            SearchCommand = new Command(ExecuteSearchCommand);
        }

        private async void ExecuteRefreshCommand()
        {
            await GetPostsAsync();
        }

        public async override Task InitializeAsync()
        {
            if (Items.Any())
                return;
            await GetPostsAsync().ConfigureAwait(false);
        }

        private async Task GetPostsAsync()
        {
            var wpPosts = await _wpService.GetAll().ConfigureAwait(false);
            AllItems = (await _postService.Convert(wpPosts).ConfigureAwait(false)).ToList();
            Items.Clear();
            foreach (var item in AllItems)
            {
                Items.Add(item);
            }
            IsBusy = false;
        }

        private async void ExecuteSearchCommand()
        {
            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            var resultItems = new List<Post>();

            if (SearchText.IsNullOrWhiteSpace())
            {
                Items.Clear();
                AllItems.ForEach(i => Items.Add(i));
                return;
            }

            var cleanSearchText = SearchText.IgnoreCaseSensitiveAndAccents();

            resultItems = AllItems.Where(i =>
                i.Title.IgnoreCaseSensitiveAndAccents()
                    .Contains(cleanSearchText) ||
                i.Author.IgnoreCaseSensitiveAndAccents()
                    .Contains(cleanSearchText)
                ).ToList();
            Items.Clear();
            resultItems.ForEach(i => Items.Add(i));
        }
    }
}
