using BurgerMonkeys.Helpers;

namespace BurgerMonkeys.Model
{
    public abstract class BaseEntity: BindableObject
    {
        public int Id { get; set; }
    }
}
