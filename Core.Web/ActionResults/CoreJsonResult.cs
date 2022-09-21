using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.ActionResults
{
	public class CoreJsonResult : JsonResult
	{
		public IList<string> ErrorMessages { get; private set; }

		public CoreJsonResult()
		{
			ErrorMessages = new List<string>();
		}

		public void AddError(string errorMessage)
		{
			ErrorMessages.Add(errorMessage);
		}

		public override void ExecuteResult(ControllerContext context)
		{
			DoUninterestingBaseClassStuff(context);

			SerializeData(context.HttpContext.Response);
		}

		private void DoUninterestingBaseClassStuff(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
				"GET".Equals(context.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException(
					"GET access is not allowed.  Change the JsonRequestBehavior if you need GET access.");
			}

			var response = context.HttpContext.Response;
			response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

			if (ContentEncoding != null)
			{
				response.ContentEncoding = ContentEncoding;
			}
		}

		protected virtual void SerializeData(HttpResponseBase response)
		{
            //if (!CoreLicense.IsValidLicenseAvailable())
            //{               
            //    Data = new
            //    {
            //        Success = false,                   
            //        ErrorMessage = "Application License Invalid!!!"
            //    };
            //}

			if (ErrorMessages.Any())
			{
                var originalData = Data;
                Data = new
                {
                    Success = false,
                    OriginalData = originalData,
                    ErrorMessage = string.Join("\n", ErrorMessages),
                    ErrorMessages = ErrorMessages.ToArray()
                };

				response.StatusCode = 400;
			}

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter()
                    //,new EscapeCommaConverter()
                    //,new IsoDateTimeConverter()
                },
            };

            response.Write(JsonConvert.SerializeObject(Data, settings));

			//if (Data == null) return;

			//response.Write(Data.ToJson());
		}
	}
    public class CoreJsonResult<T> : CoreJsonResult
	{
		public new T Data
		{
			get { return (T)base.Data; }
			set { base.Data = value; }
		}
	}
}