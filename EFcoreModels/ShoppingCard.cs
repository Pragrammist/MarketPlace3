using System;
using System.Collections.Generic;

namespace UserDataBase
{
    public class ShoppingCard
    {
        public List<Book> Books { get; set; } = new List<Book>(); // => Book.ShoppingCards
        public int Id { get; set; }
        public User User { get; set; } //
        public int UserId { get; set; }
    }
}
