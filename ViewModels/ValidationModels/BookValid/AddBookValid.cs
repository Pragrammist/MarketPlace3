using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HukSleva.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using HukSleva.ViewModels.ValidationModels.BookValid.Attributes;

namespace HukSleva.ViewModels.ValidationModels.BookValid
{
    public class AddBookValid
    {
        [Required(ErrorMessage = "обязательное поле для вода")]
        [Display(Name = "Имя")]
        [StringLength(300, ErrorMessage = "Название должно быть не больше 300 символов")]
        [Remote("IsNotExist", "ValidBook", AdditionalFields = "Author")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "обязательное поле для вода")]
        [Display(Name = "Цена")]
        [Range(0, 20000, ErrorMessage = "Цена не может быть выше 20000")]
        public int Price { get; set; } = 0;
        [Required(ErrorMessage = "обязательное поле для вода")]
        [Display(Name = "Автор")]
        [StringLength(150, ErrorMessage = "Длина не может быть выше 150")]
        [Remote("IsNotExist", "ValidBook", AdditionalFields = "Name")]
        public string Author { get; set; } = "";

        [DataType(DataType.Upload)]
        [ValidFileExtention(typeValid.img)]
        [Display(Name = "Загрузить фотографию")]
        [MaxLength(2000000, ErrorMessage = "книга слишком большая")]
        public ICollection<IFormFile> Img { get; set; }

        [DataType(DataType.Upload)]
        [ValidFileExtention(typeValid.book)]
        [Display(Name = "Загрузить книгу")]
        [MaxLength(21000000, ErrorMessage = "книга слишком большая")]
        public ICollection<IFormFile> BookBytes { get; set; }

    }
}
