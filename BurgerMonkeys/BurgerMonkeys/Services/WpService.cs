using System.Collections.Generic;
using System.Threading.Tasks;
using BurgerMonkeys.Helpers;
using WordPressPCL;
using WordPressPCL.Models;

namespace BurgerMonkeys.Services
{
    public interface IWpService
    {
        Task<IEnumerable<Post>> GetAll();
    }

    public class WpService : IWpService
    {
        static WordPressClient _client;

        static WordPressClient Client => _client ?? new WordPressClient(Constants.Url);
       
        public WpService()
        {
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            var posts = await Client.Posts.GetAll(true);
            return posts;
        }
    }
}
