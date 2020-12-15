using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerMonkeys.Model;
using BurgerMonkeys.Repositories;
using Xamarin.Essentials;

namespace BurgerMonkeys.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> Get();
        Task<IEnumerable<Post>> GetPostCountByAuthor(int id);
        Task<IEnumerable<Post>> GetFavorites();
        Task<List<Post>> Convert(IEnumerable<WordPressPCL.Models.Post> wpPosts);
        void SetFavorite(Post post);
        Task<bool> Save(List<Post> posts);
    }

    public class PostService : IPostService
    {
        readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<Post>> Convert(IEnumerable<WordPressPCL.Models.Post> wpPosts)
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
            return await Task.FromResult(posts);
        }

        public async Task<IEnumerable<Post>> Get()
        {
            return await Task.FromResult(_postRepository.Get().OrderByDescending(p => p.Date));
        }

        public Task<IEnumerable<Post>> GetFavorites()
        {
            var posts = _postRepository.Get();

            return Task.FromResult(posts.Where(p => Preferences.ContainsKey(p.Id.ToString())));
        }

        public async Task<IEnumerable<Post>> GetPostCountByAuthor(int id)
        {
            return await Task.FromResult(_postRepository.GetPostCountByAuthor(id).OrderByDescending(p => p.Date));
        }

        public async Task<bool> Save(List<Post> posts)
        {
            return await Task.Run(() =>
            {
                var savedCount = _postRepository.Save(posts);
                return savedCount == posts.Count;
            });
        }

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
