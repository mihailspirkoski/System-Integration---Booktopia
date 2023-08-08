using Booktopia.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Book SelectedBook { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}
