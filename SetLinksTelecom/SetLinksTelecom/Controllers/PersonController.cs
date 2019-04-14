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

                    

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    excelFile.AddMapping<DtoPersonExcel>(x => x.Name, "Name");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.FatherName, "Father Name");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.CNIC, "CNIC");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.CNICIssueDate, "CNIC Issue Date");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.DOB, "Date of Birth");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.Designation, "Designation");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.Gender, "Gender");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.Qualification, "Qualification");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.CurrentAddress, "Current Address");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.PermanentAddress, "Permanent Address");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.MobileBusiness, "Mobile Bussiness");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.BusinessLine, "Business Line");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.MobilePersonal, "Mobile Personal");
                    excelFile.AddMapping<DtoPersonExcel>(x => x.PersonalLine, "Personal Line");
                    string sheetName = excelFile.GetWorksheetNames().First();
                    var artistAlbums = (from a in excelFile.Worksheet<DtoPersonExcel>(sheetName) select a).ToList();
                    //var persons = _personRepo.GetData();
                    //artistAlbums.GetInvalidPersons();
                    //artistAlbums.ToList();

                    foreach (DtoPersonExcel person in artistAlbums)
                    {
                        _personRepo.ValidatePerson(person);
                    }

                    var invalidPersons = artistAlbums.Where(p => p.IsInvalid).ToList();
                      
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }

                    if (invalidPersons.Count > 0)
                    {
                        return View(invalidPersons); //Json("success", JsonRequestBehavior.AllowGet);   
                    }
                    else
                    {
                        foreach (DtoPersonExcel dtoPerson in artistAlbums)
                        {
                            try
                            {
                                var businessLine =
                                    !string.IsNullOrWhiteSpace(dtoPerson.BusinessLine) //&&
                                    //_lineRepo.LineExist(dtoPerson.BusinessLine)
                                        ? _lineRepo.GetLineId(dtoPerson.BusinessLine)
                                        : 0;

                                var personalLine =
                                    !string.IsNullOrWhiteSpace(dtoPerson.PersonalLine) //&&
                                    //_lineRepo.LineExist(dtoPerson.PersonalLine)
                                        ? _lineRepo.GetLineId(dtoPerson.PersonalLine)
                                        : 0;
                                var designation =
                                    !string.IsNullOrWhiteSpace(dtoPerson.Designation) //&&
                                    //_lineRepo.LineExist(dtoPerson.Designation)
                                        ? _designationRepo.GetDesignationId(dtoPerson.Designation)
                                        : 0;
                                Person person = new Person
                                {
                                    Name = dtoPerson.Name,
                                    FatherName = dtoPerson.FatherName,
                                    CNIC = dtoPerson.CNIC,
                                    MobileBusiness = dtoPerson.MobileBusiness,
                                    Gender = dtoPerson.Gender,
                                    DOB = dtoPerson.DOB,
                                    CNICIssueDate = dtoPerson.CNICIssueDate,
                                    CurrentAddress = dtoPerson.CurrentAddress,
                                    PermanentAddress = dtoPerson.PermanentAddress,
                                    BusinessLineMap = businessLine,
                                    MobilePersonal = dtoPerson.MobilePersonal,
                                    PersonalLineMap = personalLine,
                                    Qualification = dtoPerson.Qualification,
                                    DesignationId = designation
                                };
                                try
                                {
                                    _personRepo.SavePerson(person);
                                }
                                catch (DbEntityValidationException ex)
                                {
                                    // Retrieve the error messages as a list of strings.
                                    var errorMessages = ex.EntityValidationErrors
                                        .SelectMany(x => x.ValidationErrors)
                                        .Select(x => x.ErrorMessage);

                                    // Join the list to a single string.
                                    var fullErrorMessage = string.Join(" ", errorMessages);

                                    // Combine the original exception message with the new one.
                                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ",
                                        fullErrorMessage);

                                    // Throw a new DbEntityValidationException with the improved exception message.
                                    //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                                    dtoPerson.IsInvalid = true;
                                    dtoPerson.ErrorMessage = exceptionMessage;
                                    invalidPersons.Add(dtoPerson);
                                }
                                catch (Exception ex)
                                {
                                    dtoPerson.IsInvalid = true;
                                    dtoPerson.ErrorMessage = ex.Message;
                                    invalidPersons.Add(dtoPerson);
                                }
                            }
                            catch (Exception e)
                            {
                                dtoPerson.IsInvalid = true;
                                dtoPerson.ErrorMessage = e.Message;
                                invalidPersons.Add(dtoPerson);
                            }
                        }

                        return View(invalidPersons);
                    }
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
                List<DtoPersonExcel> list = new List<DtoPersonExcel>();
                return View(list);
            }
        }
    }
}