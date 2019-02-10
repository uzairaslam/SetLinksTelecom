using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.Data
{
    public interface IPurchaseRepo
    {
        IEnumerable<dtoDisplayPurchase> GetData();
        dtoPurchase GetPurchase(int id);
        void SavePurchase(dtoPurchase dtoPurchase);
        void UpdatePurchase(dtoPurchase dtoPurchase);
        void DeletePurchase(int id);
        DtoTangibleItemSale GetSpecificPurchase(int id);
        DtoInTangibleItemSale GetSpecificInTangiblePurchase(int id, int PersonId);
    }
}