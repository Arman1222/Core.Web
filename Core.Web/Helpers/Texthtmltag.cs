using HtmlTags;

namespace Core.Web.Helpers
{
    public enum editorFitur {
        simple,
        fullfitur,
        standart
    }
	public class Texthtmltag : HtmlTag
	{
        public HtmlTag LabelTag { get; set; }
        public HtmlTag InputTag { get; set; }
        public Texthtmltag(string data)
            : base("my-texthtml")
        {
            this.Attr("ng-model", data);
        }
        public new Texthtmltag Title(string title)
        {
            this.Attr("title", title);
            return this;
        }
        public Texthtmltag Hint(string hint)
        {
            this.Attr("hint", hint);
            return this;
        }
        public Texthtmltag OnChange(string onChange)
        {
            this.Attr("ng-change", onChange);
            return this;
        }
        public Texthtmltag OnBlur(string onBlur)
        {
            this.Attr("ng-blur", onBlur);
            return this;
        }
        public Texthtmltag Disabled(string ngDisabled)
        {
            this.Attr("ng-disabled", ngDisabled);
            return this;
        }
        public Texthtmltag editorFitur(editorFitur editorFitur)
        {
            this.Attr("type", editorFitur);
            return this;
        }
	}
}