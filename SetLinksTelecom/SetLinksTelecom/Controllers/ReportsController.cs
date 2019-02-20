using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.GeneralFolder;

namespace SetLinksTelecom.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReports _repo;

        public ReportsController(IReports repo)
        {
            _repo = repo;
        }

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Ledger()
        {
            DtoLedger ledger = new DtoLedger();
            ledger.StartDate = DateTime.Now;
            ledger.EndDate = DateTime.Now;
            return View(ledger);
        }

        [HttpPost]
        public ActionResult Ledger(DtoLedger ledger)
        {
            ledger.EndDate = ledger.EndDate.AddDays(1);
            DataTable Ledgers = new DataTable();
            Ledgers = _repo.GetLedgers(ledger);
            Ledgers.TableName = "Ledger";

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);


            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Ledger.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Ledger", Ledgers));

            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Ledger";
            return View("_ReportView");
        }
    }
}