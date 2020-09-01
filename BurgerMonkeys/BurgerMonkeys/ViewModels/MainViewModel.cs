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
        readonly IWpService _wpService;

        public MainViewModel(IPostService postService,
                             IWpService wpService)
        {
            _postService = postService;
            _wpService = wpService;
            Items = new ObservableCollection<Post>();
        }

        public async override Task InitializeAsync()
        {
            if (Items.Any())
                return;

            var wpPosts = await _wpService.GetAll().ConfigureAwait(false);

            //var items = await _postService.Get().ConfigureAwait(false);
            var items = await _postService.Convert(wpPosts);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
