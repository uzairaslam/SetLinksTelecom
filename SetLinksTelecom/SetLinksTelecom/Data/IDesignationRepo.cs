using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IDesignationRepo
    {
        IList<Designation> GetData();

        Designation GetDesignation(int id);

        void SaveDesignation(Designation designation);

        void UpdateDesignation(Designation designation);

        void DeleteDesignation(int id);
    }
}
