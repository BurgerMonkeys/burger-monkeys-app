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
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class FavoriteViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Items { get; set; }
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
                SearchAsync();
            }
        }

        private Post _selectedItem;
        public Post SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ICommand UnfavoriteCommand => new Command<int>(ExecuteUnfavoriteCommand);
        public ICommand SelectionChangedCommand => new Command(ExecuteSelectChangeCommand);


        public FavoriteViewModel(IPostService postService)
        {
            _postService = postService;

            AllItems = new List<Post>();
            Items = new ObservableCollection<Post>();
        }

        private async void ExecuteUnfavoriteCommand(int id)
        {
            var post = Items.FirstOrDefault(i => i.Id == id);
            if (post is null)
                return;

            _postService.SetFavorite(post);
            await GetFavoritePostsAsync().ConfigureAwait(false);
        }

        private async void ExecuteSelectChangeCommand() => await OpenPostAsync();

        public async override Task InitializeAsync() =>
            await GetFavoritePostsAsync().ConfigureAwait(false);

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
            foreach (var item in AllItems)
            {
                Items.Add(item);
            }
            IsBusy = false;
            EmptyMessage = "Nenhum post favorito encontrado";
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
