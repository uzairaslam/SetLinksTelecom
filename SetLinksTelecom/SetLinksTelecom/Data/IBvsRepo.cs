using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IBvsRepo
    {
        IList<BvsService> GetData();
        BvsService GetBVS(int id);
        void Save(BvsService bvsService);
    }
}
