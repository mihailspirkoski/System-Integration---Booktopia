using Booktopia.Domain.Identity;
using Booktopia.Repository.Interface;
using Booktopia.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public List<BooktopiaAppUser> getAllUsers()
        {
            return this._userRepository.GetAllUsers();
        }
    }
}
