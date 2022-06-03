using System;
using System.Collections.Generic;

namespace UserDataBase
{
    public class History
    {
        public int Id { get; set; }
        public List<Book> Bought { get; set; } = new List<Book>(); // => Book.Users
        public List<Comment> Wrote { get; set; } = new List<Comment>(); //=> Comment.User
        public List<Book> Read { get; set; } = new List<Book>(); // => Book.Users
        public User User { get; set; } //
        public List<Book> Reated { get; set; }
        public int UserId { get; set; }
    }
}
