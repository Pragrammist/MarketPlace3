using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UserDataBase
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Name { get; set; } = "";
        
        public List<Comment> Comments { get; set; } = new List<Comment>(); // Comment.Book

        [Range(0, 5)]
        public double Rating { get; set; } = 0;
        public int NumRated { get; set; } = 0;
        [StringLength(150)]
        public int CommentNum { get 
            {
                return Comments.Count;
            } 
        }

        public int ReadedNum { get; set; } = 0;

        public int BookBytesId { get; set; } = -1;

        public int ImageBytesId { get; set; } = -1;

        [Range(0, 20000)]
        public int Price { get; set; } = 0;

        [StringLength(150)]
        public string Author { get; set; } = "";
        
        public List<User> Users { get; set; } = new List<User>(); // => User.BookReading + // => User.History.Readed + //User.History.Buyed
        
        public List<ShoppingCard> ShoppingCards { get; set; } // => ShoppingCard.Books

        public List<History> HistoryBought { get; set; }
        public List<History> HistoryRead { get; set; }

        public List<History> HistoryRated { get; set; }
    }
}
