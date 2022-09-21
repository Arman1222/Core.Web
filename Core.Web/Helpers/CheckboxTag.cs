using HtmlTags;

namespace Core.Web.Helpers
{
	public class CheckboxTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag DivInputTag { get; set; }
        public CheckboxTag(string data)
            : base("div")
        {

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
            .Attr("type", "checkbox")
            .Attr("ng-model", data);

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);
            this.Append(DivInputTag);    
        }

        public new CheckboxTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);      
			return this;
		}
        public CheckboxTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size);           
            return this;
        }
        public CheckboxTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public CheckboxTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);
            return this;
        }
        public CheckboxTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);
            return this;
        }
        public CheckboxTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);
            return this;
        }
	}
}