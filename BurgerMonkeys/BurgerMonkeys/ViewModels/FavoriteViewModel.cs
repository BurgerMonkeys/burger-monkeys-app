using System.Collections.Generic;
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
	public class FavoriteViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Post> Items { get; }

        public List<Post> AllItems { get; set; }

        readonly IPostService _postService;

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

        public ICommand UnfavoriteCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand SearchCommand { get; }

        public FavoriteViewModel(IPostService postService)
        {
            _postService = postService;

            AllItems = new List<Post>();
            Items = new ObservableRangeCollection<Post>();
            UnfavoriteCommand = new AsyncCommand<int>(ExecuteUnfavoriteCommand);
            SelectionChangedCommand = new AsyncCommand(ExecuteSelectChangeCommand);
        }

        private Task ExecuteUnfavoriteCommand(int id)
        {
            var post = Items.FirstOrDefault(i => i.Id == id);
            if (post is null)
                return Task.CompletedTask;

            _postService.SetFavorite(post);
            return GetFavoritePostsAsync();
        }

        private Task ExecuteSelectChangeCommand() => OpenPostAsync();

        public override Task InitializeAsync() =>
            GetFavoritePostsAsync();

        private async Task OpenPostAsync()
        {
            if (SelectedItem is null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new PostReadPage(SelectedItem));
            SelectedItem = null;
        }

        private async Task GetFavoritePostsAsync()
        {
            AllItems = (await _postService.GetFavorites().ConfigureAwait(false)).ToList();

            Items.Clear();
            Items.AddRange(AllItems);
            IsBusy = false;
            EmptyMessage = "Nenhum post favorito encontrado";
        }

        private void Search()
        {
            var resultItems = new List<Post>();

            if (SearchText.IsNullOrWhiteSpace())
            {
                Items.Clear();
                Items.AddRange(AllItems);
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
            Items.AddRange(resultItems);
        }
    }
}
