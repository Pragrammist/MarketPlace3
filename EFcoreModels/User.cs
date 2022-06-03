using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace UserDataBase
{

     
    public class User
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string NickName { get; set; } 

        public int Password { get; set; } 

        [MaxLength(50)]
        public string Email { get; set; } 

        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }

        [MaxLength(120)]
        public string SmallDiscription { get; set; } = "";

        public Book Reading { get; set; }// => Book.Users

        public ShoppingCard ShoppingCard { get; set; }//
        public History History { get; set; }//
        [Range(0, 20000)]
        public int Money { get; set; }
        public Role Role { get; set; } = Role.user;
        public int GetHashCode(string str)
        {
            int res = -1;
            try
            {
                using (SHA256 sHA = SHA256.Create())
                {
                    var bs = Encoding.UTF8.GetBytes(str);
                    var hashBs = sHA.ComputeHash(bs);
                    res = BitConverter.ToInt32(hashBs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }
    }
}
