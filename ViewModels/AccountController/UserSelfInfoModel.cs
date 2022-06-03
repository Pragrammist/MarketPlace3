using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserDataBase;

namespace HukSleva.ViewModels
{
    public class UserSelfInfoModel
    {
        //bool _hideInfo = false;

        public UserSelfInfoModel(int id, UserDb userDb)
        {
            InitializateModelByIdFromDb(id, userDb);
        }
        public UserSelfInfoModel(User user)
        {
            InitializateModelByUserDbModel(user);
        }
        
        public UserSelfInfoModel(ClaimsPrincipal user)
        {

            InitializateModelByClaimPrincipal(user);

        }
        private void InitializateModelByClaimPrincipal(ClaimsPrincipal user)
        {
            BirthDate = user.Claims.FirstOrDefault(c => c.Type == "dateBirthday")?.Value ?? "";
            Role = user.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            SmallDiscription = user.Claims.FirstOrDefault(c => c.Type == "smallDiscription")?.Value ?? "";
            Nickname = user.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "";
            Email = user.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            PhoneNumber = user.Claims.FirstOrDefault(c => c.Type == "phoneNumber")?.Value ?? "";
            BookReadingId = user.Claims.FirstOrDefault(c => c.Type == "readingBookId")?.Value ?? "";
            ShopingCardId = user.Claims.FirstOrDefault(c => c.Type == "shoppingCardId")?.Value ?? ""; 
            HistoryId = user.Claims.FirstOrDefault(c => c.Type == "historyId")?.Value ?? ""; 
            Id = user.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "";
            Money = user.Claims.FirstOrDefault(c => c.Type == "money")?.Value ?? "";

        }

        private void InitializateModelByUserDbModel(User user)
        {
            Nickname = user?.NickName ?? "";
            Role = user?.Role.ToString() ?? "";
            SmallDiscription = user?.SmallDiscription ?? "";
            BirthDate = user?.BirthDate.ToString("d") ?? "";
            Email = user?.Email.ToString() ?? "";
            PhoneNumber = user?.PhoneNumber.ToString() ?? "";
            BookReadingId = user?.Reading?.Id.ToString() ?? "";
            ShopingCardId = user?.ShoppingCard?.Id.ToString() ?? "";
            HistoryId = user?.History?.Id.ToString() ?? "";
            Money = user?.Money.ToString() ?? "";
            BookReadingId = user?.Reading.Id.ToString();
        }
        private void InitializateModelByIdFromDb(int id, UserDb userDb)
        {
            var user = userDb.Users.Include(u => u.Reading).FirstOrDefault(u => u.Id == id);
            InitializateModelByUserDbModel(user);
        }
        public UserSelfInfoModel()
        {
            //_hideInfo = hideInfo;
        }

        public string Id { get; set; }
        public string Nickname { get; set; } = "";
        public string Role { get; set; } = "";
        public string SmallDiscription { get; set; } = "";
        public string BirthDate { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string BookReadingId { get; set; } = "";
        public string ShopingCardId { get; set; } = "";
        public string HistoryId { get; set; } = "";
        public string Money { get; set; } = "";
        //public bool Hiden { get => _hideInfo; }
    }
}
