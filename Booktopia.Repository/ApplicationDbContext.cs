using Booktopia.Domain.DomainModels;
using Booktopia.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BooktopiaAppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<BookInShoppingCart> BooksInShoppingCart { get; set; }
        public virtual DbSet<BookInOrder> BooksInOrder { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
               .Property(z => z.Id)
               .ValueGeneratedOnAdd();

            builder.Entity<BookInShoppingCart>()
                .HasOne(z => z.Book)
                .WithMany(z => z.BooksInShoppingCart)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<BookInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.BooksInShoppingCart)
                .HasForeignKey(z => z.BookId);

            builder.Entity<ShoppingCart>()
                .HasOne<BooktopiaAppUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);


            builder.Entity<BookInOrder>()
                .HasOne(z => z.OrderedBook)
                .WithMany(z => z.BooksInOrder)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<BookInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.BooksInOrder)
                .HasForeignKey(z => z.BookId);

        }
    }
}
