using Booktopia.Domain.DomainModels;
using Booktopia.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Booktopia.Services.Interface
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetDetailsForBook(Guid? id);
        void CreateNewBook(Book b);
        void UpdateExistingBook(Book b);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteBook(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
        Task<List<Book>> SearchByGenres(string? Genre);
        Task<List<string>> GetAllGenres();
    }
}
