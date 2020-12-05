using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.Tools;
using BurgerMonkeys.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class FeedViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Post> Items { get; }
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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                Search();
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

        public ICommand SelectionChangedCommand { get; }

        public ICommand FavoriteCommand { get; }

        public FeedViewModel(IPostService postService,
                             IWpService wpService)
        {
            _postService = postService;
            _wpService = wpService;
            Items = new ObservableRangeCollection<Post>();
            RefreshCommand = new AsyncCommand(ExecuteRefreshCommand);
            SearchCommand = new Command(ExecuteSearchCommand);
            SelectionChangedCommand = new AsyncCommand(ExecutedSelectionChangedCommand);
            FavoriteCommand = new Command<int>(ExecutedFavoriteCommand);
        }

		private Task ExecuteRefreshCommand() => GetPostsAsync();

		private Task ExecutedSelectionChangedCommand() => OpenPostAsync();

		public override Task InitializeAsync()
        {
            if (Items.Any())
                return Task.CompletedTask;

            return GetPostsAsync();
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
            EmptyMessage = "Carregando...";
            EmptyImage = "monkey.json";
            var wpPosts = await _wpService.GetPosts().ConfigureAwait(false);
            await _postService.Convert(wpPosts).ConfigureAwait(false);
            AllItems = (await _postService.Get().ConfigureAwait(false)).ToList();

            Items.Clear();
            foreach (var item in AllItems)
            {
                Items.Add(item);
            }
            IsBusy = false;
            EmptyMessage = "Nenhum post encontrado";
            EmptyImage = "empty.json";
        }

		private void ExecuteSearchCommand() => Search();

		private void Search()
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

        private void ExecutedFavoriteCommand(int id)
        {
            var post = Items.FirstOrDefault(i => i.Id == id);
            if (post is null)
                return;

            _postService.SetFavorite(post);
        }
    }
}
