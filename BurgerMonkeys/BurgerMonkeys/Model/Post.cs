using System;
namespace BurgerMonkeys.Model
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Slug { get; set; }
        public string Author { get; set; }
    }
}
