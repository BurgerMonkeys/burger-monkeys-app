using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BurgerMonkeys.Services;
using Xamarin.Forms.Internals;

namespace BurgerMonkeys.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<string> Items { get; set; }

        readonly IPostService _postService;

        public MainViewModel(IPostService postService)
        {
            _postService = postService;
            Items = new ObservableCollection<string>();
        }

        public async override Task InitializeAsync()
        {
            var items = await _postService.Get().ConfigureAwait(false);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
