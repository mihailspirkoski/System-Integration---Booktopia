using Booktopia.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public BooktopiaAppUser Owner { get; set; }
        public virtual ICollection<BookInShoppingCart> BooksInShoppingCart { get; set; }
    }
}
