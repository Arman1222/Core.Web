using Core.Web.ActionResults;
using Core.Web.Helpers;
using Core.Web.Models.Applications;
using Core.Web.Utilities;
using Microsoft.Reporting.WebForms;
using Microsoft.Web.Mvc;
using MyWeb;
using MyWeb.DataLayer;
using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Core.Web.Controllers
{    
    public abstract class CoreControllerBase : Controller
	{
        private SqlHelper _query = new SqlHelper("DefaultConnection");

        protected EntityBase MapEntityBase(EntityBase src,EntityBase dest)           
        {
            dest.CreateBy = src.CreateBy;
            dest.CreateDate = src.CreateDate;
            dest.UpdateBy = src.UpdateBy;
            dest.UpdateDate = src.UpdateDate;
            dest.ApproveBy = src.ApproveBy;
            dest.ApproveDate = src.ApproveDate;
            dest.RejectBy = src.RejectBy;
            dest.RejectDate = src.RejectDate;
            return dest;
        }

        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
            where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }

        //http://stackoverflow.com/questions/6581418/get-action-from-controller-using-a-lambda-expression
        //http://stackoverflow.com/questions/6391644/how-do-i-call-dynamicinvoke-on-an-expressionactiont-using-compile
        //protected object ExecuteAction<TController>(Expression<Action<TController>> action)
        //    where TController : Controller
        //{
        //    var controller = DependencyResolver.Current.GetService<TController>();
        //    return action.Compile().DynamicInvoke(controller);            
        //}

        protected TController GetController<TController>()
            where TController : Controller
        {
            return DependencyResolver.Current.GetService<TController>();            
        }

        [Obsolete("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.")]
        protected JsonResult Json<T>(T data)
        {
            throw new InvalidOperationException("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.");
        }

        protected CoreJsonResult JsonValidationError()
        {
            var result = new CoreJsonResult();

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }
            return result;
        }

        protected async Task<CoreJsonResult> JsonValidationErrorAsync()
        {
            var result = new CoreJsonResult();

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }
            return result;
        }

        protected CoreJsonResult JsonError(string errorMessage)
        {
            var result = new CoreJsonResult();

            result.AddError(errorMessage);

            return result;
        }

        protected CoreJsonResult<T> JsonError<T>(T data)
        {
            var result = new CoreJsonResult<T> { Data = data };
            result.AddError("Error Global");
            return result;
        }

        protected async Task<CoreJsonResult> JsonErrorAsync(string errorMessage)
        {
            var result = new CoreJsonResult();

            result.AddError(errorMessage);

            return result;
        }

        //public CoreJsonResult<T> CoreJson<T>(T model)
        //{
        //    return new CoreJsonResult<T>() { Data = model };
        //}

        protected async Task<CoreJsonResult<T>> JsonSuccessAsync<T>(T data)
        {
            return new CoreJsonResult<T> { Data = data };
        }

        protected CoreJsonResult<T> JsonSuccess<T>(T data)
        {
            return new CoreJsonResult<T> { Data = data };
        }

        protected async Task<JsonResult> ExecuteFaultHandledOperationAsync(Func<Task<JsonResult>> codetoExecute)
        {
            try
            {
                return await codetoExecute.Invoke();
            }
			catch (DbEntityValidationException e) //http://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert
			{
				string errorEntity = string.Empty;
				foreach (var eve in e.EntityValidationErrors)
				{
                    errorEntity += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors: <br/>",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
                        errorEntity += string.Format("- Property: \"{0}\", Error: \"{1}\"  <br/>",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
                this.saveLog(Message: errorEntity, InnerMessage: e.InnerException == null ? "" : e.InnerException.ToString(), Tracert: e.StackTrace, TypeError: "Assembly/Dll");
                return JsonError("Error Entity : " + errorEntity);
			}
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                var loaderExceptions = ex.LoaderExceptions;
                string errorLoader = string.Empty;
                foreach (var item in loaderExceptions)
                {
                    errorLoader += item.Message + "<br/>";
                }
                this.saveLog(Message: errorLoader, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError: "Assembly/Dll");
                return JsonError("Error Loader Exceptions : " + errorLoader);
            }
            catch (SqlException ex)
            {
                this.saveLog(Message: ex.Message, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError: "SQL/Database");
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        return JsonError("Error Sql : " + ex.InnerException.InnerException.Message);
                    }
                    return JsonError("Error Sql : " + ex.InnerException.Message);
                }
                return JsonError("Error Sql : " + ex.Message);
            }
            catch (Exception ex)
            {
                this.saveLog(Message: ex.Message, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError:"Code C#");
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        return JsonError("Error : " + ex.InnerException.InnerException.Message);
                    }
                    return JsonError("Error : " + ex.InnerException.Message);
                }
                return JsonError("Error : " + ex.Message);
            }
        }

        protected JsonResult ExecuteFaultHandledOperation(Func<JsonResult> codetoExecute)
        {
            try
            {
                return codetoExecute.Invoke();
            }
            catch (SqlException ex)
            {
                this.saveLog(Message: ex.Message, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError: "SQL/Database");
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        return JsonError("Error Sql : " + ex.InnerException.InnerException.Message);
                    }
                    return JsonError("Error Sql : " + ex.InnerException.Message);
                }
                return JsonError("Error Sql : " + ex.Message);
            }                    
            catch (Exception ex)
            {
                this.saveLog(Message: ex.Message, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError: "Code C#");
                if (ex.InnerException != null)
                {
                    if(ex.InnerException.InnerException != null){
                        return JsonError("Error : " + ex.InnerException.InnerException.Message);
                    }
                    return JsonError("Error : " + ex.InnerException.Message);
                }
                return JsonError("Error : " + ex.Message);
            }
        }

        protected void ExecuteFaultHandledOperation(Action codetoExecute)
        {
            try
            {
                codetoExecute.Invoke();
            }
            catch (SqlException ex)
            {
                this.saveLog(Message: ex.Message, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError: "SQL/Database");
                JsonError("Error Sql : " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
            catch (Exception ex)
            {
                this.saveLog(Message: ex.Message, InnerMessage: ex.InnerException == null ? "" : ex.InnerException.ToString(), Tracert: ex.StackTrace, TypeError: "Code C#");
                JsonError("Error : " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }

        private List<ReportParameter> GetReportParameters(string[] parameters)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            // Construct Report parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] is string && i < (parameters.Length - 1))
                {
                    ReportParameter p = new ReportParameter(parameters[i], parameters[++i]);
                    reportParameters.Add(p);
                }
            }

            return reportParameters;
        } 

        protected ReportViewer ReportViewer(string reportPath,List<ReportDataSource> dataSources, params string[] parameters)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.AsyncRendering = false;
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportPath;
            
            if (dataSources != null && dataSources.Count() > 0)
            foreach (var item in dataSources)
            {
                reportViewer.LocalReport.DataSources.Add(item); 
            }            

            List<ReportParameter> reportParameters = GetReportParameters(parameters);            
            if (reportParameters.Count() > 0)
                reportViewer.LocalReport.SetParameters(reportParameters);

            return reportViewer;
        }

        //http://www.codeproject.com/Articles/492739/Exporting-to-Word-PDF-using-Microsoft-Report-RDLC
        //http://stackoverflow.com/questions/5826649/returning-a-file-to-view-download-in-asp-net-mvc

        private ActionResult ExportReport(Assembly callingAssembly, string exportType,string contentType, string reportPath,string filename, string extension,List<ReportDataSource> dataSources, params string[] parameters)
        {           
            using (LocalReport report = new LocalReport())
            {
                //https://social.msdn.microsoft.com/Forums/en-US/5b6cd9bf-baf0-4726-8507-5e732c48dd10/the-report-definition-for-report-xxx-has-not-been-specified?forum=vsreportcontrols
                //http://stackoverflow.com/questions/12903170/the-source-of-the-report-definition-has-not-been-specified
                //http://stackoverflow.com/questions/253735/display-rdlc-report-embedded-in-a-dll-file
                Stream s = callingAssembly.GetManifestResourceStream(reportPath);
                report.LoadReportDefinition(s);

                //report.ReportPath = reportPath;
                //report.ReportEmbeddedResource = reportPath;
                //ReportDataSource rds = new ReportDataSource();
                //rds.Name = "DataSet1";//This refers to the dataset name in the RDLC file
                //rds.Value = EmployeeRepository.GetAllEmployees();
                //report.DataSources.Add(rds);

                if (dataSources != null && dataSources.Count() > 0)
                    foreach (var item in dataSources)
                    {
                        report.DataSources.Add(item);
                    }

                List<ReportParameter> reportParameters = GetReportParameters(parameters);
                if (reportParameters.Count() > 0)
                    report.SetParameters(reportParameters);

                Byte[] mybytes = report.Render(exportType);
                //Byte[] mybytes = report.Render("PDF"); for exporting to PDF

                var cd = new System.Net.Mime.ContentDisposition
                {
                    // for example foo.bak
                    FileName = ((string.IsNullOrEmpty(filename)) ? Guid.NewGuid().ToString() : filename) + "." + extension,

                    // always prompt the user for downloading, set to true if you want 
                    // the browser to try to show the file inline
                    Inline = false,
                };

                //using (FileStream fs = System.IO.File.Create(@"D:\SalSlip.doc"))
                //{
                //    fs.Write(mybytes, 0, mybytes.Length);
                //}

                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(mybytes, contentType);
            }
        }

        protected ActionResult ExportWord(string reportPath, string filename, List<ReportDataSource> dataSources, params string[] parameters)
        {
            return ExportReport(Assembly.GetCallingAssembly(),"WORD", "application/msword", reportPath, filename,"doc", dataSources, parameters);
        }

        protected ActionResult ExportPdf(string reportPath, string filename, List<ReportDataSource> dataSources, params string[] parameters)
        {
            return ExportReport(Assembly.GetCallingAssembly(),"PDF", "application/pdf",reportPath, filename, "pdf", dataSources, parameters);       
        }

        protected async Task<ActionResult> LoginAsync(
            ApplicationUserManager userManager
           , ApplicationSignInManager signInManager
           , LoginViewModel model
           , string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user = new ApplicationUser();
            using (var db = new ApplicationDbContext())
            {
                user = db.Users.FirstOrDefault(u => u.UserName == model.Username); //find by username
                if (user != null)
                {
                    if (!await userManager.IsEmailConfirmedAsync(user.Id))
                    {
                        ViewBag.Email = user.Email;
                        return View("UnconfirmedAccount");
                    }
                }
            }
            try
            {
                var result = await signInManager.PasswordSignInAsync2(model.Username, model.Password, model.RememberMe, shouldLockout: true); //login by custom password override CheckPasswordAsync di identityconfig
                switch (result)
                {
                    case CoreSignInStatus.Success:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogIn", Desc: "Login Succes", returnUrl: returnUrl, UserName: model.Username);
                        return RedirectToLocal(returnUrl);
                    case CoreSignInStatus.LockedOut:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogOut", Desc: "LogOut Succes", returnUrl: returnUrl, UserName: model.Username);
                        return View("Lockout");
                    case CoreSignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case CoreSignInStatus.NotRegistered:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogIn", Desc: "Login Failed: User Tidak Terdaftar!", returnUrl: returnUrl, UserName: model.Username);
                        ModelState.AddModelError("", "User Tidak Terdaftar!");
                        return View(model);
                    case CoreSignInStatus.InActive:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogIn", Desc: "Login Failed: User Tidak Aktif!", returnUrl: returnUrl, UserName: model.Username);
                        ModelState.AddModelError("", "User Tidak Aktif!");
                        return View(model);
                    case CoreSignInStatus.Expired:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogIn", Desc: "Login Failed: User Expired!", returnUrl: returnUrl, UserName: model.Username);
                        ModelState.AddModelError("", "User Expired!");
                        return RedirectToAction("ChangePassword", "Manage", new { message = "User Expired Silahkan Change Password!", username = model.Username, returnUrl = returnUrl });
                    //return View(model);
                    case CoreSignInStatus.IncorrectPassword:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogIn", Desc: "Login Failed: Password Salah!", returnUrl: returnUrl, UserName: model.Username);
                        ModelState.AddModelError("", "Password Salah!");
                        return View(model);
                    case CoreSignInStatus.Failure:
                    default:
                        this.saveUserLog(UserId: user == null ? "" : user.Id, TypeLog: "LogIn", Desc: "Login Failed: Invalid login attempt.!", returnUrl: returnUrl, UserName: model.Username);
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            //return RedirectToAction(WebHelpers.GetPortalActionName(), WebHelpers.GetPortalControllerName());
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(WebHelpers.GetPortalActionName(), WebHelpers.GetPortalControllerName());
        }

        private void saveLog(string Message = "", String InnerMessage = "", string Tracert = "", string TypeError = "")
        {
            try
            {
                var Params = new object[] { 
                    new SqlParameter("@Application", System.Configuration.ConfigurationManager.AppSettings["ApplicationName"]),
                    new SqlParameter("@TypeError", TypeError),
                    new SqlParameter("@Message", Message),
                    new SqlParameter("@InnerMessage", InnerMessage),
                    new SqlParameter("@Tracert", Tracert),
                    new SqlParameter("@User", User.Identity.Name) 
                };
                _query.ExecNonQueryProc("ApplicationLogError", Params);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        protected void saveUserLog(string UserId = "", string TypeLog = "", string Desc = "", string returnUrl = "" , string UserName = "")
        {
            try
            {
                var Params = new object[] { 
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@UserName",  UserName),
                    new SqlParameter("@TypeLog", TypeLog),
                    new SqlParameter("@ApplicationName", returnUrl),
                    new SqlParameter("@Desc", Desc)
                };
                _query.ExecNonQueryProc("ApplicationUserLog", Params); 
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

	}
}