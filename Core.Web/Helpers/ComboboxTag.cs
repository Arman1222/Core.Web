using HtmlTags;

namespace Core.Web.Helpers
{
	public class ComboboxTag : HtmlTag
	{
        private string _data;
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public ComboboxTag(string data)
            : base("div")
        {
            _data = data;

            this.AddClasses("form-group")
           .Attr("style", "margin-bottom:4px");                 

            DivInputTag = new HtmlTag("div")
               .AddClass(inputSize);

            LabelTag = new HtmlTag("label")
               .AddClass("control-label")
               .AddClass(labelSize)
               .Attr("for", data.Replace(".", ""))
               .Text(string.Empty);

            InputTag = new HtmlTag("my-combobox");
            //InputTag.Attr("model", data);
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".", ""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);   
        }
        public ComboboxTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size);       
            return this;
        }
        public ComboboxTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new ComboboxTag Title(string title)
		{
            LabelTag.Attr("for", title)
               .Text(title);

            InputTag.Attr("title", title);

			return this;
		}
        public new ComboboxTag Name(string name)
        {
            InputTag.Attr("name-column", name);

            return this;
        }

        public new ComboboxTag Id(string id)
        {
            InputTag.Attr("id-column", id);

            return this;
        }

        public ComboboxTag Items(string items)
        {
            InputTag.Attr("items", items);

            return this;
        }

        public ComboboxTag OnSelect(string onSelect)
        {
            InputTag.Attr("on-select", onSelect);

            return this;
        }
        public ComboboxTag OnBlur(string onBlur)
        {
            InputTag.Attr("ng-blur", onBlur);
            return this;
        }
        public ComboboxTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }
        public ComboboxTag AddValidation(string functionName, string message)
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