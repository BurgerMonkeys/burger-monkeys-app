using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurgerMonkeys.Model;
using BurgerMonkeys.Repositories;

namespace BurgerMonkeys.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> Convert(IEnumerable<WordPressPCL.Models.User> users);
        Task<List<Author>> Get();
        Task<bool> Save(List<Author> authors);
    }

    public class AuthorService : IAuthorService
    {
        readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<Author>> Convert(IEnumerable<WordPressPCL.Models.User> users)
        {
            var authors = new List<Author>();

            return await Task.Run(() =>
            {
                foreach (var user in users)
                {
                    var author = new Author
                    {
                        Id = user.Id,
                        Avatar = user.AvatarUrls.Size96,
                        Bio = user.Description,
                        Name = user.Name,
                        Github = "@burgermonkeys"
                    };

                    authors.Add(author);
                }

                return authors;
            });
        }

		public async Task<List<Author>> Get() => await Task.FromResult(_authorRepository.Get().OrderBy(a => a.Name).ToList());

        public async Task<bool> Save(List<Author> authors)
        {
            return await Task.Run(() =>
            {
                var postCount = _authorRepository.Save(authors);
                return postCount == authors.Count;
            });
        }
    }
}
