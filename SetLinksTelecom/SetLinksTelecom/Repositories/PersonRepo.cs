using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        private readonly DataContext _db;
        private readonly ILineRepo _line;

        public PersonRepo(DataContext db, ILineRepo line)
        {
            _db = db;
            _line = line;
        }

        public IList<Person> GetData(int BossId = 0, int DesignationId = 0, string withoutBoss = "")
        {
            if (withoutBoss != String.Empty)
            {
                var personId = Convert.ToInt32(withoutBoss);
                return _db.Persons.Where(p => (p.BossId == null || p.BossId == 0) && p.PersonId != personId)
                    .Include(p => p.Designation).ToList();
            }
            else if (BossId != 0)
                return _db.Persons.Where(p => p.BossId == BossId).Include(p => p.Designation).ToList();
            else if (DesignationId != 0)
                return _db.Persons.Where(p => p.DesignationId == DesignationId).ToList();
            else
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
            //Person per = _db.Persons.Single(p => p.PersonId.Equals(person.PersonId));
            //person.AccString = per.AccString;
            //person.BossId = per.BossId;
            _db.Entry(person).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeletePerson(int id)
        {
            Person person = _db.Persons.FirstOrDefault(p => p.PersonId.Equals(id));
            //_db.Persons.Remove(person);
            //_db.SaveChanges();
        }

        public void AssignBoss(int BossId, int FollowerId)
        {
            Person person = _db.Persons.FirstOrDefault(p => p.PersonId.Equals(FollowerId));
            person.BossId = BossId;
            _db.Entry(person).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void RemoveBoss(int PersonId = 0)
        {
            Person person = _db.Persons.FirstOrDefault(p => p.PersonId.Equals(PersonId));
            person.BossId = null;
            _db.Entry(person).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public bool PhoneExist(string phone)
        {
            return _db.Persons.Any(p => p.MobileBusiness.Substring(p.MobileBusiness.Length - 10).Equals(phone)
                                        || p.MobilePersonal.Substring(p.MobilePersonal.Length - 10).Equals(phone));
        }

        public void ValidatePerson(DtoPersonExcel p)
        {
            p.ErrorMessage = String.Empty;
            Regex cnic = new Regex(@"^[1-4]{1}[0-9]{4}(-)?[0-9]{7}(-)?[0-9]{1}$");

            //^(((\+92)|(0092)|(92))-{0,1})?\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$
            Regex phone = new Regex(@"^(((\+92)|(0092)|(92))-{0,1})?\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$");

            if (string.IsNullOrWhiteSpace(p.Name))
            {
                p.ErrorMessage = p.ErrorMessage + "Name is Required\n";
                p.IsInvalid = true;
            }

            if(!string.IsNullOrWhiteSpace(p.Name) && (p.Name.Length < 3 || p.Name.Length > 30))
            {
                p.ErrorMessage = p.ErrorMessage + "Length of name should \n";
                p.IsInvalid = true;
            }

            if (!cnic.IsMatch(p.CNIC))
            {
                p.ErrorMessage = p.ErrorMessage + "Not Valid CNIC Format: 12345-1234567-1 or 1234512345671\n";
                p.IsInvalid = true;
            }

            if (!phone.IsMatch(p.MobilePersonal))
            {
                p.ErrorMessage = p.ErrorMessage + "Personal Not valid phone number format\n";
                p.IsInvalid = true;
            }

            if (!phone.IsMatch(p.MobileBusiness))
            {
                p.ErrorMessage = p.ErrorMessage + "Business Not valid phone number format\n";
                p.IsInvalid = true;
            }

            if (!p.Gender.ToLower().Equals("Male".ToLower()) && !p.Gender.ToLower().Equals("Female".ToLower()))
            {
                p.ErrorMessage = p.ErrorMessage + "Gender must be Male or Female\n";
                p.IsInvalid = true;
            }

            if (string.IsNullOrWhiteSpace(p.MobilePersonal) || string.IsNullOrWhiteSpace(p.MobileBusiness))
            {
                p.ErrorMessage = p.ErrorMessage + "Mobile Numbers are required\n";
                p.IsInvalid = true;
            }

            if (p.MobileBusiness == p.MobilePersonal)
            {
                p.ErrorMessage = p.ErrorMessage + "Both Mobile numbers can't be the same\n";
                p.IsInvalid = true;
            }

            if (PhoneExist(p.MobilePersonal))
            {
                p.ErrorMessage = p.ErrorMessage + "Person mobile number already exist\n";
                p.IsInvalid = true;
            }

            if (string.IsNullOrWhiteSpace(p.MobileBusiness))
            {
                p.ErrorMessage = p.ErrorMessage + "Business mobile number already exist\n";
                p.IsInvalid = true;
            }

            if (!string.IsNullOrWhiteSpace(p.BusinessLine) && !string.IsNullOrWhiteSpace(p.PersonalLine) && p.PersonalLine == p.BusinessLine)
            {
                p.ErrorMessage = p.ErrorMessage + "Business and personal Line can't be the same\n";
                p.IsInvalid = true;
            }

            if (!_line.LineExist(p.BusinessLine))
            {
                p.ErrorMessage = p.ErrorMessage + "Business Line Doesn't Exist\n";
                p.IsInvalid = true;
            }

            if (!_line.LineExist(p.PersonalLine))
            {
                p.ErrorMessage = p.ErrorMessage + "Business line Doesn't Exist\n";
                p.IsInvalid = true;
            }

            //if (!string.IsNullOrWhiteSpace(p.Name) && p.Name.Length >= 3 && p.Name.Length <= 30
            //    && p.FatherName.Length <= 30 && cnic.IsMatch(p.CNIC) &&
            //    (p.Gender.Equals("Male") || p.Gender.Equals("Female"))
            //    && !string.IsNullOrWhiteSpace(p.MobilePersonal) && !string.IsNullOrWhiteSpace(p.MobileBusiness)
            //    && p.MobileBusiness != p.MobilePersonal && PhoneExist(p.MobilePersonal)
            //    && PhoneExist(p.MobileBusiness) && _line.LineExist(p.BusinessLine)
            //    && _line.LineExist(p.PersonalLine))
            //{
            //    p.IsInvalid = false;
            //    p.ErrorMessage = String.Empty;
            //}
            //else
            //{
            //    p.IsInvalid = true;
            //    //p.ErrorMessage = "Error in field \nname required";
            //    //p.ErrorMessage = "Error in field "+ Environment.NewLine+ "name required";
            //}
        }
    }
}