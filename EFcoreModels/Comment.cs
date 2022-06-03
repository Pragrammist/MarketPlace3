using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserDataBase
{
    public class Comment
    {
        public int Id { get; set; }

        [MaxLength(1000)]
        [MinLength(5)]
        public string Text { get; set; } = "";

        public History History { get; set; }//=> User.history.Writed

        public DateTime Date { get; set; } = DateTime.Today;

        public int NumLikes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public Book Book { get; set; } //=> Book.Comments
        public int BookId { get; set; }
    }
}
