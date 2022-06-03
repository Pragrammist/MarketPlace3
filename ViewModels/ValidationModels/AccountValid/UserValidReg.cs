using HukSleva.ViewModels.ValidationModel.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.ViewModels.ValidationModel
{
    
    public class UserValidReg
    {
        [Display(Name = "Ник")]
        [Required(ErrorMessage = "Обязательное поле")]
        [MinLength(3, ErrorMessage = "Минимальная длина 3")]
        [StringLength(25, ErrorMessage = "Максимальная длина 25")]
        [RegularExpression(@"^(?!.*\.\.)(?!\.)(?!.*\.$)(?!\d+$)[a-zA-Z0-9.]{3,25}$", ErrorMessage = "Можно изпользовать только точку, латинские символы и цифры")]
        [Remote("NickNameIsNotExist", "ValidUser", ErrorMessage = "Пользователь уже существует")]
        public string NickName { get; set; } = "";

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{6,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$", ErrorMessage = "Пароль не должен быть простым, максимальная длина 16")]
        public string Password { get; set; }

        [Display(Name = "Повтор пороля")]
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пороли не совподают")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Почта")]
        [EmailAddress(ErrorMessage = "неверный формать ввода")]
        [MaxLength(50, ErrorMessage = "Максимальная длина 50")]
        [Remote("EmailIsNotExist", "ValidUser", ErrorMessage = "Пользователь уже существует")]
        public string Email { get; set; } = "";

        [Display(Name = "Номер телефона")]
        [MaxLength(15, ErrorMessage = "Максимальная длина 15")]
        [Phone(ErrorMessage = "неверный формать ввода")]
        [Remote("PhoneIsNotExist", "ValidUser", ErrorMessage = "Пользователь уже существует")]
        public string PhoneNumber { get; set; } = "";


        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Обязательное поле для ввода")]
        [RangeDateTime(ErrorMessage = "Дата слишком большая или маленькая")]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        [Display(Name = "О себе")]
        [MaxLength(120, ErrorMessage = "Максимальная длина 120")]
        public string SmallDiscription { get; set; } = "";

    }
}
