using Booktopia.Domain.DomainModels;
using Booktopia.Domain.Identity;
using Booktopia.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booktopia.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly UserManager<BooktopiaAppUser> userManager;

        public AdminUserController(UserManager<BooktopiaAppUser> userManager,
           IUserService userService)
        {
            this._userService = userService;
            this.userManager = userManager;
        }


        [HttpGet("[action]")]
        public List<BooktopiaAppUser> getUsers()
        {
            return this._userService.getAllUsers();
        }


    }
}
