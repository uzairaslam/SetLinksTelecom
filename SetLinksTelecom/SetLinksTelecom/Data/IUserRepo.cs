using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Data
{
    public interface IUserRepo
    {
        DtoUserLogin Login(DtoUserLogin dtoUser);
    }
}
