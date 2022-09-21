using HtmlTags;

namespace Core.Web.Helpers
{
	public class PicklistTag : HtmlTag
	{
        private const string labelSize = "col-sm-2";
        private const string inputSize = "col-sm-10";
        
        private string _data;
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public HtmlTag ErrorTag { get; set; }
        private HtmlTag DivInputTag { get; set; }
        public PicklistTag(string data)
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

            InputTag = new HtmlTag("my-picklist");
            InputTag.Attr("ng-model", data);
            InputTag.Attr("name", data.Replace(".",""));

            this.Append(LabelTag);
            DivInputTag.Append(InputTag);            

            ErrorTag = new HtmlTag("div")
              .AddClass("help-block");
            DivInputTag.Append(ErrorTag);

            this.Append(DivInputTag);
        }
        public PicklistTag LabelSize(string size)
        {
            LabelTag.RemoveClass(labelSize);
            LabelTag.AddClass(size); 
            return this;
        }
        public PicklistTag InputSize(string size)
        {
            DivInputTag.RemoveClass(inputSize);
            DivInputTag.AddClass(size);
            return this;
        }
        public new PicklistTag Title(string title)
		{
            LabelTag.Text(title);          
            InputTag.Attr("title", title);
			return this;
		}
        public PicklistTag Picklist(string picklist)
        {
            InputTag.Attr("picklist", picklist);

            return this;
        }
        public PicklistTag ListService(string listService)
        {
            InputTag.Attr("list-service", listService);

            return this;
        }       
        public new PicklistTag Id(string id)
        {
            InputTag.Attr("id-column", id);

            return this;
        }
        public new PicklistTag Name(string name)
        {
            InputTag.Attr("name-column", name);

            return this;
        }
        public PicklistTag SelectedId(string selectedId)
        {
            InputTag.Attr("selected-id", selectedId);

            return this;
        }
        public PicklistTag SortBy(string sortBy)
        {
            InputTag.Attr("sort-by", sortBy);

            return this;
        }
        public PicklistTag OnSelect(string onSelect)
        {
            InputTag.Attr("on-select", onSelect);

            return this;
        }
        public PicklistTag Message(string message)
        {
            InputTag.Attr("message", message);

            return this;
        }
        public PicklistTag Disabled(string ngDisabled)
        {
            InputTag.Attr("ng-disabled", ngDisabled);

            return this;
        }
        public PicklistTag SearchParams(string searchParams)
        {
            InputTag.Attr("search-params", searchParams);

            return this;
        }
        public PicklistTag RefreshData(string refresh)
        {
            InputTag.Attr("refresh-data", refresh);

            return this;
        }
        public PicklistTag TotalItems(string totalItems)
        {
            Attr("total-items", totalItems);

            return this;
        }
        public PicklistTag OnLoad(string onLoad)
        {
            Attr("on-load", onLoad);

            return this;
        }
        public PicklistTag Response(string response)
        {
            Attr("response", response);

            return this;
        }
        public PicklistTag ShowColumns(string showColumns)
        {
            InputTag.Attr("show-columns", showColumns);

            return this;
        }
        public PicklistTag HideColumns(string hideColumns)
        {
            InputTag.Attr("hide-columns", hideColumns);

            return this;
        }
        public PicklistTag AddValidation(string functionName, string message)
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