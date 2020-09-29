using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using Xamarin.Forms;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.Tools;
using BurgerMonkeys.Views;

namespace BurgerMonkeys.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Items { get; set; }
        public List<Post> AllItems { get; set; }

        readonly IPostService _postService;
        readonly IWpService _wpService;

        private string _emptyMessage;
        public string EmptyMessage
        {
            get => _emptyMessage;
            set => SetProperty(ref _emptyMessage, value);
        }

        private string _emptyImage;
        public string EmptyImage
        {
            get => _emptyImage;
            set => SetProperty(ref _emptyImage, value);
        }

        private bool _autoPlay;
        public bool AutoPlay
        {
            get => _autoPlay;
            set => SetProperty(ref _autoPlay, value);
        }

        private string _searchText;
        public string SearchText {
            get => _searchText;
            set {
                SetProperty(ref _searchText, value);
                SearchAsync();
            }
        }

        private Post _selectedItem;
        public Post SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }


        public ICommand RefreshCommand { get; }

        public ICommand SearchCommand { get; }

        public ICommand SelectionChangedCommand => new Command(ExecutedSelectionChangedCommand);

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

        private async void ExecutedSelectionChangedCommand()
        {
            await OpenPostAsync();
        }

        public async override Task InitializeAsync()
        {
            if (Items.Any())
                return;
            await GetPostsAsync().ConfigureAwait(false);
        }

        private async Task OpenPostAsync()
        {
            if (SelectedItem is null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new PostReadPage(SelectedItem));
            SelectedItem = null;
        }

        private async Task GetPostsAsync()
        {
            AutoPlay = false;
            EmptyMessage = "Carregando...";
            EmptyImage = "monkey.json";
            AutoPlay = true;
            var wpPosts = await _wpService.GetAll().ConfigureAwait(false);
            AllItems = (await _postService.Convert(wpPosts).ConfigureAwait(false)).ToList();
            Items.Clear();
            foreach (var item in AllItems)
            {
                Items.Add(item);
            }
            IsBusy = false;
            AutoPlay = false;
            EmptyMessage = "Nenhum post encontrado";
            EmptyImage = "empty.json";
            AutoPlay = true;
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
