using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDataBase;

namespace HukSleva.ViewModels.BookController
{
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment)
        {
            if (comment.Comments.Count > 0 || comment.Comments != null)
            {
                Comments = new CommentViewModel[comment.Comments.Count];
                for (int i = 0; i > comment.Comments.Count; i++)
                {
                    Comments[i] = new CommentViewModel(comment.Comments[i]);
                }
            }
            NumDislikes = comment.Dislikes.ToString();
            NumLikes = comment.NumLikes.ToString();
            Date = comment.Date.ToString("D");
            Text = comment.Text;
        }
        public CommentViewModel[] Comments { private set; get; }
        public string NumDislikes { get; set; }
        public string NumLikes { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
    }
}
