using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Helpers;
using Core.Web.Infrastructure;
using Core.Web.Utilities;
using MyWeb.DataLayer;
using MyWeb.DataLayer.SqlCore;
using MyWeb.Infrastructure.Applications;
using MyWeb.Models.Customers;
using MyWeb.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers.Customers
{
    [MyAuthorize]
    public class CustomerController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;        
        private SqlHelper _query = new SqlHelper("SqlCoreConnection");

        public CustomerController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> TestShowError()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {                
                return JsonError("Test Return Show Error Customer");
            });
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetPage(string searchText, int pageNumber = 1, int pageSize = 10, string sortBy = "CustomerId", string sortDirection = "asc", string address = "", string email = "", DateTime? terminationDate = null)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlCoreDbContext())
                {
                    int totalItems = ctx.Customers
                    .Count(x => (string.IsNullOrEmpty(searchText) ||
                             x.Name.Contains(searchText))
                        && (string.IsNullOrEmpty(address) ||
                             x.Address.Contains(address))
                        && (string.IsNullOrEmpty(email) ||
                             x.Email.Contains(email))
                    );
                    var listModels = await ctx.Customers
                    .Where(x => (string.IsNullOrEmpty(searchText) ||
                             x.Name.Contains(searchText))
                            && (string.IsNullOrEmpty(address) ||
                             x.Address.Contains(address))
                            && (string.IsNullOrEmpty(email) ||
                                 x.Email.Contains(email))
                            && (terminationDate == null ||
                                 x.TerminationDate == terminationDate)
                    )
                    .OrderBy(sortBy + " " + sortDirection)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                    IList<CustomerViewModel> list = Mapper.Map<IList<Customer>, IList<CustomerViewModel>>(listModels);

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });
        }

        public async Task<JsonResult> All(string searchText, string sortBy = "CustomerId", string sortDirection = "asc")
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlCoreDbContext())
                {
                    var models = ctx.Customers
                    .Where(x => string.IsNullOrEmpty(searchText) ||
                            (
                             x.Name.Contains(searchText) ||
                             x.Address.Contains(searchText)
                            )
                    );

                    int totalItems = await models.CountAsync();

                    var listModels = await models
                    .OrderBy(sortBy + " " + sortDirection)                   
                    .ToListAsync();

                    IList<CustomerViewModel> list = Mapper.Map<IList<Customer>, IList<CustomerViewModel>>(listModels);

                    return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
                }
            });

        }

        public async Task<JsonResult> Add(CustomerViewModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return JsonValidationError();
                }

                if (form.Name.Length < 5)
                {
                    return JsonError("Name Minimal 5 Char");
                }

                using (var ctx = new SqlCoreDbContext())
                {
                    Customer customerModel = Mapper.Map<Customer>(form);

                    ctx.Customers.Add(customerModel);
                    await ctx.SaveChangesAsync();

                    return JsonSuccess("Success!");
                }
            });
        }

        public async Task<JsonResult> Update(CustomerViewModel form)
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                using (var ctx = new SqlCoreDbContext())
                {
                    var target = ctx.Customers.Find(form.CustomerId);
                    if (target == null)
                    {
                        return JsonError("Data Tidak Ditemukan di database!");
                    }

                    Mapper.Map(form, target);
                    target.SetUpdateByCurrentUser();
                    
                    await ctx.SaveChangesAsync();


                    return JsonSuccess("Update Success");
                }
            });
        }


        ///// <summary>
        ///// Execute a Soap WebService call
        ///// </summary>
        //public override void Execute()
        //{
        //    HttpWebRequest request = this.CreateWebRequest();
        //    XmlDocument soapEnvelopeXml = new XmlDocument();
        //    soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>"+
        //        "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">"+
        //        "<soap:Body>"+
        //        "<HelloWorld3 xmlns=\"http://tempuri.org/\">"+
        //        "<parameter1>test</parameter1>"+
        //        "<parameter2>23</parameter2>"+
        //        "<parameter3>test</parameter3>"+
        //        "</HelloWorld3>"+
        //        "</soap:Body>"+
        //        "</soap:Envelope>");
        //    using (Stream stream = request.GetRequestStream())
        //    {
        //        soapEnvelopeXml.Save(stream);
        //    }
        //    using (WebResponse response = request.GetResponse())
        //    {
        //        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
        //        {
        //            string soapResult = rd.ReadToEnd();
        //            Console.WriteLine(soapResult);
        //        }
        //    }
        //}
        ///// <summary>
        ///// Create a soap webrequest to [Url]
        ///// </summary>
        ///// <returns></returns>
        //public HttpWebRequest CreateWebRequest()
        //{
        //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://dev.nl/Rvl.Demo.TestWcfServiceApplication/SoapWebService.asmx");
        //    webRequest.Headers.Add(@"SOAP:Action");
        //    webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        //    webRequest.Accept = "text/xml";
        //    webRequest.Method = "POST";
        //    return webRequest;
        //}
      
        public JsonResult Upload()
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var avatar = new Core.Web.Utilities.File()
                        {
                            FileName = System.IO.Path.GetFileName(fileContent.FileName),
                            FileType = FileType.Xlsx,
                            ContentType = fileContent.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(fileContent.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(fileContent.ContentLength);
                        }
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return JsonError("Unable to upload. Try again, and if the problem persists see your system administrator.");
            }
            return JsonSuccess("Upload File Success");
        }

        public JsonResult UploadData(IEnumerable<UploadData> data)
        {
            IEnumerable<UploadData> uploadExcelList = data;
            return JsonSuccess("Upload File Success");
        }

    }
}