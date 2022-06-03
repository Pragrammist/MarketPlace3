using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace UserDataBase
{
    public enum fileType {book, img }
   
    public class UserFile
    {
        protected string GetRandomName(int length)
        {
            string symbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            

            string res = "";
            for (int i = 0; i < length; i++)
            {
                var rNum = RandomNumberGenerator.GetInt32(symbols.Length);
                res += symbols[rNum];
            }
            return res;
        }
        public int Id { get; set; }
        protected string name;
        public string Name { 
            get 
            {
                return name;
            } 
            set 
            {
                var lrName = 10;
                name = GetRandomName(lrName);
            } 
        }

        [MaxLength(150000000)]
        public byte[] Bytes { get; set; }
        public string Extention { get; set; }
        public fileType FileType { get; set; }
    }
}
