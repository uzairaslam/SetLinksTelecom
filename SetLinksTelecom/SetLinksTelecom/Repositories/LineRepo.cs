using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class LineRepo : ILineRepo
    {
        private readonly DataContext _db;

        public LineRepo(DataContext db)
        {
            _db = db;
        }
        public IList<Line> GetLines()
        {
            return _db.Lines.ToList();
        }
    }
}