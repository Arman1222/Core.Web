using Core.Web.Helpers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Core.Web.Utilities
{
    public class Email
    {
        private SqlHelper _query = new SqlHelper("DefaultConnection");
        private string templateFooter = "</td></tr></tr></table><div class=\"footer\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"content-block\"><span class=\"apple-link\">Company Ltd. Bank Mayora, Kcp Tomang Raya, Jakarta Barat 11440</span><br>Contact : Telp. (021) 5655288, Fax. (021) 5655277</a>.</td></tr><tr><td class=\"content-block powered-by\">Powered by <a href=\"mailto:itdev@bankmayora.co.id\" style=\"color:blue;\">IT Develpment</a> Bank Mayora.</td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>";
        private string templateHeader = "<!doctype html><html><head><meta name=\"viewport\" content=\"width=device-width\" /><meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF-8\" /><title></title><style>img {border: none;-ms-interpolation-mode: bicubic;max-width: 100%;} body {background-color: #f6f6f6;font-family: sans-serif;-webkit-font-smoothing: antialiased;font-size: 14px;line-height: 1.4;margin: 0;padding: 0;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;} table {border-collapse: separate;mso-table-lspace: 0pt;mso-table-rspace: 0pt;width: 100%;}table td { font-family: sans-serif;font-size: 14px;vertical-align: top;}.body {background-color: #f6f6f6;width: 100%;}.container {display: block;Margin: 0 auto !important;max-width: 580px;padding: 10px;width: auto !important;width: 580px;} .content {box-sizing: border-box;display: block;Margin: 0 auto;max-width: 580px;padding: 10px;} .main {background: #fff;border-radius: 3px;width: 100%;} .wrapper {box-sizing: border-box;padding: 20px;} .footer {clear: both;padding-top: 10px;text-align: center;width: 100%;}.footer td,.footer p,.footer span,.footer a { color: #999999;font-size: 12px;text-align: center;} h1, h2, h3, h4 {color: #000000;font-family: sans-serif;font-weight: 400;line-height: 1.4;margin: 0;Margin-bottom: 30px;} h1 {font-size: 35px;font-weight: 300;text-align: center;text-transform: capitalize;} p, ul, ol {font-family: sans-serif;font-size: 14px;font-weight: normal;margin: 0;Margin-bottom: 15px;}p li,ul li,ol li { list-style-position: inside;margin-left: 5px;} a {color: #3498db;text-decoration: underline;}.btn {box-sizing: border-box;width: 100%;}.btn > tbody > tr > td { padding-bottom: 15px;}.btn table { width: auto;}.btn table td { background-color: #ffffff;border-radius: 5px;text-align: center;}.btn a { background-color: #ffffff;border: solid 1px #3498db;border-radius: 5px;box-sizing: border-box;color: #3498db;cursor: pointer;display: inline-block;font-size: 14px;font-weight: bold;margin: 0;padding: 12px 25px;text-decoration: none;text-transform: capitalize;} .btn-primary table td {background-color: #3498db;} .btn-primary a {background-color: #3498db;border-color: #3498db;color: #ffffff;}.last {margin-bottom: 0;} .first {margin-top: 0;} .align-center {text-align: center;} .align-right {text-align: right;} .align-left {text-align: left;} .clear {clear: both;} .mt0 {margin-top: 0;} .mb0 {margin-bottom: 0;} .preheader {color: transparent;display: none;height: 0;max-height: 0;max-width: 0;opacity: 0;overflow: hidden;mso-hide: all;visibility: hidden;width: 0;} .powered-by a {text-decoration: none;} hr {border: 0;border-bottom: 1px solid #f6f6f6;Margin: 20px 0;} @media only screen and (max-width: 620px) {table[class=body] h1 { font-size: 28px !important;margin-bottom: 10px !important;}table[class=body] p,table[class=body] ul,table[class=body] ol,table[class=body] td,table[class=body] span,table[class=body] a { font-size: 16px !important;}table[class=body] .wrapper,table[class=body] .article { padding: 10px !important;}table[class=body] .content { padding: 0 !important;}table[class=body] .container { padding: 0 !important;width: 100% !important;}table[class=body] .main { border-left-width: 0 !important;border-radius: 0 !important;border-right-width: 0 !important;}table[class=body] .btn table { width: 100% !important;}table[class=body] .btn a { width: 100% !important;}table[class=body] .img-responsive { height: auto !important;max-width: 100% !important;width: auto !important;}} @media all {.ExternalClass { width: 100%;}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div { line-height: 100%;}.apple-link a { color: inherit !important;font-family: inherit !important;font-size: inherit !important;font-weight: inherit !important;line-height: inherit !important;text-decoration: none !important;} .btn-primary table td:hover { background-color: #34495e !important;}.btn-primary a:hover { background-color: #34495e !important;border-color: #34495e !important;} } </style></head><body class=\"\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\"><tr><td>&nbsp;</td><td class=\"container\"><div class=\"content\"><table class=\"main\"><tr><td class=\"wrapper\">";
        public string profile_name { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
        public string body_format { get; set; }
        private List<string> blind_copy_recipients { get; set; }
        private List<string> copy_recipients { get; set; }
        private List<string> recipients { get; set; }
        public bool useTemplateHtmlBody { get; set; }
        public Email()
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.body_format = "Text";
        }
        public Email(string recipient, string subject, string body)
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.recipients.Add(recipient);
            this.subject = subject;
            this.body = body;
            this.body_format = "Text";
        }
        public Email(List<string> recipients, string subject, string body)
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.recipients = recipients;
            this.subject = subject;
            this.body = body;
            this.body_format = "Text";
        }
        public void addRecipients(string Email)
        {
            this.recipients.Add(Email);
        }
        public void addRecipients(List<string> Email)
        {
            this.recipients.AddRange(Email);
        }
        public void addCopy_recipients(string Email)
        {
            this.copy_recipients.Add(Email);
        }
        public void addCopy_recipients(List<string> Email)
        {
            this.copy_recipients.AddRange(Email);
        }
        public void addBlind_copy_recipients(string Email)
        {
            this.blind_copy_recipients.Add(Email);
        }
        public void addBlind_copy_recipients(List<string> Email)
        {
            this.blind_copy_recipients.AddRange(Email);
        }
        public void setUseHtmlFormatBody(bool HtmlFormatBody)
        {
            this.body_format = HtmlFormatBody ? "HTML" : null;
            this.useTemplateHtmlBody = HtmlFormatBody;
        }

        public void Send()
        {
            try
            {
                string _body = this.useTemplateHtmlBody ? templateHeader + this.body + templateFooter : this.body;
                string _recepients = String.Join(";", this.recipients.ToArray());
                string _copy_recepients = String.Join(";", this.copy_recipients.ToArray());
                string _blind_copy_recepients = String.Join(";", this.blind_copy_recipients.ToArray());
                var Params = new object[] { 
                new SqlParameter("@SubjectMail", this.subject),
                new SqlParameter("@AlertTo", _recepients),
                new SqlParameter("@AlertCC", String.IsNullOrWhiteSpace(_copy_recepients) ? (object)DBNull.Value : _copy_recepients ),
                new SqlParameter("@AlertBCC", String.IsNullOrWhiteSpace(_blind_copy_recepients) ? (object)DBNull.Value : _blind_copy_recepients ),
                new SqlParameter("@Body",_body),
                new SqlParameter("@body_format",this.body_format)
                };
                _query.ExecNonQuery("[sp_notif_email] @SubjectMail, @AlertTo, @AlertCC, @AlertBCC, @Body, @body_format", Params);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
    public class EmailServer
    {
        private string _attachmentTemp = "~/Files/EmailAttachment/Temp/"; 
        private SqlHelper _query = new SqlHelper("DefaultConnection");
        private string templateFooter = "</td></tr></tr></table><div class=\"footer\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"content-block\"><span class=\"apple-link\">Company Ltd. Bank Mayora, Kcp Tomang Raya, Jakarta Barat 11440</span><br>Contact : Telp. (021) 5655288, Fax. (021) 5655277</a>.</td></tr><tr><td class=\"content-block powered-by\">Powered by <a href=\"mailto:itdev@bankmayora.co.id\" style=\"color:blue;\">IT Develpment</a> Bank Mayora.</td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>";
        private string templateHeader = "<!doctype html><html><head><meta name=\"viewport\" content=\"width=device-width\" /><meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF-8\" /><title></title><style>img {border: none;-ms-interpolation-mode: bicubic;max-width: 100%;} body {background-color: #f6f6f6;font-family: sans-serif;-webkit-font-smoothing: antialiased;font-size: 14px;line-height: 1.4;margin: 0;padding: 0;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;} table {border-collapse: separate;mso-table-lspace: 0pt;mso-table-rspace: 0pt;width: 100%;}table td { font-family: sans-serif;font-size: 14px;vertical-align: top;}.body {background-color: #f6f6f6;width: 100%;}.container {display: block;Margin: 0 auto !important;max-width: 580px;padding: 10px;width: auto !important;width: 580px;} .content {box-sizing: border-box;display: block;Margin: 0 auto;max-width: 580px;padding: 10px;} .main {background: #fff;border-radius: 3px;width: 100%;} .wrapper {box-sizing: border-box;padding: 20px;} .footer {clear: both;padding-top: 10px;text-align: center;width: 100%;}.footer td,.footer p,.footer span,.footer a { color: #999999;font-size: 12px;text-align: center;} h1, h2, h3, h4 {color: #000000;font-family: sans-serif;font-weight: 400;line-height: 1.4;margin: 0;Margin-bottom: 30px;} h1 {font-size: 35px;font-weight: 300;text-align: center;text-transform: capitalize;} p, ul, ol {font-family: sans-serif;font-size: 14px;font-weight: normal;margin: 0;Margin-bottom: 15px;}p li,ul li,ol li { list-style-position: inside;margin-left: 5px;} a {color: #3498db;text-decoration: underline;}.btn {box-sizing: border-box;width: 100%;}.btn > tbody > tr > td { padding-bottom: 15px;}.btn table { width: auto;}.btn table td { background-color: #ffffff;border-radius: 5px;text-align: center;}.btn a { background-color: #ffffff;border: solid 1px #3498db;border-radius: 5px;box-sizing: border-box;color: #3498db;cursor: pointer;display: inline-block;font-size: 14px;font-weight: bold;margin: 0;padding: 12px 25px;text-decoration: none;text-transform: capitalize;} .btn-primary table td {background-color: #3498db;} .btn-primary a {background-color: #3498db;border-color: #3498db;color: #ffffff;}.last {margin-bottom: 0;} .first {margin-top: 0;} .align-center {text-align: center;} .align-right {text-align: right;} .align-left {text-align: left;} .clear {clear: both;} .mt0 {margin-top: 0;} .mb0 {margin-bottom: 0;} .preheader {color: transparent;display: none;height: 0;max-height: 0;max-width: 0;opacity: 0;overflow: hidden;mso-hide: all;visibility: hidden;width: 0;} .powered-by a {text-decoration: none;} hr {border: 0;border-bottom: 1px solid #f6f6f6;Margin: 20px 0;} @media only screen and (max-width: 620px) {table[class=body] h1 { font-size: 28px !important;margin-bottom: 10px !important;}table[class=body] p,table[class=body] ul,table[class=body] ol,table[class=body] td,table[class=body] span,table[class=body] a { font-size: 16px !important;}table[class=body] .wrapper,table[class=body] .article { padding: 10px !important;}table[class=body] .content { padding: 0 !important;}table[class=body] .container { padding: 0 !important;width: 100% !important;}table[class=body] .main { border-left-width: 0 !important;border-radius: 0 !important;border-right-width: 0 !important;}table[class=body] .btn table { width: 100% !important;}table[class=body] .btn a { width: 100% !important;}table[class=body] .img-responsive { height: auto !important;max-width: 100% !important;width: auto !important;}} @media all {.ExternalClass { width: 100%;}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div { line-height: 100%;}.apple-link a { color: inherit !important;font-family: inherit !important;font-size: inherit !important;font-weight: inherit !important;line-height: inherit !important;text-decoration: none !important;} .btn-primary table td:hover { background-color: #34495e !important;}.btn-primary a:hover { background-color: #34495e !important;border-color: #34495e !important;} } </style></head><body class=\"\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\"><tr><td>&nbsp;</td><td class=\"container\"><div class=\"content\"><table class=\"main\"><tr><td class=\"wrapper\">";
        private List<string> blind_copy_recipients = new List<string>();
        private List<string> copy_recipients= new List<string>();
        private List<string> recipients= new List<string>();
        private List<string> Attachments= new List<string>();
        public string profile_name { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
        public string body_format { get; set; }
        public bool useTemplateHtmlBody { get; set; }
        public string ITNotif { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        private void EmailSet()
        {
            ITNotif = (string)_query.ExecScalarProc("[SP_GetEmailConfig]", "@Key", "ITNotif");
            Host = (string)_query.ExecScalarProc("[SP_GetEmailConfig]", "@Key", "Host");
            Port = Convert.ToInt32((string)_query.ExecScalarProc("[SP_GetEmailConfig]", "@Key", "Port"));
        }
        public EmailServer()
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.body_format = "Text";
            EmailSet();
        }
        public EmailServer(string recipient, string subject, string body)
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.recipients.Add(recipient);
            this.subject = subject;
            this.body = body;
            this.body_format = "Text";
            EmailSet();
        }
        public EmailServer(List<string> recipients, string subject, string body)
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.recipients = recipients;
            this.subject = subject;
            this.body = body;
            this.body_format = "Text";
            EmailSet();
        }
        public void addRecipients(string Email)
        {
            this.recipients.Add(Email);
        }
        public void addRecipients(List<string> Email)
        {
            this.recipients.AddRange(Email);
        }
        public void addCopy_recipients(string Email)
        {
            this.copy_recipients.Add(Email);
        }
        public void addCopy_recipients(List<string> Email)
        {
            this.copy_recipients.AddRange(Email);
        }
        public void addBlind_copy_recipients(string Email)
        {
            this.blind_copy_recipients.Add(Email);
        }
        public void addBlind_copy_recipients(List<string> Email)
        {
            this.blind_copy_recipients.AddRange(Email);
        }
        public void addAttachments(string pathAttachment)
        {
            this.Attachments.Add(pathAttachment);
        }
        public void addAttachments(List<string> pathAttachment)
        {
            this.Attachments.AddRange(pathAttachment);
            
        }
        public void addAttachments(ReportViewer reportViewer, string namaReport, AttachmentType TypeAttachment)
        {
            string aa = ConvertReport(reportViewer, namaReport, TypeAttachment);
            this.Attachments.Add(aa);
        }
        public void setUseHtmlFormatBody(bool HtmlFormatBody)
        {
            this.body_format = HtmlFormatBody ? "HTML" : "TEXT";
            this.useTemplateHtmlBody = HtmlFormatBody;
        }
        private string ConvertReport(ReportViewer reportViewer, string namaReport, AttachmentType TypeAttachment)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string format, Extensi;
            switch (TypeAttachment)
            {
                case AttachmentType.excel:
                    format = "Excel";
                    Extensi = ".xls";
                    break;
                case AttachmentType.pdf:
                    format = "PDF";
                    Extensi = ".pdf";
                    break;
                case AttachmentType.Word:
                    format = "Word";
                    Extensi = ".docx";
                    break;
                default:
                    format = "Excel";
                    Extensi = ".xls";
                    break;
            }
            byte[] bytes;
            bytes = reportViewer.LocalReport.Render(format, null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //Write report out to temporary PDF file
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(_attachmentTemp)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(_attachmentTemp));
            }
            string nameGuid = Guid.NewGuid().ToString();
            string filename = HttpContext.Current.Server.MapPath(_attachmentTemp + nameGuid +"-"+namaReport + Extensi);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
                fs.Close();
            }
            return filename;
        }
        public void Send()
        {
            try
            {
                string[] email = this.ITNotif.Split(';');
                MailMessage message = new MailMessage();
                message.From = new MailAddress(email[0]);
                if (recipients.Count() > 0)
                    foreach (var item in recipients)
                    {
                        if (!String.IsNullOrWhiteSpace(item))
                            message.To.Add(item);
                    }
                if (copy_recipients.Count() > 0)
                    foreach (var item in copy_recipients)
                    {
                        if (!String.IsNullOrWhiteSpace(item))
                            message.CC.Add(item);
                    }
                if (blind_copy_recipients.Count() > 0)
                    foreach (var item in blind_copy_recipients)
                    {
                        if (!String.IsNullOrWhiteSpace(item))
                            message.Bcc.Add(item);
                    }
                if (Attachments.Count() > 0)
                    foreach (var item in Attachments)
                    {
                        if (!String.IsNullOrWhiteSpace(item))
                            message.Attachments.Add(new Attachment(item));
                    }
                message.Subject = this.subject;
                message.IsBodyHtml = this.body_format.ToUpper() == "HTML";
                message.Body = this.body;

                //This is for testing.
                SmtpClient smtp = new SmtpClient(this.Host);
                //port number for Gmail mail
                smtp.Port = this.Port;
                //credentials to login in to Gmail account
                smtp.Credentials = new NetworkCredential(email[0], email[1]);
                //enabled SSL
                smtp.EnableSsl = true;
               
                //Send an email
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
    public class EmailDatabase
    {
        private SqlHelper _query = new SqlHelper("DefaultConnection");
        private string templateFooter = "</td></tr></tr></table><div class=\"footer\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"content-block\"><span class=\"apple-link\">Company Ltd. Bank Mayora, Kcp Tomang Raya, Jakarta Barat 11440</span><br>Contact : Telp. (021) 5655288, Fax. (021) 5655277</a>.</td></tr><tr><td class=\"content-block powered-by\">Powered by <a href=\"mailto:itdev@bankmayora.co.id\" style=\"color:blue;\">IT Develpment</a> Bank Mayora.</td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>";
        private string templateHeader = "<!doctype html><html><head><meta name=\"viewport\" content=\"width=device-width\" /><meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF-8\" /><title></title><style>img {border: none;-ms-interpolation-mode: bicubic;max-width: 100%;} body {background-color: #f6f6f6;font-family: sans-serif;-webkit-font-smoothing: antialiased;font-size: 14px;line-height: 1.4;margin: 0;padding: 0;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;} table {border-collapse: separate;mso-table-lspace: 0pt;mso-table-rspace: 0pt;width: 100%;}table td { font-family: sans-serif;font-size: 14px;vertical-align: top;}.body {background-color: #f6f6f6;width: 100%;}.container {display: block;Margin: 0 auto !important;max-width: 580px;padding: 10px;width: auto !important;width: 580px;} .content {box-sizing: border-box;display: block;Margin: 0 auto;max-width: 580px;padding: 10px;} .main {background: #fff;border-radius: 3px;width: 100%;} .wrapper {box-sizing: border-box;padding: 20px;} .footer {clear: both;padding-top: 10px;text-align: center;width: 100%;}.footer td,.footer p,.footer span,.footer a { color: #999999;font-size: 12px;text-align: center;} h1, h2, h3, h4 {color: #000000;font-family: sans-serif;font-weight: 400;line-height: 1.4;margin: 0;Margin-bottom: 30px;} h1 {font-size: 35px;font-weight: 300;text-align: center;text-transform: capitalize;} p, ul, ol {font-family: sans-serif;font-size: 14px;font-weight: normal;margin: 0;Margin-bottom: 15px;}p li,ul li,ol li { list-style-position: inside;margin-left: 5px;} a {color: #3498db;text-decoration: underline;}.btn {box-sizing: border-box;width: 100%;}.btn > tbody > tr > td { padding-bottom: 15px;}.btn table { width: auto;}.btn table td { background-color: #ffffff;border-radius: 5px;text-align: center;}.btn a { background-color: #ffffff;border: solid 1px #3498db;border-radius: 5px;box-sizing: border-box;color: #3498db;cursor: pointer;display: inline-block;font-size: 14px;font-weight: bold;margin: 0;padding: 12px 25px;text-decoration: none;text-transform: capitalize;} .btn-primary table td {background-color: #3498db;} .btn-primary a {background-color: #3498db;border-color: #3498db;color: #ffffff;}.last {margin-bottom: 0;} .first {margin-top: 0;} .align-center {text-align: center;} .align-right {text-align: right;} .align-left {text-align: left;} .clear {clear: both;} .mt0 {margin-top: 0;} .mb0 {margin-bottom: 0;} .preheader {color: transparent;display: none;height: 0;max-height: 0;max-width: 0;opacity: 0;overflow: hidden;mso-hide: all;visibility: hidden;width: 0;} .powered-by a {text-decoration: none;} hr {border: 0;border-bottom: 1px solid #f6f6f6;Margin: 20px 0;} @media only screen and (max-width: 620px) {table[class=body] h1 { font-size: 28px !important;margin-bottom: 10px !important;}table[class=body] p,table[class=body] ul,table[class=body] ol,table[class=body] td,table[class=body] span,table[class=body] a { font-size: 16px !important;}table[class=body] .wrapper,table[class=body] .article { padding: 10px !important;}table[class=body] .content { padding: 0 !important;}table[class=body] .container { padding: 0 !important;width: 100% !important;}table[class=body] .main { border-left-width: 0 !important;border-radius: 0 !important;border-right-width: 0 !important;}table[class=body] .btn table { width: 100% !important;}table[class=body] .btn a { width: 100% !important;}table[class=body] .img-responsive { height: auto !important;max-width: 100% !important;width: auto !important;}} @media all {.ExternalClass { width: 100%;}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div { line-height: 100%;}.apple-link a { color: inherit !important;font-family: inherit !important;font-size: inherit !important;font-weight: inherit !important;line-height: inherit !important;text-decoration: none !important;} .btn-primary table td:hover { background-color: #34495e !important;}.btn-primary a:hover { background-color: #34495e !important;border-color: #34495e !important;} } </style></head><body class=\"\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\"><tr><td>&nbsp;</td><td class=\"container\"><div class=\"content\"><table class=\"main\"><tr><td class=\"wrapper\">";
        public string profile_name { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
        public string body_format { get; set; }
        private List<string> blind_copy_recipients { get; set; }
        private List<string> copy_recipients { get; set; }
        private List<string> recipients { get; set; }
        public bool useTemplateHtmlBody { get; set; }
        public EmailDatabase()
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.body_format = "Text";
        }
        public EmailDatabase(string recipient, string subject, string body)
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.recipients.Add(recipient);
            this.subject = subject;
            this.body = body;
            this.body_format = "Text";
        }
        public EmailDatabase(List<string> recipients, string subject, string body)
        {
            this.recipients = new List<string>();
            this.copy_recipients = new List<string>();
            this.blind_copy_recipients = new List<string>();
            this.recipients = recipients;
            this.subject = subject;
            this.body = body;
            this.body_format = "Text";
        }
        public void addRecipients(string Email)
        {
            this.recipients.Add(Email);
        }
        public void addRecipients(List<string> Email)
        {
            this.recipients.AddRange(Email);
        }
        public void addCopy_recipients(string Email)
        {
            this.copy_recipients.Add(Email);
        }
        public void addCopy_recipients(List<string> Email)
        {
            this.copy_recipients.AddRange(Email);
        }
        public void addBlind_copy_recipients(string Email)
        {
            this.blind_copy_recipients.Add(Email);
        }
        public void addBlind_copy_recipients(List<string> Email)
        {
            this.blind_copy_recipients.AddRange(Email);
        }
        public void setUseHtmlFormatBody(bool HtmlFormatBody)
        {
            this.body_format = HtmlFormatBody ? "HTML" : null;
            this.useTemplateHtmlBody = HtmlFormatBody;
        }
        public void Send()
        {
            try
            {
                string _body = this.useTemplateHtmlBody ? templateHeader + this.body + templateFooter : this.body;
                string _recepients = String.Join(";", this.recipients.ToArray());
                string _copy_recepients = String.Join(";", this.copy_recipients.ToArray());
                string _blind_copy_recepients = String.Join(";", this.blind_copy_recipients.ToArray());
                var Params = new object[] { 
                new SqlParameter("@SubjectMail", this.subject),
                new SqlParameter("@AlertTo", _recepients),
                new SqlParameter("@AlertCC", String.IsNullOrWhiteSpace(_copy_recepients) ? (object)DBNull.Value : _copy_recepients ),
                new SqlParameter("@AlertBCC", String.IsNullOrWhiteSpace(_blind_copy_recepients) ? (object)DBNull.Value : _blind_copy_recepients ),
                new SqlParameter("@Body",_body),
                new SqlParameter("@body_format",this.body_format)
                };
                _query.ExecNonQuery("[sp_notif_email] @SubjectMail, @AlertTo, @AlertCC, @AlertBCC, @Body, @body_format", Params);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }

    public enum AttachmentType { 
        excel,
        pdf,
        Word
    }
}
