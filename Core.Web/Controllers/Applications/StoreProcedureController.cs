using Core.Web.Controllers;
using MyWeb.DataLayer;
using MyWeb.Infrastructure.Applications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    public class StoreProcedureController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        private ICurrentUser _currentUser;
        public StoreProcedureController(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _applicationDbContext = context;
            _currentUser = currentUser;
        }

        public JsonResult ExecStoreProcedure(RequestExecSp _input)
        {
            GlobalResponse response = new GlobalResponse();
            try
            {
                string errMessage = "";
                List<MyParameter> params_ = new List<MyParameter>();
                string userId = null;
                if (_input.pActionBy)
                {
                    userId = _currentUser.User.Id;
                }
                var a = _applicationDbContext.ExecSp(_input, out params_, out errMessage, userId);
                if (!String.IsNullOrWhiteSpace(errMessage))
                {
                    response = libraryGlobalResponse.ErrorApps(errMessage, a);
                    if (params_.Count > 0)
                    {
                        response.outParam = JToken.Parse(JsonConvert.SerializeObject(params_));
                    }
                    return JsonError(response);
                }
                response = libraryGlobalResponse.Success(a);
                if (params_.Count > 0)
                {
                    response.outParam = JToken.Parse(JsonConvert.SerializeObject(params_));
                }
                return JsonSuccess(response);
            }
            catch (Exception ex)
            {
                var friendlyException = getMostDeepest(ex);
                response = libraryGlobalResponse.Error(Message: ex.Message, data: new
                {
                    Message = friendlyException.Message,
                    Source = friendlyException.Source,
                    Number = friendlyException.HResult,
                    TargetSite = friendlyException.TargetSite,
                    HelpLink = friendlyException.HelpLink,
                    data = friendlyException.Data
                });

                return JsonError(response);
            }
        }

        private Exception getMostDeepest(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex;
            }
            else
            {
                return getMostDeepest(ex.InnerException);
            }
        }

    }

    public class GlobalResponse
    {
        public GlobalResponse()
        {
        }
        public string Code { get; set; }
        public string Message { get; set; }
        public string status { get; set; }
        public JToken data { get; set; }
        public JToken outParam { get; set; }
    }

    public static class libraryGlobalResponse
    {
        public static GlobalResponse Error(String Code = "99", string Message = "Error Complex", string Status = "Error Global", object data = null)
        {
            GlobalResponse model = new GlobalResponse();
            model.Code = Code;
            model.Message = Message;
            model.status = Status;
            model.data = JToken.Parse(JsonConvert.SerializeObject(data));
            return model;
        }
        public static GlobalResponse ErrorSystem()
        {
            GlobalResponse model = new GlobalResponse();
            model.Code = "98";
            model.Message = "System Error";
            model.status = "Error System";
            return model;
        }
        public static GlobalResponse ErrorApps(string Status = "Error Global", object data = null)
        {
            GlobalResponse model = new GlobalResponse();
            model.Code = "97";
            model.Message = Status;
            model.status = "Error Apps";
            model.data = JToken.Parse(JsonConvert.SerializeObject(data));
            return model;
        }
        public static GlobalResponse ErrorRequest()
        {
            GlobalResponse model = new GlobalResponse();
            model.Code = "01";
            model.Message = "Invalid Request";
            model.status = "Error User";
            return model;
        }
        public static GlobalResponse ErrorFile()
        {
            GlobalResponse model = new GlobalResponse();
            model.Code = "02";
            model.Message = "Invalid File";
            model.status = "Error User";
            return model;
        }
        public static GlobalResponse Success(object data = null)
        {
            GlobalResponse model = new GlobalResponse();
            model.Code = "00";
            model.Message = "Success";
            model.status = "Success";
            model.data = JToken.Parse(JsonConvert.SerializeObject(data));
            return model;
        }

    }

}
