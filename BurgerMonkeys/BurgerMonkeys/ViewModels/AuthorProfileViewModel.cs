using System;
using System.Threading.Tasks;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;

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

        private int _postCount;
        public int PostCount
        {
            get => _postCount;
            set => SetProperty(ref _postCount, value);
        }

        public AuthorProfileViewModel(
            Author author,
            IPostService postService)
        {
            Author = author;
            _postService = postService;
        }

        public async override Task InitializeAsync()
        {
            var postCount = await _postService.GetPostCountByAuthor(Author.Id);

            PostCount = postCount;
        }
    }
}
