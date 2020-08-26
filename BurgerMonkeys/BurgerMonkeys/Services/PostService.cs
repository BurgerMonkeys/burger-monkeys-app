using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BurgerMonkeys.Services
{
    public interface IPostService
    {
        Task<IEnumerable<string>> Get();
    }

    public class PostService : IPostService
    {
        public async Task<IEnumerable<string>> Get()
        {
            return await Task.FromResult(new List<string>
            {
                "Gorila", "Xpanze", "Mico"
            });
        }
    }
}
