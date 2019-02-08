using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface IProductCategoryRepo
    {
        List<DtoProductCategory> GetData();
        ProductCategory GetProductCategory(int id);
        void SaveProductCategory(ProductCategory productCategory);
        void UpdateProductCategory(ProductCategory productCategory);
    }
}
