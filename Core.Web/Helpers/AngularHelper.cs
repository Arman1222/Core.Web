using HtmlTags;
using Microsoft.Web.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Core.Web.Helpers
{
	public static class AngularHelperExtension
	{
		public static AngularHelper<TModel> Angular<TModel>(this HtmlHelper<TModel> helper)
		{
			return new AngularHelper<TModel>(helper);
		}
	}
	public class AngularHelper<TModel>
	{
		private readonly HtmlHelper<TModel> _htmlHelper;

		public AngularHelper(HtmlHelper<TModel> helper)
		{
			_htmlHelper = helper;
		}

		public AngularModelHelper<TModel> ModelFor(string expressionPrefix)
		{
			return new AngularModelHelper<TModel>(_htmlHelper, expressionPrefix);
		}

        public HtmlTag FormForModel(string expressionPrefix)
        {
            var modelHelper = ModelFor(expressionPrefix);

            var formGroupForMethodGeneric = typeof(AngularModelHelper<TModel>)
                .GetMethod("FormGroupFor");

            var wrapperTag = new HtmlTag("div").NoTag();

            foreach (var prop in typeof(TModel)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.GetCustomAttributes().OfType<HiddenInputAttribute>().Any()) continue;

                var formGroupForProp = formGroupForMethodGeneric
                    .MakeGenericMethod(prop.PropertyType);

                var propertyLambda = MakeLambda(prop);

                var formGroupTag = (HtmlTag)formGroupForProp.Invoke(modelHelper,
                    new[] { propertyLambda });

                wrapperTag.Append(formGroupTag);
            }

            return wrapperTag;
        }

        //Constructs a lambda of the form x => x.PropName
        private object MakeLambda(PropertyInfo prop)
        {
            var parameter = Expression.Parameter(typeof(TModel), "x");
            var property = Expression.Property(parameter, prop);
            var funcType = typeof(Func<,>).MakeGenericType(typeof(TModel), prop.PropertyType);

            //x => x.PropName
            return Expression.Lambda(funcType, property, parameter);
        }

        public UIRatingTag UIRating(string model)
        {
            return new UIRatingTag(model);
        }
        public HtmlTag AlertGlobal(string errorModel)
        {
            var alertTag = new HtmlTag("form-message")
                .Attr("model", errorModel);                     
            return alertTag;
        }
        public HtmlTag Alert(string errorModel, string infoText = null)
        {
            var infoTag = new HtmlTag("div")
            .AddClass("alert alert-info")
            .Attr("ng-hide",errorModel +" != null || " + (!string.IsNullOrEmpty(infoText) ? "false" : "true" ))
            .AppendHtml(infoText);

            var errorTag = new HtmlTag("div")
            .AddClass("alert alert-danger")
            .Attr("ng-show", errorModel + " != null ")
            .Append(
                new HtmlTag("p")
                .Attr("ng-bind-html", errorModel + " | unsafe") //http://creative-punch.net/2014/04/preserve-html-text-output-angularjs/
            );           

            var alertTag = new HtmlTag("div")
            .Append(infoTag)
            .Append(errorTag);

            return alertTag;          
        }
        public GridTag GridFor<TController>(Expression<Action<TController>> targetAction)
                    where TController : Controller
        {
            var dataUrl = _htmlHelper.BuildUrlFromExpression(targetAction);

            return new GridTag(dataUrl);
        }
        public GridTag GridFor(string gridData)
        {
            return new GridTag(gridData);
        }
        public PicklistTag PicklistFor(string data)
        {           
            return new PicklistTag(data);
        }
        public ComboboxTag ComboboxFor(string data)
        {
            return new ComboboxTag(data);
        }
        public DateTag DateFor(string data)
        {
            return new DateTag(data);
        }
        public MonthTag MonthFor(string data)
        {
            return new MonthTag(data);
        }
        public EnumTag EnumFor(string data)
        {
            return new EnumTag(data);
        }
        public TimeTag TimeFor(string data)
        {
            return new TimeTag(data);
        }
        public NumberTag NumberFor(string data)
        {
            return new NumberTag(data);
        }
        public CheckboxTag CheckboxFor(string data)
        {
            return new CheckboxTag(data);
        }
        public TextTag TextFor(string data)
        {
            return new TextTag(data);
        }
        public UploadDataTag UploadDataFor(string data)
        {
            return new UploadDataTag(data);
        }
        public UploadFileTag UploadFileFor(string data)
        {
            return new UploadFileTag(data);
        }
        public FileUploadTag FileUploadFor(string data)
        {
            return new FileUploadTag(data);
        }
        public Texthtmltag TexthtmlFor(string data)
        {
            return new Texthtmltag(data);
        }
        public MultiselectTag MultiselectFor(string data)
        {
            return new MultiselectTag(data);
        }

        public MultiselectPickTag MultiselectPicklistFor(string data)
        {
            return new MultiselectPickTag(data);
        }
	}
}