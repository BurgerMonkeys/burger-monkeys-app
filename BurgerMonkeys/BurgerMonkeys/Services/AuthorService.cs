using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BurgerMonkeys.Model;

namespace BurgerMonkeys.Services
{
    public interface IAuthorService
    {
        void Convert(IEnumerable<WordPressPCL.Models.User> users);
        Task<List<Author>> Get();
    }

    public class AuthorService : IAuthorService
    {
        List<Author> Authors = new List<Author>();

        public void Convert(IEnumerable<WordPressPCL.Models.User> users)
        {
            var authors = new List<Author>();

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

            Authors = authors;
        }

		public Task<List<Author>> Get() => Task.FromResult(Authors);
	}
}
