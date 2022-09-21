using HtmlTags;

namespace Core.Web.Helpers
{
	public class TimeTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public TimeTag(string data)
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

            InputTag = new HtmlTag("timepicker-pop")
            .AddClass("input-group")
            .Attr("input-time", data);

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);
            this.Append(DivInputTag);    
        }
        public TimeTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public TimeTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new TimeTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);        

			return this;
		}

        public new TimeTag ShowMeridian(bool showMeridian)
		{   
            InputTag.Attr("show-meridian", showMeridian);
		    return this;
	    }
    
        public TimeTag OnChange(string onChange)
        {
            InputTag.Attr("ng-change", onChange);
            return this;
        }

        public TimeTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);
            return this;
        }
       
	}
}