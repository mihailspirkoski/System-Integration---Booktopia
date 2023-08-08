using Booktopia.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Domain.DTO
{
    public class SearchBooksByGenreDto
    {
        public List<Book> Books;
        public List<string> Genres { get; set; }
    }
}
