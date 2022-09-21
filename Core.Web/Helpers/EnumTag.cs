using HtmlTags;
using System;

namespace Core.Web.Helpers
{
	public class EnumTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";

        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public EnumTag(string data)
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

            InputTag = new HtmlTag("select")
            .AddClass("form-control")
            .Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".", ""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);    
        }
        public EnumTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public EnumTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public EnumTag EnumType(string enumType)
        {
            foreach (var item in Enum.GetValues(Type.GetType(enumType)))
            {
                InputTag.Append(new HtmlTag("option")
                .Attr("value", item.ToString()).AppendHtml(item.ToString()));
            }
            return this;
        }
        public new EnumTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);          
           
			return this;
		}
        public EnumTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);

            return this;
        }
        public EnumTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);

            return this;
        }
        public EnumTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }
        public EnumTag AddValidation(string functionName, string message)
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