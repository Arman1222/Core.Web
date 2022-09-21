using HtmlTags;

namespace Core.Web.Helpers
{
	public class MonthTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";

        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public MonthTag(string data)
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

            InputTag = new HtmlTag("my-monthpicker");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".", ""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);    
        }
        public MonthTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public MonthTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new MonthTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);
           
            InputTag.Attr("title", title);

			return this;
		}
        public MonthTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);

            return this;
        }
        public MonthTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);

            return this;
        }
        public MonthTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }
        public MonthTag MinDate(string minMonth)
        {
            InputTag.Attr("min-date", minMonth);

            return this;
        }
        public MonthTag MaxDate(string maxMonth)
        {
            InputTag.Attr("max-date", maxMonth);

            return this;
        }
        public MonthTag DateFormat(string dateFormat)
        {
            InputTag.Attr("date-format", dateFormat);

            return this;
        }
        public MonthTag YearRange(string yearRange)
        {
            InputTag.Attr("year-range", yearRange);

            return this;
        }
        public MonthTag AddValidation(string functionName, string message)
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