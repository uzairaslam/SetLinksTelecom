using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Data
{
    public interface ISaleRepo
    {
        DtoSale GetSale(int id);
        void SaveTangibleSale(DtoTangibleSale dtoTangibleSale);
        void SaveInTangibleSale(DtoInTangibleSale dtoInTangibleSale);
    }
}
