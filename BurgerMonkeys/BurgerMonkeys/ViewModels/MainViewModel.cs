using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;

namespace BurgerMonkeys.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Items { get; set; }

        readonly IPostService _postService;

        public MainViewModel(IPostService postService)
        {
            _postService = postService;
            Items = new ObservableCollection<Post>();
        }

        public async override Task InitializeAsync()
        {
            if (Items.Any())
                return;

            var items = await _postService.Get().ConfigureAwait(false);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
