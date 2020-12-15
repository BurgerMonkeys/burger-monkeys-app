using System;
using System.Collections.Generic;
using BurgerMonkeys.Model;
using LiteDB;

namespace BurgerMonkeys.Repositories
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> Get();
        int Save(List<Author> authors);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly ILiteCollection<Author> _liteCollection = App.Database.GetCollection<Author>("authors");

        public IEnumerable<Author> Get()
        {
            return _liteCollection.FindAll();
        }

        public int Save(List<Author> authors)
        {
            try
            {
                var savedCount = _liteCollection.Upsert(authors);
                return savedCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
