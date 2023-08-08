using Booktopia.Domain.DomainModels;
using Booktopia.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktopia.Repository.Implementation
{
    public class BookkRepository : IBookkRepository
    {

        private readonly ApplicationDbContext context;

        public BookkRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            return await context.Books.ToListAsync();
        }

        public async Task<List<string>> GetAllGenres()
        {
            List<string> result = new List<string>();

            foreach (var book in await this.GetAll())
            {
                if (!result.Contains(book.Genre))
                    result.Add(book.Genre);
            }

            return result;
        }

        public async Task<List<Book>> SearchByGenres(string Genre)
        {
            return await context.Books
                .Where(x => x.Genre.Equals(Genre)).ToListAsync();
        }
    }
}
