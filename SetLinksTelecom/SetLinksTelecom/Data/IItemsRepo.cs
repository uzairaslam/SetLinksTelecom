using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IItemsRepo
    {
        IList<Item> GetData();
        Item GetItem(int id);

        void SaveItem(Item item);

        void UpdateItem(Item item);
        void DeleteItem(int id);
    }
}
