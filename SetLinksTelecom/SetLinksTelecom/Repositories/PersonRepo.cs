using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        private readonly DataContext _db;

        public PersonRepo(DataContext db)
        {
            _db = db;
        }

        public IList<Person> GetData()
        {
            return _db.Persons.Include(p => p.Designation).ToList();
        }

        public Person GetPerson(int id)
        {
            var person = _db.Persons.FirstOrDefault(d => d.PersonId.Equals(id));
            person.Designations = _db.Designations.ToList();
            person.Lines = _db.Lines.ToList();
            return person;
        }

        public void SavePerson(Person person)
        {
            Designation designation = _db.Designations.Single(d => d.Id.Equals(person.DesignationId));
            var head = _db.AccHead.Single(h => h.HeadCode.Equals(13));
            var subhead = head.SubHeads.Single(s => s.SubHeadString == designation.AccString);
            var maxAcc = _db.AccAccounts.Where(acc => acc.HeadCode == head.HeadCode && acc.SubHeadCode == subhead.SubHeadCode)
                             .Max(a => (int?)a.AccCode) ?? 0;
            maxAcc = ++maxAcc;
            _db.AccAccounts.Add(new AccAccount
            {
                HeadCode = 13,
                SubHeadCode = subhead.SubHeadCode,
                OID = 0,
                AccCode = maxAcc,
                AccMade = 1,
                AccName = person.Name,
                AccString = subhead.SubHeadString + "-" + (maxAcc.ToAccString())
            });
            person.AccString = subhead.SubHeadString + "-" + (maxAcc.ToAccString());
            _db.Persons.Add(person);
            _db.SaveChanges();
        }

        public void UpdatePerson(Person person)
        {
            _db.Entry(person).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeletePerson(int id)
        {
            Person person = _db.Persons.FirstOrDefault(p => p.PersonId.Equals(id));
            _db.Persons.Remove(person);
            _db.SaveChanges();
        }
    }
}