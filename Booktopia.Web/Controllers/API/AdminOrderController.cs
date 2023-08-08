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
    public class AdminOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<BooktopiaAppUser> userManager;

        public AdminOrderController(IOrderService orderService, UserManager<BooktopiaAppUser> userManager)
        {
            this._orderService = orderService;
            this.userManager = userManager;
        }

        [HttpGet("[action]")]
        public List<Order> GetOrders()
        {
            return this._orderService.getAllOrders();
        }

        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this._orderService.getOrderDetails(model);
        }


    }
}
