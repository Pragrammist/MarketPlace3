using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HukSleva.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HukSleva.ViewModels.ValidationModel
{
    
    public class UserValidAutho
    {
        [Display(Name = "Введите ник, номер телефона или почту")]
        [Required(ErrorMessage = "Обязательное поле")]
        [Remote("IsExit", "ValidUser", ErrorMessage = "Пользователь не найден")]
        
        public string AllPurposeField { get; set; }


        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{6,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$", ErrorMessage = "Пароль не может быть простым, максимальная длина 16")]
        public string Password { get; set; }


        public FieldType GetFieldType(string value)
        {
            EmailAddressAttribute emailAddress = new EmailAddressAttribute();
            PhoneAttribute phoneAttribute = new PhoneAttribute();

            if (phoneAttribute.IsValid(value))
            {
                return FieldType.phone;
            }
            else if (emailAddress.IsValid(value))
            {
                return FieldType.email;
            }
            else
            {
                return FieldType.nickname;
            }
        }
    }
}
