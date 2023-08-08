using Booktopia.Domain.Identity;
using Booktopia.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booktopia.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<BooktopiaAppUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<BooktopiaAppUser>();
        }
        public IEnumerable<BooktopiaAppUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public List<BooktopiaAppUser> GetAllUsers()
        {
            return entities.ToList();
        }

        public BooktopiaAppUser Get(string id)
        {
            return entities
                .Include(z => z.UserCart)
                .Include("UserCart.BooksInShoppingCart")
                .Include("UserCart.BooksInShoppingCart.Book")
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(BooktopiaAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(BooktopiaAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(BooktopiaAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        
    }
}
