using HtmlTags;

namespace Core.Web.Helpers
{
	public class TextTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";

        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        public HtmlTag DivInputTag { get; set; }
        public TextTag(string data)
            : base("div")
        {
            _data = data;

            this.AddClasses("form-group");
            //.Attr("style","margin-bottom:4px");
            //.Attr("form-group-validation", name);           

            DivInputTag = new HtmlTag("div")
               .AddClass(inputSize);

            LabelTag = new HtmlTag("label")
               .AddClass("control-label")
               .AddClass(labelSize)
               .Attr("for", string.Empty)
               .Text(string.Empty);

            InputTag = new HtmlTag("input")
            .AddClass("form-control")
            .Attr("type", "text")
            .Attr("ng-model", data);

            //this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);    
        }
        public TextTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size);
            return this;
        }
        public TextTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new TextTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);
            this.InsertFirst(LabelTag);
           
			return this;
		}
        public TextTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);

            return this;
        }
        public TextTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);

            return this;
        }
        public TextTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }
        public TextTag AddValidation(string functionName, string message)
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