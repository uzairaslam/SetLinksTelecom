using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class SaleRepo : ISaleRepo
    {
        private readonly DataContext _db;

        public SaleRepo(DataContext db)
        {
            _db = db;
        }

        public DtoSale GetSale(int id)
        {
            DtoSale dtoSale = new DtoSale();
            return dtoSale;
        }

        public void SaveTangibleSale(DtoTangibleSale dtoTangibleSale)
        {
            Sale sale = new Sale
            {
                PersonId = dtoTangibleSale.PersonId,
                Date = dtoTangibleSale.Date,
                OverAllTotal = dtoTangibleSale.OverAllTotal
            };
            sale.SaleDetails = new List<SaleDetail>();
            dtoTangibleSale.ItemSales.ForEach(dt =>
            {
                sale.SaleDetails.Add(new SaleDetail
                {
                    Rate = dt.Rate,
                    PurchaseId = dt.PurchaseId,
                    Qty = dt.Qty,
                    LineId = 0,
                    SubTotal = dt.SubTotal
                });
            });
            _db.Sales.Add(sale);
            _db.SaveChanges();
        }
        public void SaveInTangibleSale(DtoInTangibleSale dtoInTangibleSale)
        {
            Sale sale = new Sale
            {
                PersonId = dtoInTangibleSale.PersonId,
                Date = dtoInTangibleSale.Date,
                OverAllTotal = dtoInTangibleSale.OverAllTotal
            };
            sale.SaleDetails = new List<SaleDetail>();
            dtoInTangibleSale.ItemSales.ForEach(dt =>
            {
                sale.SaleDetails.Add(new SaleDetail
                {
                    Rate = dt.Rate,
                    PurchaseId = dt.PurchaseId,
                    Qty = dt.Qty,
                    LineId = dt.LineId,
                    SubTotal = dt.SubTotal,
                    CommProfit = (((dt.Rate * dt.Qty) / 100) * dt.Rate)/100
                });
            });
            _db.Sales.Add(sale);
            _db.SaveChanges();
        }
    }
}