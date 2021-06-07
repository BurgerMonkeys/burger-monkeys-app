using System;
using System.Collections.ObjectModel;
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
    public class AuthorsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Author> Authors { get; set; }
        readonly IWpService _wpService;
        readonly IAuthorService _authorService;

        bool loaded;

        private Author _selectedItem;
        public Author SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ICommand SelectionChangedCommand => new AsyncCommand(ExecutedSelectionChangedCommandAsync);

        public AuthorsViewModel(IWpService wpService,
                                IAuthorService authorService)
        {
            _wpService = wpService;
            Authors = new ObservableRangeCollection<Author>();
            _authorService = authorService;
        }

        public async override Task InitializeAsync()
        {
            await GetAuthors().ConfigureAwait(false);

            if (!loaded)
                await DownloadAuthors().ConfigureAwait(false);
        }

        private async Task DownloadAuthors()
        {
            var users = await _wpService.GetAuthors().ConfigureAwait(false);
            var authorsSave = await _authorService.Convert(users).ConfigureAwait(false);
            var saved = await _authorService.Save(authorsSave).ConfigureAwait(false);

            if (saved)
            {
                await GetAuthors().ConfigureAwait(false);
                loaded = true;
            }
        }

        private Task ExecutedSelectionChangedCommandAsync() => OpenProfileAsync();

		private async Task OpenProfileAsync()
        {
            if (SelectedItem is null)
                return;

            await Application
                    .Current
                    .MainPage
                    .Navigation
                    .PushAsync(new AuthorProfilePage(SelectedItem))
                    .ConfigureAwait(false);
            SelectedItem = null;
        }

        private async Task GetAuthors()
        {
            var authors = await _authorService.Get();

            if (authors is null || !authors.Any())
                return;

            if(Authors.Any())
                Authors.Clear();

            Authors.AddRange(authors);
        }
    }
}
