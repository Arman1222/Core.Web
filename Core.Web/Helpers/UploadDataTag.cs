using HtmlTags;

namespace Core.Web.Helpers
{
	public class UploadDataTag : HtmlTag
    {
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        private string _data;        
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public UploadDataTag(string data)
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

            InputTag = new HtmlTag("my-upload");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".", ""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);
        }
        public new UploadDataTag Title(string title)
        {
            LabelTag.Text(title);
            InputTag.Attr("title", title);
            return this;
		}
        public UploadDataTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size);
            return this;
        }
        public UploadDataTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public UploadDataTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);
            return this;
        }
        public UploadDataTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);
            return this;
        }
        public new UploadDataTag Data(string data)
        {
            InputTag.Attr("upload-data", data);
            return this;
        }
        public UploadDataTag Columns(string columns)
        {
            InputTag.Attr("columns", columns);
            return this;
        }
        public UploadDataTag DisableHeaders()
        {
            InputTag.Attr("disable-headers", false);
            return this;
        }
        public UploadDataTag Range(string range)
        {
            InputTag.Attr("range", range);
            return this;
        }
        public UploadDataTag Where(string where)
        {
            InputTag.Attr("where", where);
            return this;
        }
        public UploadDataTag AddValidation(string functionName, string message)
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