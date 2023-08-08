using Booktopia.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteBookFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
