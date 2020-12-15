using System;
using System.Collections.Generic;
using BurgerMonkeys.Model;
using LiteDB;

namespace BurgerMonkeys.Repositories
{
    public interface IPostRepository
    {
        IEnumerable<Post> Get();
        int Save(List<Post> posts);
        IEnumerable<Post> GetPostCountByAuthor(int id);
    }

    public class PostRepository : IPostRepository
    {
        readonly ILiteCollection<Post> _liteCollection = App.Database.GetCollection<Post>("posts");

        public IEnumerable<Post> Get() => _liteCollection.FindAll();

        public IEnumerable<Post> GetPostCountByAuthor(int id)
        {
            return _liteCollection.Find(p => p.AuthorId == id);
        }

        public int Save(List<Post> posts)
        {
            try
            {
                var savedCount = _liteCollection.Upsert(posts);
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
