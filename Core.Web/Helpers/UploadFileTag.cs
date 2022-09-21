using HtmlTags;

namespace Core.Web.Helpers
{
	public class FileUploadTag : HtmlTag
    {
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public FileUploadTag(string data)
            : base("div")
        {
            _data = data;

            this.AddClasses("form-group");
            //.Attr("style", "margin-bottom:4px");                    

            DivInputTag = new HtmlTag("div")
               .AddClass(inputSize);

            LabelTag = new HtmlTag("label")
               .AddClass("control-label")
               .AddClass(labelSize)
               .Attr("for", data.Replace(".", ""))
               .Text(string.Empty);

            InputTag = new HtmlTag("file-upload");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".", ""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);   
        }
        public new FileUploadTag Title(string title)
        {
            LabelTag.Text(title);
            InputTag.Attr("title", title);
            return this;
		}
        public FileUploadTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size);
            return this;
        }
        public FileUploadTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        } 
        public FileUploadTag Folder(string folder)
        {
            InputTag.Attr("folder", folder);
            return this;
        }
        public FileUploadTag Error(string error)
        {
            InputTag.Attr("error", error);
            return this;
        }
        public FileUploadTag MaxSize(string maxSize)
        {
            InputTag.Attr("max-size", maxSize);
            return this;
        }
        public FileUploadTag Accept(string accept)
        {
            InputTag.Attr("accept", accept);
            return this;
        }
        public FileUploadTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);
            return this;
        }
        public FileUploadTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);
            return this;
        }
        public FileUploadTag SetFile(string SetFile)
        {
            InputTag.Attr("set-file", SetFile);
            return this;
        }
        public FileUploadTag AddValidation(string functionName, string message)
        {
            if (!this.HasAttr("form-group-validation"))
            {
                this.Attr("form-group-validation", _data.Replace(".", ""));
            }
            DivInputTag.AddValidation(functionName, message);
            return this;
        }
    }
}