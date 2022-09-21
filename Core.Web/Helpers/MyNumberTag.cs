using HtmlTags;

namespace Core.Web.Helpers
{
	public class MyNumberTag : HtmlTag
	{
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public MyNumberTag(string data)
            : base("my-number")
        {          
            this.Attr("ng-model", data);         
        }

        public new MyNumberTag Title(string title)
		{            
            this.Attr("title", title);
			return this;
		}

        public MyNumberTag Min(string min)
        {
            this.Attr("min", min);
            return this;
        }
        public MyNumberTag Max(string max)
        {
            this.Attr("max", max);

            return this;
        }
        public MyNumberTag MaxDecimals(string maxDecimals)
        {
            this.Attr("max-decimals", maxDecimals);

            return this;
        }
        public MyNumberTag Prepend(string prepend)
        {
            this.Attr("prepend", prepend);

            return this;
        }
        public MyNumberTag Append(string append)
        {
            this.Attr("append", append);

            return this;
        }
        public MyNumberTag Hint(string hint)
        {
            this.Attr("hint", hint);

            return this;
        }

        public MyNumberTag OnChange(string onChange)
        {
            this.Attr("ng-change", onChange);

            return this;
        }

        public MyNumberTag OnBlur(string onBlur)
        {
            this.Attr("ng-blur", onBlur);

            return this;
        }

        public MyNumberTag OnKeyPress(string onKeyPress)
        {
            this.Attr("ng-keydown", onKeyPress);

            return this;
        }

        public MyNumberTag Disabled(string ngDisabled)
        {
            this.Attr("ng-disabled", ngDisabled);

            return this;
        }
	}
}