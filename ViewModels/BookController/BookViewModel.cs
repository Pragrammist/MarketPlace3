using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDataBase;

namespace HukSleva.ViewModels.BookController
{
    public class BookViewModel
    {

        Book _book;
        byte[] img;
        public BookViewModel(Book book, byte[] f)
        {
            _book = book;
            RefreshImg(f);
            Refresh(_book);
        }
        public void Refresh(Book book)
        {
            Comments = new CommentViewModel[book.CommentNum];
            for (int i = 0; i < book.CommentNum; i++)
            {
                Comments[i] = new CommentViewModel(book.Comments[i]);
            }
            
            Author = book.Author;
            Name = book.Name;
            Price = book.Price.ToString();
            Rating = Math.Round(book.Rating, 2).ToString(); 
            Id = book.Id;
            NumRated = book.NumRated.ToString();
        }
        public void RefreshImg(byte[] bs)
        {
            img = bs;
        }
        public string Author { get; set; }
        byte[] _bytes = null;
        byte[] _img = null;
        public byte[] Bytes { get 
            {
                if (_bytes == null)
                {
                    _bytes = null;
                }
                return _bytes;
            } 
        }
        public byte[] Img { get 
            {
                if (_img == null)
                {
                    _img = img;
                }
                return _img;
            } 
        }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Rating { get; set; }
        public int Id { set; get; }
        public CommentViewModel[] Comments { get; private set; }
        string _imgSrc = "";
        public string ImgSrc
        {
            get
            {
                if(_imgSrc == "")
                {
                    var base64 = Convert.ToBase64String(Img ?? new byte[] { });
                    var imgSrc = String.Format("data:image;base64,{0}", base64);
                    _imgSrc = imgSrc;
                }

                return _imgSrc;
            }
        }
        public string NumRated { get; set; }
    }
}
