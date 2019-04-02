using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public Line GetLine(int id)
        {
            return _db.Lines.FirstOrDefault(l => l.LineId.Equals(id));
        }

        public void SaveLine(Line line)
        {
            _db.Lines.Add(line);
            _db.SaveChanges();
        }

        public void UpdateLine(Line line)
        {
            _db.Entry(line).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public bool LineExist(string name)
        {
            return _db.Lines.Any(l => l.Name.ToLower().Equals(name.ToLower()));
        }
    }
}