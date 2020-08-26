using System;
using System.Threading.Tasks;

namespace BurgerMonkeys.Abstractions
{
    public interface IInitialize
    {
        Task InitializeAsync();
    }
}
