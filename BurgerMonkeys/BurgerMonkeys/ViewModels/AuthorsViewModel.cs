using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BurgerMonkeys.Model;
using BurgerMonkeys.Views;
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class AuthorsViewModel : BaseViewModel
    {
        public ObservableCollection<Author> Authors { get; set; }

        private Author _selectedItem;
        public Author SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ICommand SelectionChangedCommand => new Command(ExecutedSelectionChangedCommandAsync);

        public AuthorsViewModel()
        {
            Authors = new ObservableCollection<Author>();
        }

        public async override Task InitializeAsync() =>
            await GetAuthors().ConfigureAwait(false);

        private async void ExecutedSelectionChangedCommandAsync()
        {
            await OpenPrifileAsync();
        }

        private async Task OpenPrifileAsync()
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
            if (Authors.Any())
                return;

            await Task.Run(() =>
            {
                Authors.Add(new Author
                {
                    Avatar = "https://s.gravatar.com/avatar/96ff57841c27f46f64c7d75fe5577f00?s=100",
                    Name = "Breno Angelotti",
                    Github = "@BrenoAngelotti"
                });
                Authors.Add(new Author
                {
                    Avatar = "https://s.gravatar.com/avatar/32eb7d391b76a1d0773eb77ba18e34bc?s=100",
                    Name = "Eduardo Pacheco Beraldo",
                    Github = "@EduardoPac"
                });
                Authors.Add(new Author
                {
                    Avatar = "https://s.gravatar.com/avatar/9802e38d5bd2cd85db8b0720d5feed29?s=100",
                    Name = "Ricardo Prestes",
                    Github = "@ricardoprestes"
                });
            });
        }
    }
}
