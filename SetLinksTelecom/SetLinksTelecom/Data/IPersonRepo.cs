using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IPersonRepo
    {
        IList<Person> GetData(int BossId = 0, int DesignationId = 0);
        Person GetPerson(int id);
        void SavePerson(Person person);
        void UpdatePerson(Person person);
        void DeletePerson(int id);
    }
}
