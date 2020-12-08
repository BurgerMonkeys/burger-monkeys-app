using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using BurgerMonkeys.Views;
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class AuthorProfileViewModel : BaseViewModel, IInitialize
    {
        readonly IPostService _postService;

        private Author _author;
        public Author Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        List<Post> PostsAuthor { get; set; }

        private int _postCount;
        public int PostCount
        {
            get => _postCount;
            set => SetProperty(ref _postCount, value);
        }

        public ICommand OpenPostsCommand { get; }

        public AuthorProfileViewModel(
            Author author,
            IPostService postService)
        {
            Author = author;
            _postService = postService;
            OpenPostsCommand = new Command(ExecuteOpenPostsCommand);
        }

        private void ExecuteOpenPostsCommand()
        {
            if(PostCount > 0)
                Application.Current.MainPage
                    .Navigation.PushAsync(new AuthorPostsPage(PostsAuthor));
        }

        public async override Task InitializeAsync()
        {
            PostsAuthor = (await _postService.GetPostCountByAuthor(Author.Id)).ToList();

            PostCount = PostsAuthor.Count;
        }
    }
}
