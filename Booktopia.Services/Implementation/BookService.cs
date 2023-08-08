using Booktopia.Domain.DomainModels;
using Booktopia.Domain.DTO;
using Booktopia.Repository.Interface;
using Booktopia.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktopia.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BookService> _logger;
        private readonly IBookkRepository _bookkRepository;

        public BookService(IRepository<Book> bookRepository, IUserRepository userRepository,
            IRepository<BookInShoppingCart> bookInShoppingCartRepository, ILogger<BookService> logger,
            IBookkRepository bookkRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _bookInShoppingCartRepository = bookInShoppingCartRepository;
            _logger = logger;
            _bookkRepository = bookkRepository;
        }

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {

            var user = this._userRepository.Get(userID);

            var userShoppingCart = user.UserCart;


            if (item.BookId != null && userShoppingCart != null)
            {
                var book = this.GetDetailsForBook(item.BookId);

                if (book != null)
                {
                    BookInShoppingCart itemToAdd = new BookInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Book = book,
                        BookId = book.Id,
                        ShoppingCart = userShoppingCart,
                        ShoppingCartId = userShoppingCart.Id,
                        Quantity = item.Quantity
                    };

                    this._bookInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation("Well done. Book successfully added into ShoppingCart");
                    return true;
                }
                return false;
            }
            _logger.LogInformation("Something was wrong. BookId or UserShoppingCart may be unavailable");
            return false;
        }

        public void CreateNewBook(Book b)
        {
            this._bookRepository.Insert(b);
        }

        public void DeleteBook(Guid id)
        {
            var book = this.GetDetailsForBook(id);
            this._bookRepository.Delete(book);
        }

        public List<Book> GetAllBooks()
        {
            return this._bookRepository.GetAll().ToList();
        }

        public async Task<List<string>> GetAllGenres()
        {
            return await this._bookkRepository.GetAllGenres();
        }

        public Book GetDetailsForBook(Guid? id)
        {
            return this._bookRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var book = this.GetDetailsForBook(id);

            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedBook = book,
                BookId = book.Id,
                Quantity = 1
            };

            return model;
        }

        public async Task<List<Book>> SearchByGenres(string Genre)
        {
            if (Genre == null || Genre.Equals(""))
            {
                return await this._bookkRepository.GetAll();
            }
                

            return await this._bookkRepository.SearchByGenres(Genre);
        }

        public void UpdateExistingBook(Book b)
        {
            this._bookRepository.Update(b);
        }
    }
}
