using Core.Web.Utilities;
using HtmlTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;

namespace Core.Web.Helpers
{
	public class GridTag : HtmlTag
	{
		public class ColumnBuilder<T>
		{
			private readonly GridTag _tag;

			public ColumnBuilder(GridTag tag)
			{
				_tag = tag;
			}
            public void Add<TProp>(Expression<Func<T, TProp>> property,
                string columnHeader = null,
                string cellFilter = null,
                string cellTemplate = null,
                //string cellClass = null,                
                int? width = null,
                bool enableCellEdit = false,
                string type = null,
                bool enableFiltering = false
                )
            {
                var columnDefinition = new ColumnDefinition();
                columnDefinition.Name = columnHeader;
                if (width != null)
                {
                    columnDefinition.Width = width;
                }

                if (!string.IsNullOrEmpty(cellTemplate)) //jika ada button action di grid
                {
                    columnDefinition.CellTemplate = cellTemplate;
                }
                columnDefinition.Field = property.ToCamelCaseName();
                columnDefinition.CellFilter = cellFilter;
                columnDefinition.enableCellEdit = enableCellEdit;
                columnDefinition.type = type;
                columnDefinition.enableFiltering = enableFiltering;
                _tag._columns.Add(columnDefinition);
            }
            public void Add(string property,
                string columnHeader = null,
                string cellFilter = null,
                string cellTemplate = null,
                //string cellClass = null,                
                int? width = null,
                bool enableCellEdit = false,
                string type = null,
                bool enableFiltering = false
                )
            {
                var columnDefinition = new ColumnDefinition();
                columnDefinition.Name = columnHeader;
                if (width != null)
                {
                    columnDefinition.Width = width;
                }

                if (!string.IsNullOrEmpty(cellTemplate)) //jika ada button action di grid
                {
                    columnDefinition.CellTemplate = cellTemplate;
                }
                columnDefinition.Field = property;
                columnDefinition.CellFilter = cellFilter;
                columnDefinition.enableCellEdit = enableCellEdit;
                columnDefinition.type = type;
                columnDefinition.enableFiltering = enableFiltering;
                _tag._columns.Add(columnDefinition);
            }
            public void AddExtra(string property,
                string columnHeader = null,
                string cellFilter = null,
                string cellTemplate = null,
                //string cellClass = null,                
                int? width = null,
                bool enableCellEdit = false,
                string type = null,
                bool enableFiltering = false,
                bool leftDirection = false
                )
            {
                var columnDefinition = new ExtraColumnDefinition();
                columnDefinition.Name = columnHeader;
                if (width != null)
                {
                    columnDefinition.Width = width;
                }
                if (!string.IsNullOrEmpty(cellTemplate)) //jika ada button action di grid
                {
                    columnDefinition.CellTemplate = cellTemplate;
                }
                columnDefinition.Field = property;
                columnDefinition.CellFilter = cellFilter;
                columnDefinition.enableCellEdit = enableCellEdit;
                columnDefinition.type = type;
                columnDefinition.enableFiltering = enableFiltering;
                columnDefinition.leftdirection = leftDirection;
                _tag._extracolumns.Add(columnDefinition);
            }
		}
		private class ColumnDefinition
		{
			public string Field { get; set; }
			public string Name { get; set; }
			public string CellFilter { get; set; }
            public string CellClass { get; set; }
            public string CellTemplate { get; set; }
            public int? Width { get; set; }
            public bool enableCellEdit { get; set; }
            public bool enableFiltering { get; set; }
            public string type { get; set; }
		}

        private class ExtraColumnDefinition
        {
            public ExtraColumnDefinition()
            {
                this.leftdirection = true;
            }
            public string Field { get; set; }
            public string Name { get; set; }
            public string CellFilter { get; set; }
            public string CellClass { get; set; }
            public string CellTemplate { get; set; }
            public int? Width { get; set; }
            public bool enableCellEdit { get; set; }
            public bool enableFiltering { get; set; }
            public string type { get; set; }
            public bool leftdirection { get; set; }
        }

