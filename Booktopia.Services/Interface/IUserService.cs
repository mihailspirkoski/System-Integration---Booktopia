using Booktopia.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Services.Interface
{
    public interface IUserService
    {
        List<BooktopiaAppUser> getAllUsers();
    }
}
