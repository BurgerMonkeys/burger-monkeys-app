using System.Collections.Generic;
using System.Threading.Tasks;
using BurgerMonkeys.Helpers;
using WordPressPCL;
using WordPressPCL.Models;

namespace BurgerMonkeys.Services
{
    public interface IWpService
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<IEnumerable<User>> GetAuthors();
    }

    public class WpService : IWpService
    {
        static WordPressClient _client;

        static WordPressClient Client => _client ?? new WordPressClient(Constants.Url);
       
        public WpService()
        {
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await Client.Posts.GetAll(true).ConfigureAwait(false);
            return posts;
        }

        public async Task<IEnumerable<User>> GetAuthors()
        {
            var authors = await Client.Users.GetAll().ConfigureAwait(false);
            return authors;
        }
    }
}
