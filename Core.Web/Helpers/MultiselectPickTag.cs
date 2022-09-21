using HtmlTags;

namespace Core.Web.Helpers
{
	public class MultiselectPickTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        
        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public MultiselectPickTag(string data)
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

            InputTag = new HtmlTag("multiple-autocomplete-pick");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".",""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);            

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);
        }
        public MultiselectPickTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public MultiselectPickTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new MultiselectPickTag Title(string title)
		{
            LabelTag.Text(title);          
            InputTag.Attr("title", title);
			return this;
		}
        public MultiselectPickTag ListService(string ListItem)
        {
            InputTag.Attr("list-service", ListItem);
            return this;
        }
        public new MultiselectPickTag Name(string name)
        {
            InputTag.Attr("object-property", name);
            return this;
        }
        public MultiselectPickTag SortBy(string sortBy)
        {
            InputTag.Attr("sort-by", sortBy);

            return this;
        }
        public MultiselectPickTag ShowColumns(string showColumns)
        {
            InputTag.Attr("show-columns", showColumns);

            return this;
        }
        public MultiselectPickTag HideColumns(string hideColumns)
        {
            InputTag.Attr("hide-columns", hideColumns);

            return this;
        }
        public MultiselectPickTag SearchParams(string searchParams)
        {
            InputTag.Attr("search-params", searchParams);

            return this;
        }
        public MultiselectPickTag OnSelect(string onSelectFunction)
        {
            InputTag.Attr("after-select-item", onSelectFunction);
            return this;
        }
        public MultiselectPickTag BeforeSelect(string BeforeSelectFunction)
        {
            InputTag.Attr("before-select-item", BeforeSelectFunction);
            return this;
        }
        public MultiselectPickTag OnRemove(string onRemoveFunction)
        {
            InputTag.Attr("after-remove-item", onRemoveFunction);
            return this;
        }
        public MultiselectPickTag BeforeRemove(string BeforeRemoveFunction)
        {
            InputTag.Attr("before-remove-item", BeforeRemoveFunction);
            return this;
        }
        public MultiselectPickTag Disabled(string ngDisabled = "false")
        {
            InputTag.Attr("ng-disabled", ngDisabled);
            return this;
        }
        public MultiselectPickTag AddValidation(string functionName, string message)
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