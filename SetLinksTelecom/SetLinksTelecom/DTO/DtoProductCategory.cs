using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoProductCategory
    {
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int InventoryTypeId { get; set; }
        public string InventoryTypeName { get; set; }
    }
}