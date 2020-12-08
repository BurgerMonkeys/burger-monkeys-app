using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class AuthorPostsViewModel : BaseViewModel
    {
        private readonly IPostService _postService;
        public ObservableRangeCollection<Post> Items { get; set; }

        private string _emptyMessage;
        public string EmptyMessage
        {
            get => _emptyMessage;
            set => SetProperty(ref _emptyMessage, value);
        }

        private Post _selectedItem;
        public Post SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ICommand SelectionChangedCommand { get; }
        public ICommand FavoriteCommand { get; }

        public AuthorPostsViewModel(
            IEnumerable<Post> posts,
            IPostService postService)
        {
            Title = posts.First().Author;
            Items = new ObservableRangeCollection<Post>();
            Items.AddRange(posts);

            _postService = postService;
            SelectionChangedCommand = new AsyncCommand(ExecuteSelectionChangedCommand);
            FavoriteCommand = new Command<int>(ExecuteFavoriteCommand);
        }

        private Task ExecuteSelectionChangedCommand() => OpenPostAsync();

        private async Task OpenPostAsync()
        {
            if (SelectedItem is null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new PostReadPage(SelectedItem));
        }

        private void ExecuteFavoriteCommand(int id)
        {
            var post = Items.FirstOrDefault(i => i.Id == id);

            if (post is null)
                return;

            _postService.SetFavorite(post);
        }
    }
}
