using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _db;

        public UserRepo(DataContext db)
        {
            _db = db;
        }

        public DtoUserLogin Login(DtoUserLogin dtoUser)
        {
            User user = _db.Users.FirstOrDefault(u =>
                u.Username.Equals(dtoUser.Username) && u.Password.Equals(dtoUser.Password));
            dtoUser.ErrorMessage = user == null ? "Wrong username or Password" : String.Empty;
            return dtoUser;
            //if (user == null)
            //{
            //    dtoUser.ErrorMessage = "Wrong username or Password";
            //    return dtoUser;
            //}
        }
    }
}