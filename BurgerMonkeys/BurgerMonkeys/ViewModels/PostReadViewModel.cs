using System.Text;
using System.Windows.Input;
using BurgerMonkeys.Model;
using BurgerMonkeys.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class PostReadViewModel : BaseViewModel
    {
        private readonly IPostService _postService;

        private readonly Post _post;

        private string _favoriteText;
        public string FavoriteText
        {
            get => _favoriteText;
            set => SetProperty(ref _favoriteText, value);
        }

        private string _favoriteIcon;
        public string FavoriteIcon
        {
            get => _favoriteIcon;
            set => SetProperty(ref _favoriteIcon, value);
        }

        private string _postTitle;
        public string PostTitle
        {
            get => _postTitle;
            set => SetProperty(ref _postTitle, value);
        }

        private HtmlWebViewSource _body;
        public HtmlWebViewSource Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }

        public ICommand FavoriteCommand { get; set; }
        public ICommand ShareCommand { get; set; }

        public PostReadViewModel(IPostService postService,
                                 Post post)
        {
            _postService = postService;
            _post = post;
            _post.Favorite = Preferences.ContainsKey(post.Id.ToString());
            PostTitle = _post.Title;

            FavoriteCommand = new Command(ExecutedFavoriteCommand);
            ShareCommand = new Command(ExecuteShareCommand);

            SetFavoriteText();
            SetFavoriteIcon();
            LoadPost();
        }

        

        private void LoadPost()
        {
            var cssFile = App.Current.RequestedTheme == OSAppTheme.Light ? "leitor.css" : "leitor-dark.css";

            var sb = new StringBuilder();

            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append($"<link href=\"{cssFile}\" rel=\"stylesheet\" />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append($"<h1>{_post.Title}</h1>");
            if (!string.IsNullOrWhiteSpace(_post.Image))
            {
                sb.Append($"<figure><img src=\"{_post.Image}\"></figure>");
                sb.Append("<br/>");
            }
            sb.Append($"<p class=\"details\">Por <span class=\"author\">{_post.Author}</span> em <span class=\"publish-date\">{_post.Date.ToString("d")}</span></p>");
            sb.Append(_post.Body);
            sb.Append("</body>");
            sb.Append("</html>");

            var html = new HtmlWebViewSource
            {
                Html = sb.ToString()
            };
            Body = html;
        }

        private void ExecutedFavoriteCommand()
        {
            _postService.SetFavorite(_post);
            SetFavoriteText();
            SetFavoriteIcon();
        }

        private async void ExecuteShareCommand() 
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = _post.Url,
                Title = _post.Title,
                Text = "Veja este post na BurgerMonkeys"
            });
        }

        private void SetFavorite()
        {
            _post.Favorite = !_post.Favorite;
            var id = _post.Id.ToString();
            if (_post.Favorite)
                Preferences.Set(id, true);
            else
                Preferences.Remove(id);

        }

        private void SetFavoriteText() =>
            FavoriteText = _post.Favorite ? "Remover" : "Favoritar";

        private void SetFavoriteIcon() =>
            FavoriteIcon = _post.Favorite ? "star" : "star_outline";
    }
}
