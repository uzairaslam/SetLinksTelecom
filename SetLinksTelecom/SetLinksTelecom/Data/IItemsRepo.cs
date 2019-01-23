using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IItemsRepo
    {
        IEnumerable<dtoItem> GetData();
        Item GetItem(int id);

        void SaveItem(Item item);

        void UpdateItem(Item item);
        void DeleteItem(int id);
    }
}
