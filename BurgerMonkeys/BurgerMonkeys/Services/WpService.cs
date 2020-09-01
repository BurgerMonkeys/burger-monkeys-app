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
        readonly WordPressClient _client;

        public WpService()
        {
            _client = new WordPressClient(Constants.Url);
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            var posts = await _client.Posts.GetAll(true);
            return posts;
        }
    }
}
