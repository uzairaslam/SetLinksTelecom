using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class BvsRepo : IBvsRepo
    {
        private readonly DataContext _db;

        public BvsRepo(DataContext db)
        {
            _db = db;
        }

        public BvsService GetBVS(int id)
        {
            return _db.BvsServices.FirstOrDefault(b => b.BvsServiceId.Equals(id));
        }

        public void Save(BvsService bvsService)
        {
            if (bvsService.BvsServiceId == 0)
            {
                _db.BvsServices.Add(bvsService);
            }
            else
            {
                _db.Entry(bvsService).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public List<DtoBvsAllotment> GetAllotedServices(int PersonId, int ItemId = 0)
        {
            //var alloted = 
                return (from bs in _db.BvsServices
                join B in (
                        (from bas in _db.BvsAllotServices
                            join bvs in _db.BvsAllots on bas.BVSAllotId equals bvs.BVSAllotId
                        join p in _db.Purchases on bvs.ItemId equals p.PurchaseId
                        join i in _db.Items on p.ItemId equals i.ItemId 
                            where
                                bvs.PersonId == PersonId
                            select new
                            {
                                bas.BvsServiceId,
                                bas.BVSAllotServiceId,
                                ItemId = p.ItemId,
                                ItemName = i.Name
                            })) on new {BvsServiceId = bs.BvsServiceId} equals new {BvsServiceId = (int) B.BvsServiceId}
                    into B_join
                from B in B_join.DefaultIfEmpty()
                    select new DtoBvsAllotment
                    {
                        ItemId = (int?)B.ItemId ?? 0,
                        ItemName = B.ItemName ?? "",
                        AllotmentServiceses = new List<DtoBvsAllotmentServices>
                        {
                           new DtoBvsAllotmentServices
                           {
                                   BVSAllotServiceId = (int?)B.BVSAllotServiceId ?? 0,
                               BvsServiceId = bs.BvsServiceId,
                                   BvsServiceName = bs.Name,
                                   Active =
                                       B.BVSAllotServiceId == null ? false : true
                           }
                        }
                    }
                //select new DtoBvsAllotmentServices
                //{
                //    BVSAllotServiceId = (int?)B.BVSAllotServiceId,
                //    BvsServiceName = bs.Name,
                //    Active =
                //        B.BVSAllotServiceId == null ? false : true
                //}
                    ).ToList();
            //DtoBvsAllotment allotment = new DtoBvsAllotment();
            //foreach (DtoBvsAllotment bvs in alloted)
            //{
            //    allotment.ItemId = 
            //}
        }

        public void BvsAllotment(DtoBvsAllotment allotment)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                BVSAllot allot = _db.BvsAllots.FirstOrDefault(b => b.PersonId.Equals(allotment.PersonId));
                if (allot != null)
                {
                    _db.BvsAllots.Remove(allot);
                    _db.SaveChanges();

                    _db.BvsAllotServices.RemoveRange(
                        _db.BvsAllotServices.Where(b => b.BVSAllotId.Equals(allot.BVSAllotId)));
                    _db.SaveChanges();
                }


                BVSAllot alloted = new BVSAllot
                {
                    ItemId = allotment.ItemId ?? 0,
                    PersonId = allotment.PersonId
                };
                _db.BvsAllots.Add(alloted);
                _db.SaveChanges();

                //List<BVSAllotService> dbServices = _db.

                foreach (DtoBvsAllotmentServices services in allotment.AllotmentServiceses)
                {
                    if (services.Active)
                    {
                        BVSAllotService service = new BVSAllotService
                        {
                            BVSAllotId = alloted.BVSAllotId,
                            BvsServiceId = services.BvsServiceId
                        };

                        _db.BvsAllotServices.Add(service);
                        _db.SaveChanges();
                    }
                }
                transaction.Commit();
            }
        }

        public IList<BvsService> GetData()
        {
            return _db.BvsServices.ToList();
        }
    }
}