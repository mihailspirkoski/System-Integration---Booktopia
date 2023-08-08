using Booktopia.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public BooktopiaAppUser User { get; set; }

        public IEnumerable<BookInOrder> BooksInOrder { get; set; }
    }
}
