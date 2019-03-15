using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IAccVoucher
    {
        List<DtoVoucherDisplay> GetData();
        DtoJvEntry GetJvEntry();
        void SaveJv(DtoSaveJv dtoSaveJv);
    }
}
