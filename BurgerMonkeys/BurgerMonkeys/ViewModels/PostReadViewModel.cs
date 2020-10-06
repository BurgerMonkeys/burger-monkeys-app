using System.Text;
using BurgerMonkeys.Model;
using Xamarin.Forms;

namespace BurgerMonkeys.ViewModels
{
    public class PostReadViewModel : BaseViewModel
    {
        private readonly Post _post;

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

        public PostReadViewModel(Post post)
        {
            _post = post;
            PostTitle = _post.Title;

            var cssFile = "leitor.css";

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
    }
}
