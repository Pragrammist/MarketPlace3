using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.ViewModels.ValidationModels.BookValid.Attributes
{
    public enum typeValid {book, img }
    public class ValidFileExtentionAttribute : ValidationAttribute
    {

        private string[] exts;
        public ValidFileExtentionAttribute(typeValid typeValid)
        {
            ErrorMessage = "не верный формат";
            if (typeValid == typeValid.book)
            {
                exts = new string[] { "fb2", "epub", "mobi", "kf8", "djvu", "lrf", "azw", "txt" };
            }
            else
            {
                exts = new string[] { "jpg", "gif", "png" };
            }

        }

        public override bool IsValid(object value)
        {
            var files = value as IEnumerable<IFormFile>;

            if (files == null)
            {
                return false;
            }

            for (int i = 0; i < files.Count(); i++)
            {
                var file = files.ElementAt(i);
                var reverseStr = new string(file.FileName.Reverse().ToArray());
                var ext = new string(reverseStr.TakeWhile(sym => sym != '.').Reverse().ToArray());
                bool isTrue = exts.Contains(ext);
                if (isTrue)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
