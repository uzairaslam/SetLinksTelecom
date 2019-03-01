using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class dtoItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int ItemCode { get; set; }
        public string Subname { get; set; }
        public decimal SaleRate { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int InventoryTypeId { get; set; }
        public string InventoryTypeName { get; set; }
    }
}