using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.GeneralFolder;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepo _personRepo;
        private readonly IDesignationRepo _designationRepo;
        private readonly ILineRepo _lineRepo;

        public PersonController(IPersonRepo personRepo, IDesignationRepo designationRepo, ILineRepo lineRepo)
        {
            _personRepo = personRepo;
            _designationRepo = designationRepo;
            _lineRepo = lineRepo;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int BossId = 0, int DesignationId = 0, string withoutBoss = "")
        {
            IList<Person> persons = _personRepo.GetData(BossId, DesignationId, withoutBoss);
            return Json(new { data = (from P in persons
                select new
                {
                    PersonId = P.PersonId,
                    Name = P.Name,
                    FatherName = P.FatherName,
                    Gender = P.Gender,
                    CNIC = P.CNIC,
                    DOB = P.DOB,
                    DOBFormatted = P.DOBFormatted,
                    CNICIssueDateFormatted = P.CNICIssueDateFormatted,
                    P.MobileBusiness,
                    P.CurrentAddress,
                    P.MobilePersonal,
                    P.PermanentAddress,
                    P.Qualification,
                    Designation = P.Designation.Name
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveBoss(int PersonId = 0)
        {
            _personRepo.RemoveBoss(PersonId);
            return Json(new { success = true, message = "Removed Follower Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonGrid()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                Person person = new Person();
                person.Designations = _designationRepo.GetData().ToList();
                person.Lines = _lineRepo.GetLines().ToList();
                person.CNICIssueDate = DateTime.Now;
                person.DOB = DateTime.Now;
                return View(person);
            }
            else
                return View(_personRepo.GetPerson(id));
        }

        [HttpPost]
        public ActionResult AddOrEdit(Person person)
        {
            try
            {
                //_personRepo.SavePerson(person);
                //return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                if (person.PersonId == 0)
                {
                    _personRepo.SavePerson(person);
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _personRepo.UpdatePerson(person);
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in " + (person.PersonId == 0 ? "saving" : "updation") + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AssignBoss(int BossId, int FollowerId)
        {
            _personRepo.AssignBoss(BossId,FollowerId);
            return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _personRepo.DeletePerson(id);
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error in deletion" + "\n" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PersonMapping()
        {
            DtoPersonMapping mapping = new DtoPersonMapping();
            mapping.Designations = _designationRepo.GetData().ToList();
            return View(mapping);
        }

        [HttpGet]
        public ActionResult UploadExcel()
        {
            List<DtoPersonExcel> list = new List<DtoPersonExcel>();
            return View(list);
        }

        [HttpPost]
        public ActionResult UploadExcel(User users, HttpPostedFileBase FileUpload)
        {

            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {


                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Docs/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    //var ds = new DataSet();

                    //adapter.Fill(ds, "ExcelTable");

                    //DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var artistAlbums = (from a in excelFile.Worksheet<DtoPersonExcel>(sheetName) select a).ToList();
                    //artistAlbums.GetInvalidPersons();
                    //artistAlbums.ToList();

                    foreach (DtoPersonExcel person in artistAlbums)
                    {
                        _personRepo.ValidatePerson(person);
                    }

                    var invalidPersons = artistAlbums.Where(p => p.IsInvalid).ToList();

                    //foreach (var a in artistAlbums)
                    //{
                    //    try
                    //    {
                    //        //if (a.Name != "" && a.Address != "" && a.ContactNo != "")
                    //        //{
                    //        //    DtoPersonExcel TU = new DtoPersonExcel();
                    //        //    TU.Name = a.Name;
                    //        //    TU.Address = a.Address;
                    //        //    TU.ContactNo = a.ContactNo;
                    //        //    db.Users.Add(TU);

                    //        //    db.SaveChanges();



                    //        //}
                    //        //else
                    //        //{
                    //        //    data.Add("<ul>");
                    //        //    if (a.Name == "" || a.Name == null) data.Add("<li> name is required</li>");
                    //        //    if (a.Address == "" || a.Address == null) data.Add("<li> Address is required</li>");
                    //        //    if (a.ContactNo == "" || a.ContactNo == null) data.Add("<li>ContactNo is required</li>");

                    //        //    data.Add("</ul>");
                    //        //    data.ToArray();
                    //        //    return Json(data, JsonRequestBehavior.AllowGet);
                    //        //}
                    //    }

                    //    catch (DbEntityValidationException ex)
                    //    {
                    //        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    //        {

                    //            foreach (var validationError in entityValidationErrors.ValidationErrors)
                    //            {

                    //                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                    //            }

                    //        }
                    //    }
                    //}
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }

                    return View(invalidPersons); //Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}