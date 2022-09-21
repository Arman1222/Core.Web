using Core.Web.Controllers;
using Core.Web.Models.Applications;
using Core.Web.Utilities;
using Microsoft.Reporting.WebForms;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyWeb.Controllers.Applications
{    
	public class ReportController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;

        public ReportController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

		// GET: /ReportExample/
        public ActionResult Index()
        {            
            List<Application> list = null;
            using (var ctx = new ApplicationDbContext())
            {
                list = ctx.ApplicationSet.ToList();
            }            

            List<ReportDataSource> reportDataSourceList = new List<ReportDataSource> ();
            reportDataSourceList.Add(new ReportDataSource("DataSet1", list.ToDataTable()));

            //var reportViewer = ReportViewer(@"\Reports\Applications\ApplicationReportExample.rdlc", reportDataSourceList, "ReportTitle", "Report Application"); 
            //ViewBag.ReportViewer = reportViewer;
            //EmailServer aa = new EmailServer();
            //aa.addAttachments(reportViewer, "Nama Report 2", AttachmentType.excel);
            //aa.addRecipients("ichwan.yasir@bankmayora.co.id");
            //aa.body = "asjdajsdasjda jdashd ashdosakjhda sdhaskdh sakhd";
            //aa.body_format = "html";
            //aa.Send();
            return View();
        }

        public JsonResult Word()
        {
            List<Application> list = null;
            using (var ctx = new ApplicationDbContext())
            {
                list = ctx.ApplicationSet.ToList();
            }

            List<ReportDataSource> reportDataSourceList = new List<ReportDataSource>();
            reportDataSourceList.Add(new ReportDataSource("DataSet1", list.ToDataTable()));

            FileContentResult fr = (FileContentResult)ExportWord("MyWeb.Reports.Applications.ApplicationReportExample.rdlc", "TestWord", reportDataSourceList, "ReportTitle", "Report Application");       

            return JsonSuccess(new {contentType=fr.ContentType, data = fr.FileContents});
        }

        public JsonResult Pdf()
        {
            List<Application> list = null;
            using (var ctx = new ApplicationDbContext())
            {
                list = ctx.ApplicationSet.ToList();
            }

            List<ReportDataSource> reportDataSourceList = new List<ReportDataSource>();
            reportDataSourceList.Add(new ReportDataSource("DataSet1", list.ToDataTable()));

            FileContentResult fr = (FileContentResult)ExportPdf("MyWeb.Reports.Applications.ApplicationReportExample.rdlc", "TestPdf", reportDataSourceList, "ReportTitle", "Report Application");

            return JsonSuccess(new { contentType = fr.ContentType, data = fr.FileContents });
        }       

        //private ReportParameter[] GetParametersLocal()
        //{
        //    ReportParameter p1 = new ReportParameter("ReportTitle", "Local Report Example");
        //    return new ReportParameter[] { p1 };
        //}
	}
}