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
            var html = new HtmlWebViewSource
            {
                Html = $"<h1>{_post.Title}</h1>{_post.Body}"
            };
            Body = html;
        }
    }
}
