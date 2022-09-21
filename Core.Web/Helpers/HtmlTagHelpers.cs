using HtmlTags;
using System;

namespace Core.Web.Helpers
{
    public static class HtmlTagHelpers
    {
        //public static HtmlTag GetTagName(this HtmlTag tag)
        //{
        //    //check if textarea
        //    if (tag.HasMetaData()
        //    {
        //        inputTag = new HtmlTag("textarea")
        //            .AddClass("form-control");
        //    }
        //    //check if enum type
        //    else if (metadata.ModelType.BaseType == typeof(Enum))
        //    {
        //        inputTag = new HtmlTag("select")
        //        .AddClass("form-control");
        //    }
        //    else if (metadata.DataTypeName == "Time")
        //    {
        //        inputTag = new HtmlTag("timepicker-pop")
        //            .AddClass("input-group")
        //            .Attr("input-time", expression)
        //            .Attr("show-meridian", "showMeridian");
        //    }
        //    else if (metadata.ModelType == typeof(DateTime?) || metadata.ModelType == typeof(DateTime)) //check if datetime
        //    {
        //        inputTag = new HtmlTag("ng-bs3-datepicker")
        //            .Attr("date-format", "YYYY-MM-DD");
        //    }
        //    else if (metadata.ModelType == typeof(string))
        //    {
        //        inputTag = new HtmlTag("input")
        //        .Attr("type", "text")
        //        .AddClass("form-control");
        //    }

        //    tag.TagName =
        //    tag.Attr("ng-change", action);
        //    return tag;
        //}

        public static string LABEL_SIZE = "col-sm-2";
        public static string INPUT_SIZE = "col-sm-10";

        public static HtmlTag FindModelChildTag(this HtmlTag tag)
        {
            if (tag.HasAttr("ng-model") || tag.HasAttr("model")){
                return tag;
            }else if(tag.Children.Count > 0){                
                foreach(var tagChild in tag.Children){
                    var tagInput = tagChild.FindModelChildTag();
                    if (tagInput != null)
                    {
                        return tagInput;
                    }
                }
            }
            return null;
        }
        public static HtmlTag OnChange(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-change", action);
            }            
            return tag;
        }
        public static HtmlTag OnClick(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-click", action);
            }
            return tag;
        }

        public static HtmlTag SetClass(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-class", action);
            }
            return tag;
        }
        public static HtmlTag ShowIf(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-if", action);
            }
            return tag;
        }

        public static HtmlTag HideIf(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-hide", action);
            }
            return tag;
        }
        public static HtmlTag OnBlur(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-blur", action);
            }
            return tag;
        }
        public static HtmlTag OnKeyDown(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-keydown", action);
            }
            return tag;
        }
        public static HtmlTag OnKeyPress(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-keypress", action);
            }
            return tag;
        }
        public static HtmlTag OnKeyUp(this HtmlTag tag, string action)
        {
            //HtmlTag tagInput = tag.Children.FirstOrDefault(h => h.HasAttr("ng-model"));
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-keyup", action);
            }
            return tag;
        }
        public static HtmlTag Disabled(this HtmlTag tag, string expression)
        {            
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-disabled", expression);
            }
            return tag;
        }
        public static HtmlTag MinDate(this HtmlTag tag, string expression)
        {
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("min-date", expression);
            }
            return tag;
        }
        public static HtmlTag MaxDate(this HtmlTag tag, string expression)
        {
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("max-date", expression);
            }
            return tag;
        }
        public static HtmlTag DefaultValue(this HtmlTag tag, string expression)
        {
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-init", tagInput.Attr("ng-model") + " = '" + expression + "'");
            }
            return tag;
        }
        public static HtmlTag DefaultValue(this HtmlTag tag, int expression)
        {
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-init", expression);
            }
            return tag;
        }
        public static HtmlTag DefaultValue(this HtmlTag tag, decimal expression)
        {
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                tagInput.Attr("ng-init", expression);
            }
            return tag;
        }
        public static HtmlTag DefaultValue(this HtmlTag tag, DateTime? date)
        {
            HtmlTag tagInput = tag.FindModelChildTag();
            if (tagInput != null)
            {
                if(date.HasValue)
                    tagInput.Attr("ng-init", ((DateTime)date).ToString("o") );
                
            }
            return tag;
        }
        public static HtmlTag FindHelpBlockChildTag(this HtmlTag tag)
        {
            if (tag.HasClass("help-block"))
            {
                return tag;
            }
            else if (tag.Children.Count > 0)
            {
                foreach (var tagChild in tag.Children)
                {
                    var tagInput = tagChild.FindHelpBlockChildTag();
                    if (tagInput != null)
                    {
                        return tagInput;
                    }
                }
            }
            return null;
        }
        public static HtmlTag FindTagByClassName(this HtmlTag tag, string className)
        {
            if (tag.HasClass(className))
            {
                return tag;
            }
            else if (tag.Children.Count > 0)
            {
                foreach (var tagChild in tag.Children)
                {
                    var tagInput = tagChild.FindTagByClassName(className);
                    if (tagInput != null)
                    {
                        return tagInput;
                    }
                }
            }
            return null;
        }
        public static HtmlTag SetLabelSize(this HtmlTag tag, string size)
        {
            HtmlTag tagInput = tag.FindTagByClassName(HtmlTagHelpers.LABEL_SIZE);
            if (tagInput != null)
            {
                tagInput.RemoveClass(HtmlTagHelpers.LABEL_SIZE);
                tagInput.AddClass(size);
            }
            return tag;
        }
        public static HtmlTag SetInputSize(this HtmlTag tag, string size)
        {
            HtmlTag tagInput = tag.FindTagByClassName(HtmlTagHelpers.INPUT_SIZE);
            if (tagInput != null)
            {
                tagInput.RemoveClass(HtmlTagHelpers.INPUT_SIZE);
                tagInput.AddClass(size);
            }
            return tag;
        }
        public static HtmlTag AddValidation(this HtmlTag tag, string functionName, string message)
        {
            HtmlTag tagModel = tag.FindModelChildTag();
            HtmlTag tagHelp = tag.FindHelpBlockChildTag();
            if (tagHelp != null && tagModel != null)
            {
                var messageTag = new HtmlTag("p")
                //.Attr("ng-show", functionName + "('" + tagModel.Attr("name") + "') && (vm.form." + tagModel.Attr("name") + ".$touched || vm.form.$submitted)")
                .Attr("custom-validation ng-show", "vm.form." + tagModel.Attr("name") + ".$invalid  && (vm.form." + tagModel.Attr("name") + ".$touched || vm.form.$submitted)")
                .Attr("ng-model", tagModel.Attr("ng-model"))
                .Attr("ng-form", "vm.form." + tagModel.Attr("name"))
                .Attr("element-name", tagModel.Attr("name"))
                .Attr("validate-function", functionName)
                .AppendHtml(message);

                tagHelp.Append(messageTag);
            }
            return tag;
        }
    }
}