using HtmlTags;

namespace Core.Web.Helpers
{
	public class DateTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";

        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public DateTag(string data)
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

            InputTag = new HtmlTag("my-datepicker");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".", ""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);    
        }
        public DateTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public DateTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new DateTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);
           
            InputTag.Attr("title", title);

			return this;
		}
        public DateTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);

            return this;
        }
        public DateTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);

            return this;
        }
        public DateTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }
        public DateTag MinDate(string minDate)
        {
            InputTag.Attr("min-date", minDate);

            return this;
        }
        public DateTag MaxDate(string maxDate)
        {
            InputTag.Attr("max-date", maxDate);

            return this;
        }
        public DateTag DateFormat(string dateFormat)
        {
            InputTag.Attr("date-format", dateFormat);

            return this;
        }
        public DateTag YearRange(string yearRange)
        {
            InputTag.Attr("year-range", yearRange);

            return this;
        }
        public DateTag AddValidation(string functionName, string message)
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