using Core.Web.Controllers;
using Core.Web.Infrastructure;
using System;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    [MyAuthorize]
    public class UploadFileController : CoreControllerBase
    {
        private ApplicationUserManager _userManager;
        private string _attachmentKatalog = "~/Files/";
        private string _attachmentKatalogTemp = "~/Files/Temp/"; 
        private string _Temp = "/Temp/";

        public UploadFileController()
        {  
        }

        public JsonResult Upload(string folder = null)
        {
            string path = String.IsNullOrWhiteSpace(folder) ? Server.MapPath(_attachmentKatalogTemp) : Server.MapPath(_attachmentKatalog+folder+_Temp);
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filename = string.Empty;
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        //filename = Guid.NewGuid() + "-" + fileContent.FileName;
                        //Ady , begin
                        filename = fileContent.FileName;
                        //Ady , end
                        string nameAndLocation = (String.IsNullOrWhiteSpace(folder) ? _attachmentKatalogTemp : _attachmentKatalog + folder + _Temp) + filename;
                        fileContent.SaveAs(Server.MapPath(nameAndLocation));
                        break;
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                return JsonError("Unable to upload Attachment File. Try again, and if the problem persists see your system administrator.");
            }
            return JsonSuccess(filename);
        }

        public JsonResult ResetData(string namaFile, string folder = null)
        {
            try
            {
                string path = String.IsNullOrWhiteSpace(folder) ? Server.MapPath(_attachmentKatalogTemp + namaFile) : Server.MapPath(_attachmentKatalog + folder + _Temp + namaFile);
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileInfo fl = new FileInfo(path);
                    fl.Delete();
                }
                return JsonSuccess("Succes");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return JsonError("Unable to upload Attachment Katalog. Try again, and if the problem persists see your system administrator.");
            }
        }

        public JsonResult SetImage(string namaFile, string folder = null)
        {
            try
            {
                var file = String.IsNullOrWhiteSpace(folder) ? Server.MapPath(_attachmentKatalogTemp + namaFile) : Server.MapPath(_attachmentKatalog + folder + _Temp + namaFile);
                System.IO.File.Copy(file, Path.Combine(Server.MapPath(String.IsNullOrWhiteSpace(folder) ? _attachmentKatalog : _attachmentKatalog + folder), Path.GetFileName(file)), true);
                System.IO.File.Delete(file);
                return JsonSuccess("Succes");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return JsonError("Unable to upload Attachment Katalog. Try again, and if the problem persists see your system administrator.");
            }
        }

        public JsonResult ClearImage(string namaFile, string folder = null)
        {
            try
            {
                var file = String.IsNullOrWhiteSpace(folder) ? Server.MapPath(_attachmentKatalog + namaFile) : Server.MapPath(_attachmentKatalog + folder +"/"+ namaFile);
                System.IO.File.Delete(file);
                return JsonSuccess("Succes");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return JsonError("Unable to upload Attachment Katalog. Try again, and if the problem persists see your system administrator.");
            }
        }

    }
}