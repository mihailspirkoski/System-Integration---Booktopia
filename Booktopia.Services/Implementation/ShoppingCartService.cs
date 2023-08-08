using Booktopia.Domain.DomainModels;
using Booktopia.Domain.DTO;
using Booktopia.Repository.Interface;
using Booktopia.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booktopia.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<BookInOrder> _bookInOrderRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository,
            IUserRepository userRepository, IRepository<Order> orderRepository,
            IRepository<BookInOrder> bookInOrderRepository,
            IRepository<EmailMessage> mailRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _bookInOrderRepository = bookInOrderRepository;
            _mailRepository = mailRepository;
        }

        
        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);



            var userShoppingCart = loggedInUser.UserCart;

            var allBooks = userShoppingCart.BooksInShoppingCart.ToList();

            var allBooksPrice = allBooks.Select(z => new
            {
                BookPrice = z.Book.BookPrice,
                Quantity = z.Quantity
            }).ToList();

            float totalPrice = 0;

            foreach (var item in allBooksPrice)
            {
                totalPrice += (item.Quantity * item.BookPrice);
            }

            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Books = allBooks,
                TotalPrice = totalPrice
            };


            return scDto;
        }


        public bool deleteBookFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.BooksInShoppingCart.Where(z => z.BookId.Equals(id)).FirstOrDefault();

                userShoppingCart.BooksInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId

                };

                this._orderRepository.Insert(order);

                List<BookInOrder> booksInOrder = new List<BookInOrder>();

                var result = userShoppingCart.BooksInShoppingCart.Select(z => new BookInOrder
                {
                    Id = Guid.NewGuid(),
                    BookId = z.Book.Id,
                    OrderedBook = z.Book,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();


                booksInOrder.AddRange(result);


                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order contains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    totalPrice += (item.Quantity * item.OrderedBook.BookPrice);

                    sb.AppendLine(i.ToString() + ". " + item.OrderedBook.BookName + " with price of: " + item.OrderedBook.BookPrice + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();



                foreach (var item in booksInOrder)
                {
                    this._bookInOrderRepository.Insert(item);
                }



                loggedInUser.UserCart.BooksInShoppingCart.Clear();

                this._mailRepository.Insert(mail);
                this._userRepository.Update(loggedInUser);



                return true;
            }
            return false;
        }

    }
}
