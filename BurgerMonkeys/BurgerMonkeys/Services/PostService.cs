using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerMonkeys.Model;

namespace BurgerMonkeys.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> Get();
        Task<IEnumerable<Post>> Convert(IEnumerable<WordPressPCL.Models.Post> wpPosts);
    }

    public class PostService : IPostService
    {
        public Task<IEnumerable<Post>> Convert(IEnumerable<WordPressPCL.Models.Post> wpPosts)
        {
            var posts = new List<Post>();
            foreach (var wpPost in wpPosts)
            {
                var post = new Post
                {
                    Id = wpPost.Id,
                    Title = wpPost.Title.Rendered,
                    Date = wpPost.Date,
                    Slug = wpPost.Slug,
                    Url = wpPost.Link
                };

                var authors = wpPost.Embedded.Author;
                if (authors.Any())
                    post.Author = authors.First().Name;
                if (!(wpPost.Embedded.WpFeaturedmedia is null))
                {
                    var media = wpPost.Embedded.WpFeaturedmedia.First();
                    if (!(media is null))
                    {
                        post.Image = media.SourceUrl;
                        post.Thumbnail = "http://i1.wp.com/" + media.SourceUrl
                                    .Replace("https://", "")
                                    .Replace("http://", "") + "?h=150";
                    }
                }
                posts.Add(post);
            }
            return Task.FromResult<IEnumerable<Post>>(posts);
        }

        public async Task<IEnumerable<Post>> Get()
        {
            return await Task.FromResult(new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Widgets no iOS 14: limitados ou complexos o suficiente?",
                    Body = "Body do primeiro post",
                    Date = DateTime.Now,
                    Slug = "primeiro-post",
                    Url = "https://burgermonkeys.com/widgets-no-ios-14-limitados-ou-complexos-o-suficiente/",
                    Image = "https://burgermonkeys.com/wp-content/uploads/2020/06/ios-14-widgets.jpg",
                    Author = "Breno Gregorio Angelotti"
                },
                new Post
                {
                    Id = 2,
                    Title = "Como Code Snippets do Visual Studio for Mac pode nos ajudar na criação de apps Xamarin Forms?",
                    Body = "Body do segundo post",
                    Date = DateTime.Now,
                    Slug = "segundo-post",
                    Url = "https://burgermonkeys.com/como-code-snippets-do-visual-studio-for-mac-pode-nos-ajudar-na-criacao-de-apps-xamarin-forms/",
                    Image = "https://burgermonkeys.com/wp-content/uploads/2020/06/code-snippets.png",
                    Author = "Ricardo Prestes"
                }
                ,
                new Post
                {
                    Id = 3,
                    Title = "Compartilhando banco de dados entre apps",
                    Body = "Body do segundo post",
                    Date = DateTime.Now,
                    Slug = "segundo-post",
                    Url = "https://burgermonkeys.com/compartilhando-banco-de-dados-entre-apps/",
                    Image = "https://burgermonkeys.com/wp-content/uploads/2020/04/BannerPost-1.png",
                    Author = "Eduardo Pacheco"
                }
            });
        }
    }
}
