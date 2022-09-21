using HtmlTags;

namespace Core.Web.Helpers
{
	public class MultiselectTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        
        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public MultiselectTag(string data)
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

            InputTag = new HtmlTag("multiple-autocomplete");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".",""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);            

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);
        }
        public MultiselectTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public MultiselectTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new MultiselectTag Title(string title)
		{
            LabelTag.Text(title);          
            InputTag.Attr("title", title);
			return this;
		}
        public MultiselectTag ListItem(string ListItem)
        {
            InputTag.Attr("suggestions-list", ListItem);
            return this;
        }       
        public new MultiselectTag Name(string name)
        {
            InputTag.Attr("object-property", name);
            return this;
        }
        public MultiselectTag OnSelect(string onSelectFunction)
        {
            InputTag.Attr("after-select-item", onSelectFunction);
            return this;
        }
        public MultiselectTag BeforeSelect(string BeforeSelectFunction)
        {
            InputTag.Attr("before-select-item", BeforeSelectFunction);
            return this;
        }
        public MultiselectTag OnRemove(string onRemoveFunction)
        {
            InputTag.Attr("after-remove-item", onRemoveFunction);
            return this;
        }
        public MultiselectTag BeforeRemove(string BeforeRemoveFunction)
        {
            InputTag.Attr("before-remove-item", BeforeRemoveFunction);
            return this;
        }
        public MultiselectTag Disabled(string ngDisabled = "false")
        {
            InputTag.Attr("ng-disabled", ngDisabled);
            return this;
        }
        public MultiselectTag AddValidation(string functionName, string message)
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