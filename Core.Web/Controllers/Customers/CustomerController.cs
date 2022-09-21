using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Web.Controllers;
using MyWeb.DataLayer;
using MyWeb.ViewModels.Customers;
using MyWeb.Models.Customers;
using System;
using MyWeb.Controllers;
using MyWeb.DataLayer.SqlCore;
using MyWeb.Infrastructure.Applications;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity.Infrastructure;
using Core.Web.Utilities;
using Core.Web.Helpers;
using System.Threading.Tasks;
using Core.Web.Infrastructure;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Web.UI.WebControls;
using Core.Web.Utilities;
namespace MyWeb.Controllers.Customers
{
    [MyAuthorize]
	public class CustomerController : CoreControllerBase
	{
        private ApplicationDbContext _applicationDbContext;
        private ICurrentUser _currentUser;
        private SqlCoreDbContext _sqlCoreDbContext = new SqlCoreDbContext();
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

		public JsonResult All(int pageNumber = 1, int pageSize = 5)
		{
            return ExecuteFaultHandledOperation(() =>
            {
                //int pageCount = Convert.ToInt32(Math.Ceiling((double)(_sqlCoreDbContext.Customers.Count()
                //     / pageSize)));

                int totalItems = _sqlCoreDbContext.Customers.Count();

                var customerModels = _sqlCoreDbContext.Customers
                .OrderByDescending(x => x.CreateDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                IList<CustomerViewModel> list = Mapper.Map<IList<Customer>, IList<CustomerViewModel>>(customerModels);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
                
		}

        //public async Task<JsonResult> TestQueryAsync()
        //{
        //    //http://jeremybytes.blogspot.co.id/2015/01/task-and-await-basic-exception-handling.html
        //    //http://stackoverflow.com/questions/13689493/catching-exceptions-from-asynchronous-httpwebrequest-calls-in-a-task
        //    return ExecuteFaultHandledOperationAsync(() =>
        //    {
        //        await _query.ExecNonQueryAsync("exec test", "@test", ""); 
                
        //        return JsonSuccess("");
        //    });

        //}

        public async Task<JsonResult> Add(CustomerViewModel form)
		{
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return JsonValidationError();
                }

                if(form.Name.Length < 5){
                    return JsonError("Customer Name Minimal 5 Char");
                }

                Customer customerModel = Mapper.Map<Customer>(form);                    

                _sqlCoreDbContext.Customers.Add(customerModel);
                await _sqlCoreDbContext.SaveChangesAsync();
               
			    //var model = Mapper.Map<CustomerViewModel>(customer);
                return JsonSuccess(customerModel);
            });
		}

        public async Task<JsonResult> Update(CustomerViewModel form)
		{
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                var target = _sqlCoreDbContext.Customers.Find(form.CustomerId);
                if (target == null)
                {
                    return JsonError("Customer Tidak Ditemukan di database!");
                }
             
                Mapper.Map(form, target);
                await _sqlCoreDbContext.SaveChangesAsync();
                CustomerViewModel updatedCustomer = _sqlCoreDbContext.Customers.Project().To<CustomerViewModel>().Single(x => x.CustomerId == form.CustomerId);
                
                return JsonSuccess(updatedCustomer);
            });
		}

        //http://www.mikesdotnetting.com/article/259/asp-net-mvc-5-with-ef-6-working-with-files
        //http://monox.mono-software.com/blog/post/Mono/233/Async-upload-using-angular-file-upload-directive-and-net-WebAPI-service/
        //http://www.briankeating.net/post/Angularjs-NET-File-Upload
        public JsonResult Upload()
        {           
            try
            {                
                     foreach (string file in Request.Files) {
                        var fileContent = Request.Files[file];
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {
                            var avatar = new File()
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