        private class validators
        {
            public bool required { get; set; }
            public string startWith { get; set; }
        }
        private readonly List<ColumnDefinition> _columns = new List<ColumnDefinition>();
        private readonly List<ExtraColumnDefinition> _extracolumns = new List<ExtraColumnDefinition>();

        //public GridTag(string dataUrl)
        //    : base("mvc-grid")
        //{
        //    Attr("grid-data-url", dataUrl);
        //}

        public GridTag(string gridData)
            : base("mvc-grid")
        {
            Attr("grid-data", gridData);            
        }
        public new GridTag GetAll(string gridDataAll)
        {
            Attr("grid-data-all", gridDataAll);
            return this;
        }
		public new GridTag Title(string title)
		{
			Attr("title", title);
			return this;
		}
        public new GridTag Init(string init)
        {
            Attr("init", init);
            return this;
        }
        public new GridTag Message(string message)
        {
            Attr("message", message);
            return this;
        }
        public new GridTag TableActions(string name)
        {
            Attr("table-actions", name);
            return this;
        }
        public GridTag SelectColumn(bool isSelectColumn)
        {
            Attr("select-column", isSelectColumn.ToString());
            return this;
        }
        public GridTag RefreshData(string refresh)
        {
            Attr("refresh-data", refresh);
            return this;
        }
        public GridTag AddData(string AddData)
        {
            Attr("add-data", AddData);
            return this;
        }
        public GridTag SearchParams(string searchParams)
        {
            Attr("search-params", searchParams);

            return this;
        }
        public GridTag ScopeParams(string scopeParams)
        {
            Attr("scope-params", scopeParams);

            return this;
        }
        public GridTag SelectedRows(string selectedRows)
        {
            Attr("selected-rows", selectedRows);
            return this;
        }
        public GridTag TotalItems(string totalItems)
        {
            Attr("total-items", totalItems);
            return this;
        }
        public GridTag OnLoad(string onLoad)
        {
            Attr("on-load", onLoad);
            return this;
        }
        public GridTag Response(string response)
        {
            Attr("response", response);
            return this;
        }
        public GridTag AutoLoad(bool autoLoad)
        {
            Attr("auto-load", autoLoad);
            return this;
        }
        public GridTag EnableSelect(bool enableSelect)
        {
            Attr("enable-select", enableSelect);
            return this;
        }
        public GridTag EnableFilter(bool enableFilter = true)
        {
            Attr("enable-filter", enableFilter);
            return this;
        }
        public GridTag RowStyle(string rowStyleFunction)
        {
            Attr("row-style", rowStyleFunction);
            return this;
        }
		public GridTag Columns<T>(Action<ColumnBuilder<T>> configAction)
		{
			var builder = new ColumnBuilder<T>(this);
			configAction(builder);
			return this;
		}
		protected override void writeHtml(HtmlTextWriter html)
		{
			if (_columns.Any())
                this.Attr("columns", _columns.ToArray().ToJson(includeNull: false));
            if (_extracolumns.Any())
                this.Attr("extra-columns", _extracolumns.ToArray().ToJson(includeNull: false));
			base.writeHtml(html);
		}
        public GridTag Disabled(string ngDisabled)
        {
            Attr("ng-disabled", ngDisabled);
            return this;
        }
        public GridTag CsvFilename(string filename)
        {
            Attr("csv-filename", filename);
            return this;
        }
        public GridTag HideSearch()
        {
            Attr("hide-search", true);
            return this;
        }
        public GridTag EditCell(bool editable)
        {
            Attr("editable-grid", editable);
            return this;
        }
        public GridTag onBlurEditedCell(string action)
        {
            this.Attr("on-blur-edited-cell", action);
            return this;
        }
        //20190711, jeni, begin
        public GridTag HideMinimize()
        {
            Attr("hide-minimize", true);
            return this;
        }
        public GridTag HideClose()
        {
            Attr("hide-close", true);
            return this;
        }
        //20190711, jeni, end
	}
}