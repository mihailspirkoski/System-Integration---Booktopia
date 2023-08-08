using Booktopia.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<BooktopiaAppUser> GetAll();
        List<BooktopiaAppUser> GetAllUsers();
        BooktopiaAppUser Get(string id);
        void Insert(BooktopiaAppUser entity);
        void Update(BooktopiaAppUser entity);
        void Delete(BooktopiaAppUser entity);
    }
}
