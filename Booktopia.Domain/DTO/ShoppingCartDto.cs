using Booktopia.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<BookInShoppingCart> Books { get; set; }
        public float TotalPrice { get; set; }
    }
}
