using HtmlTags;

namespace Core.Web.Helpers
{
	public class NumberTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public NumberTag(string data)
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

            InputTag = new HtmlTag("number-input");
            InputTag.Attr("ng-model", data);

            //this.Append(LabelTag);
            DivInputTag.Append(InputTag);
            this.Append(DivInputTag);    
        }
        public NumberTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size);
            return this;
        }
        public NumberTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new NumberTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);           
            this.InsertFirst(LabelTag);
           
            InputTag.Attr("title", title);

			return this;
		}

        public NumberTag Min(string min)
        {
            InputTag.Attr("min", min);

            return this;
        }
        public NumberTag Max(string max)
        {
            InputTag.Attr("max", max);

            return this;
        }
        public NumberTag Start(string start)
        {
            InputTag.Attr("start", start);

            return this;
        }
        public NumberTag Hint(string hint)
        {
            InputTag.Attr("hint", hint);

            return this;
        }

        public NumberTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);

            return this;
        }

        public NumberTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);

            return this;
        }

        public NumberTag OnKeyPress(string onKeyPress)
        {
            InputTag.Attr("ng-keydown", onKeyPress);

            return this;
        }

        public NumberTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }

        public NumberTag DisableDecimal(bool disableDecimal)
        {
            InputTag.Attr("disable-decimal", disableDecimal ? "1" : "0");

            return this;
        }

        public NumberTag DecimalPlaces(int decimalPlaces)
        {
            InputTag.Attr("decimal-places", decimalPlaces );

            return this;
        }

	}
}