using HtmlTags;

namespace Core.Web.Helpers
{
	public class UploadFileTag : HtmlTag
	{
        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public UploadFileTag(string data)
            : base("my-upload-file")
        {
            _data = data;      
            this.Attr("ng-model", data);   
        }
        public new UploadFileTag Title(string title)
		{           
            this.Attr("title", title);
			return this;
		}
        public UploadFileTag LabelSize(string size)
        {
            this.Attr("label-size", size);
            return this;
        }
        public UploadFileTag InputSize(string size)
        {
            this.Attr("input-size", size);
            return this;
        } 
        public UploadFileTag Path(string path)
        {
            this.Attr("path", path);
            return this;
        }
        public UploadFileTag File(string file)
        {
            this.Attr("file", file);
            return this;
        }
        public UploadFileTag Response(string response)
        {
            this.Attr("response", response);
            return this;
        }
        public UploadFileTag Error(string error)
        {
            this.Attr("error", error);
            return this;
        }
        public UploadFileTag MaxSize(string maxSize)
        {
            this.Attr("max-size", maxSize);
            return this;
        }
        public UploadFileTag Accept(string accept)
        {
            this.Attr("accept", accept);
            return this;
        }
        public UploadFileTag OnChange(string onChange)
        {
            this.Attr("ng-change", onChange);
            return this;
        }
        public UploadFileTag Disabled(string ngDisabled)
        {
            this.Attr("ng-disabled", ngDisabled);
            return this;
        }       
	}
}