using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerMonkeys.Model;
using Xamarin.Essentials;

namespace BurgerMonkeys.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> Get();
        Task<int> GetPostCountByAuthor(int id);
        Task<IEnumerable<Post>> GetFavorites();
        Task Convert(IEnumerable<WordPressPCL.Models.Post> wpPosts);
        void SetFavorite(Post post);
    }

    public class PostService : IPostService
    {
        static List<Post> Posts = new List<Post>();
 
        public Task Convert(IEnumerable<WordPressPCL.Models.Post> wpPosts)
        {
            var posts = new List<Post>();
            foreach (var wpPost in wpPosts)
            {
                var post = new Post
                {
                    Id = wpPost.Id,
                    Title = wpPost.Title.Rendered.Replace("&#8211;", "-"),
                    Date = wpPost.Date,
                    Slug = wpPost.Slug,
                    Url = wpPost.Link,
                    Body = wpPost.Content.Rendered,
                    Favorite = Preferences.ContainsKey(wpPost.Id.ToString())
                };

                var authors = wpPost.Embedded.Author;
                if (authors.Any())
                {
                    var firstAuthor = authors.First();
                    post.Author = firstAuthor.Name;
                    post.AuthorId = firstAuthor.Id;
                }
                    
                if (wpPost.Embedded.WpFeaturedmedia is { })
                {
                    var media = wpPost.Embedded.WpFeaturedmedia.First();
                    if (media is { })
                    {
                        post.Image = media.SourceUrl;
                        post.Thumbnail = "http://i1.wp.com/" + media.SourceUrl
                                    .Replace("https://", "")
                                    .Replace("http://", "") + "?h=150";
                    }
                }
                else
                {
                    post.Thumbnail = "bm_pattern";
                }
                posts.Add(post);
            }
            Posts = posts;
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Post>> Get() => Task.FromResult<IEnumerable<Post>>(Posts);

        public Task<IEnumerable<Post>> GetFavorites() =>
            Task.FromResult(Posts.Where(p => Preferences.ContainsKey(p.Id.ToString())));

        public Task<int> GetPostCountByAuthor(int id) =>
            Task.FromResult(Posts.Count(p => p.AuthorId == id));

        public void SetFavorite(Post post)
        {
            post.Favorite = !post.Favorite;
            var id = post.Id.ToString();
            if (post.Favorite)
                Preferences.Set(id, true);
            else
                Preferences.Remove(id);
        }
    }
}
