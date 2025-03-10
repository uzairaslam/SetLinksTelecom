﻿using System;
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
            DtoLedger ledger = new DtoLedger {StartDate = DateTime.Now, EndDate = DateTime.Now};
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

        [HttpGet]
        public ActionResult Voucher()
        {
            DtoVoucher voucher = new DtoVoucher {StartDate = DateTime.Now, EndDate = DateTime.Now};
            return View(voucher);
        }

        [HttpPost]
        public ActionResult Voucher(DtoVoucher voucher)
        {
            voucher.EndDate = voucher.EndDate.AddDays(1);
            var vouchers = _repo.GetVouchers(voucher);
            vouchers.TableName = "Ledger";
            //return Json(new {data = Ledgers}, JsonRequestBehavior.AllowGet);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);


            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Voucher.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Voucher", vouchers));

            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Ledger";
            return View("_ReportView");
        }

        [HttpGet]
        public ActionResult TrailBalance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TrailBalance(int i=0)
        {
            DataTable TrailBal = _repo.GetTrailBalance();
            TrailBal.TableName = "DSTrailBalance";
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\TrailBalance.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DSTrailBalance", TrailBal));
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Trail Balance";
            return View("_ReportView");
        }

        [HttpGet]
        public ActionResult BalanceSheet()
        {
            return View();
        }


        [HttpPost]
        public ActionResult BalanceSheet(int i = 0)
        {
            DataTable BalSheet = _repo.GetBalanceSheet();
            BalSheet.TableName = "DSBalanceSheet";
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Reset();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\BalanceSheet.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DSBalanceSheet", BalSheet));
            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            reportViewer.LocalReport.Refresh();
            
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Balance Sheet";
            return View("_ReportView");
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Clear();
            DataTable SBalanceSheet = _repo.GetSummaryBalanceSheet();
            SBalanceSheet.TableName = "DSsumBalSheet";
            //ReportDataSource datasource = new ReportDataSource("DSsumBalSheet", SBalanceSheet);
            e.DataSources.Add(new ReportDataSource("DSsumBalSheet", SBalanceSheet));
            //e.DataSources.Add(datasource);
        }

        [HttpGet]
        public ActionResult SumBalSheet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SumBalSheet(int i = 0)
        {
            //SummaryBalanceSheet
            DataTable SBalSheet = _repo.GetSummaryBalanceSheet();
            SBalSheet.TableName = "DSsumBalSheet";
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SumBalSheet.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DSsumBalSheet", SBalSheet));
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Balance Sheet";
            return View("_ReportView");
        }

        [HttpGet]
        public ActionResult FullStock()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FullStock(int i = 0)
        {
            DataTable FullStock = _repo.GetFullStock();
            FullStock.TableName = "DSFullStock";
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);   //Reports/FullStock
            reportViewer.Height = Unit.Percentage(900);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\FullStock.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DSFullStock", FullStock));
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Full Stock";
            return View("_ReportView");
        }

        [HttpGet]
        public ActionResult CustomLedger()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomLedger(DtoCustomLedger CLdr)
        {
            DataTable CusLdr = _repo.GetCustomLedger(CLdr);
            CusLdr.TableName = "DS_CustomLedger";
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\CustomLedger.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_CustomLedger", CusLdr));
            ReportParameterCollection rptParam = new ReportParameterCollection 
            { new ReportParameter("ParamDateFrom", CLdr.StartDate.ToShortDateString()),
              new ReportParameter("ParamDateTo", CLdr.EndDate.ToShortDateString()),
              new ReportParameter("DOName", CLdr.AccString)
            };
            reportViewer.LocalReport.SetParameters(rptParam);
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportTitle = "Customer Ledger";
            return View("_ReportView");
        }
    }
}