using Booktopia.Domain.DomainModels;
using Booktopia.Domain.DTO;
using Booktopia.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Booktopia.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: Books
        public IActionResult Index()
        {
            var allBooks = this._bookService.GetAllBooks();
            return View(allBooks);
        }

        // GET: ListBooksByGenre - genres only
        public async Task<IActionResult> ListBooksByGenre(string? Genre)
        {
            if (Genre != null && Genre.Contains("Choose"))
            {
                Genre = null;
            }
                

            SearchBooksByGenreDto result = new SearchBooksByGenreDto()
            {
                Books = await this._bookService.SearchByGenres(Genre),
                Genres = await this._bookService.GetAllGenres()
            };

            return View(result);
        }

        // GET: ListBooksByGenre - genres and books
        public async Task<IActionResult> ListBooksByGenree(string? Genre)
        {
            if (Genre != null && Genre.Contains("Choose"))
            {
                Genre = null;

                List<Book> newbooks = new List<Book>();
                Book newbook = new Book()
                {
                    BookName = "",
                    BookImage = null,
                    BookPrice = 0,
                    Genre = "",
                    Author = null,
                    DateCreated = DateTime.Now,
                    BooksInOrder = null,
                    BooksInShoppingCart = null,
                   
                };
                newbooks.Add(newbook);

                SearchBooksByGenreDto resultt = new SearchBooksByGenreDto()
                {
                   
                    Books = newbooks,
                    Genres = await this._bookService.GetAllGenres()
                };

                return View(resultt);
            }


            SearchBooksByGenreDto result = new SearchBooksByGenreDto()
            {
                Books = await this._bookService.SearchByGenres(Genre),
                Genres = await this._bookService.GetAllGenres()
            };

            return View(result);
        }

        // GET: AddBookToCart
        [Authorize(Roles = "Administrator, StandardUser")]
        public IActionResult AddBookToCart(Guid? id)
        {
            var model = this._bookService.GetShoppingCartInfo(id);
            return View(model);
        }

        // POST: AddBookToCart
        [Authorize(Roles = "Administrator, StandardUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookToCart([Bind("BookId", "Quantity")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._bookService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Books");
            }

            return View(item);
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Details2/5
        public IActionResult Details2(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,BookName,BookImage,BookPrice,Genre,Author,DateCreated")] Book book)
        {
            if (ModelState.IsValid)
            {
                this._bookService.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,BookName,BookImage,BookPrice,Genre,Author,DateCreated")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._bookService.UpdateExistingBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._bookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return this._bookService.GetDetailsForBook(id) != null;
        }
    }
}
