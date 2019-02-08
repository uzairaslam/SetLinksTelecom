using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IInventoryType
    {
        List<InventoryType> GetData();
        InventoryType GetInventoryType(int id);
        void SaveInventoryType(InventoryType inventoryType);
        void UpdateInventoryType(InventoryType inventoryType);
    }
}
