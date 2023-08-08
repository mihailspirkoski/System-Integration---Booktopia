using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Booktopia.Domain.DomainModels
{
    public class Book : BaseEntity
    {
        [Required]
        public string BookName { get; set; }
        [Required]
        public string BookImage { get; set; }
        [Required]
        public float BookPrice { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public virtual ICollection<BookInShoppingCart> BooksInShoppingCart { get; set; }
        public IEnumerable<BookInOrder> BooksInOrder { get; set; }
    }
}
