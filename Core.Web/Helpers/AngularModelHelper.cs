﻿using Core.Web.Utilities;
using HtmlTags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Helpers
{
    public class AngularModelHelper<TModel>
    {
        protected readonly HtmlHelper Helper;
        private readonly string _expressionPrefix;
        private HtmlTag DivInputTag { get; set; }
        public AngularModelHelper(HtmlHelper helper, string expressionPrefix)
        {
            Helper = helper;
            _expressionPrefix = expressionPrefix;
        }

        /// <summary>
        /// Converts an lambda expression into a camel-cased string, prefixed
        /// with the helper's configured prefix expression, ie:
        /// vm.model.parentProperty.childProperty
        /// </summary>
        public IHtmlString ExpressionFor<TProp>(Expression<Func<TModel, TProp>> property)
        {
            var expressionText = ExpressionForInternal(property);
            return new MvcHtmlString(expressionText);
        }

        /// <summary>
        /// Converts a lambda expression into a camel-cased AngularJS binding expression, ie:
        /// {{vm.model.parentProperty.childProperty}} 
        /// </summary>
        public IHtmlString BindingFor<TProp>(Expression<Func<TModel, TProp>> property)
        {
            return MvcHtmlString.Create("{{" + ExpressionForInternal(property) + "}}");
        }

        /// <summary>
        /// Creates a div with an ng-repeat directive to enumerate the specified property,
        /// and returns a new helper you can use for strongly-typed bindings on the items
        /// in the enumerable property.
        /// </summary>
        public AngularNgRepeatHelper<TSubModel> Repeat<TSubModel>(
            Expression<Func<TModel, IEnumerable<TSubModel>>> property, string variableName)
        {
            var propertyExpression = ExpressionForInternal(property);
            return new AngularNgRepeatHelper<TSubModel>(
                Helper, variableName, propertyExpression);
        }

        private string ExpressionForInternal<TProp>(Expression<Func<TModel, TProp>> property)
        {
            var camelCaseName = property.ToCamelCaseName();

            var expression = !string.IsNullOrEmpty(_expressionPrefix)
                ? _expressionPrefix + "." + camelCaseName
                : camelCaseName;

            return expression;
        }

        public static PropertyInfo GetPropertyInfo(ModelMetadata metadata)
        {
            var prop = metadata.ContainerType.GetProperty(metadata.PropertyName);
            return prop;
        }

        public static object[] GetAttributes(ModelMetadata metadata)
        {
            var prop = GetPropertyInfo(metadata);
            var attrs = prop.GetCustomAttributes(false);
            return attrs;
        }

        public static T GetAttribute<T>(ModelMetadata metadata)
            where T : System.Attribute
        {
            var prop = GetPropertyInfo(metadata);
            return prop.GetCustomAttribute<T>();
        }

        public HtmlTag FormGroupFor<TProp>(Expression<Func<TModel, TProp>> property)
        {
            var metadata = ModelMetadata.FromLambdaExpression(property, new ViewDataDictionary<TModel>());

            var name = ExpressionHelper.GetExpressionText(property);

            var expression = ExpressionForInternal(property);

            //Creates <div class="form-group has-feedback"
            //				form-group-validation="Name">
            var formGroup = new HtmlTag("div")
                .AddClasses("form-group")
                .Attr("form-group-validation", name);

            //20191001, jeni, begin
            //var labelText = metadata.DisplayName ?? name.Humanize(LetterCasing.Title);
            var labelText = metadata.DisplayName ?? name;
            //20191001, jeni, end

            //Creates <label class="control-label" for="Name">Name</label>
            var label = new HtmlTag("label")
                .AddClass("control-label")
                .AddClass(HtmlTagHelpers.LABEL_SIZE)
                .Attr("for", name)
                .Text(labelText);

            DivInputTag = new HtmlTag("div")
               .AddClass(HtmlTagHelpers.INPUT_SIZE);

            HtmlTag inputTag = null;
            //check if html
            if (metadata.DataTypeName == "Html")
            {
                inputTag = new HtmlTag("my-texthtml");
            }
            //check if textarea
            if (metadata.DataTypeName == "MultilineText")
            {
                inputTag = new HtmlTag("textarea")
                    .AddClass("form-control");
            }
            //check if enum type
            else if (metadata.ModelType.BaseType == typeof(Enum))
            {
                inputTag = new HtmlTag("select")
                .AddClass("form-control");
            }
            //check if bool type
            else if (metadata.ModelType == typeof(bool) || metadata.ModelType == typeof(Boolean))
            {
                // <div class="col-sm-offset-2 col-sm-10">
                // <div class="checkbox">
                //   <label><input type="checkbox"> Remember me</label>
                // </div>
                //</div>  
                inputTag = new HtmlTag("input")
                .Attr("type", "checkbox");
            }
            else if (metadata.DataTypeName == "Time")
            {
                inputTag = new HtmlTag("timepicker-pop")
                    .AddClass("input-group")
                    .Attr("input-time", expression)
                    .Attr("show-meridian", "true");
            }
            else if (metadata.ModelType == typeof(DateTime?) || metadata.ModelType == typeof(DateTime)) //check if datetime
            {
                //inputTag = new HtmlTag("ng-bs3-datepicker")
                //    .Attr("date-format", "DD-MM-YYYY");                              
                inputTag = new HtmlTag("my-datepicker");
            }
            else if (metadata.ModelType == typeof(string))
            {
                inputTag = new HtmlTag("input")
                .Attr("type", "text")
                .AddClass("form-control");
            }
            //https://github.com/cohenadair/angular.number-input http://cohenadair.github.io/angular.number-input/
            else if (metadata.ModelType == typeof(decimal) || metadata.ModelType == typeof(decimal?))
            {
                inputTag = new HtmlTag("number-input")
                    .Attr("decimal-places", "2");
                //.Attr("disable-decimal", "false")
                //.Attr("step", "0.01");

                //inputTag = new HtmlTag("my-number")
                //   .Attr("max-decimals", "2");
            }
            //https://github.com/cohenadair/angular.number-input http://cohenadair.github.io/angular.number-input/
            else if (metadata.ModelType == typeof(int) || metadata.ModelType == typeof(int?))
            {
                inputTag = new HtmlTag("number-input");
            }

            var placeholder = metadata.Watermark ??
                              (labelText + "...");
            //Creates <input ng-model="expression"
            //		   class="form-control" name="Name" type="text" >
            inputTag.Attr("ng-model", expression)
                .Attr("name", name) //name="Name"
                .Attr("placeholder", placeholder);

            //check if enum type
            if (metadata.ModelType.BaseType == typeof(Enum))
            {
                foreach (var item in Enum.GetValues(metadata.ModelType))
                {
                    inputTag.Append(new HtmlTag("option")
                    .Attr("value", item.ToString()).AppendHtml(item.ToString()));
                }
            }

            //check if bool type
            //if (metadata.ModelType == typeof(bool) || metadata.ModelType == typeof(Boolean))
            //{               
            //    var divCheckbox = new HtmlTag("div")
            //    .AddClass("checkbox");    

            //    inputTag = divCheckbox.Append(inputTag);
            //}

            //https://scotch.io/tutorials/angularjs-form-validation-with-ngmessages
            //http://www.sitepoint.com/easy-form-validation-angularjs-ngmessages/
            //Creates <div> for error messages
            //var errorMessages = new HtmlTag("div")
            //    .AddClass("help-block")
            //    .Attr("ng-messages", "vm.form." + name + ".$error")
            //    .Attr("ng-if", "vm.form." + name + ".$touched");

            var errorMessages = new HtmlTag("div")
                .AddClass("help-block");

            DivInputTag.Append(inputTag);

            ApplyValidationToInput(inputTag, errorMessages, metadata);

            DivInputTag.Append(errorMessages);

            return formGroup
                .Append(label)
                .Append(DivInputTag);
        }
        public HtmlTag FormGroupFor(string Property, string Label = null, TypeForm DataType = TypeForm.text, string PlaceHolder = "...", bool IsRequired = false, PropLenght PropLenght = null, PropRange PropRange = null)
        {
            //var metadata = ModelMetadata.FromLambdaExpression(property, new ViewDataDictionary<TModel>());

            var name = Property.Substring(Property.LastIndexOf('.') + 1);
            if (Label != null)
            {
                name = Label;
            }

            var expression = Property;

            //Creates <div class="form-group has-feedback"
            //				form-group-validation="Name">
            var formGroup = new HtmlTag("div")
                .AddClasses("form-group")
                .Attr("form-group-validation", name);

            //20191001, jeni, begin
            //var labelText = name.Humanize(LetterCasing.Title);
            var labelText = name;
            //20191001, jeni, end

            //Creates <label class="control-label" for="Name">Name</label>
            var label = new HtmlTag("label")
                .AddClass("control-label")
                .AddClass(HtmlTagHelpers.LABEL_SIZE)
                .Attr("for", name)
                .Text(labelText);

            DivInputTag = new HtmlTag("div")
               .AddClass(HtmlTagHelpers.INPUT_SIZE);

            HtmlTag inputTag = null;
            //check if html
            switch (DataType)
            {
                case TypeForm.Html:
                    inputTag = new HtmlTag("my-texthtml");
                    break;
                case TypeForm.MultilineText:
                    inputTag = new HtmlTag("textarea")
                    .AddClass("form-control")
                    .Attr("style", "height: 100px;");
                    break;
                case TypeForm.boolean:
                    inputTag = new HtmlTag("input")
                    .Attr("type", "checkbox");
                    break;
                case TypeForm.Time:
                    inputTag = new HtmlTag("timepicker-pop")
                    .AddClass("input-group")
                    .Attr("input-time", expression)
                    .Attr("show-meridian", "true");
                    break;
                case TypeForm.datetime:
                    inputTag = new HtmlTag("my-datepicker");
                    break;
                case TypeForm.text:
                    inputTag = new HtmlTag("input")
                    .Attr("type", "text")
                    .AddClass("form-control");
                    break;
                case TypeForm.dacimal:
                    inputTag = new HtmlTag("number-input")
                        .Attr("decimal-places", "2");
                    break;
                case TypeForm.integer:
                    inputTag = new HtmlTag("number-input");
                    break;
                case TypeForm.Phone:
                    inputTag = new HtmlTag("input")
                    .Attr("type", "text")
                    .AddClass("form-control");
                    break;
                case TypeForm.Email:
                    inputTag = new HtmlTag("input")
                    .Attr("type", "email")
                    .AddClass("form-control");
                    break;
                default:
                    inputTag = new HtmlTag("input")
                    .Attr("type", "text")
                    .AddClass("form-control");
                    break;
            }


            var placeholder = PlaceHolder;
            inputTag.Attr("ng-model", expression)
                .Attr("name", name)
                .Attr("placeholder", placeholder);


            var errorMessages = new HtmlTag("div")
                .AddClass("help-block");

            DivInputTag.Append(inputTag);
            
            ApplyValidationToInput(inputTag, errorMessages, DataType, IsRequired, PropLenght, PropRange);

            DivInputTag.Append(errorMessages);

            return formGroup
                .Append(label)
                .Append(DivInputTag);
        }

        private HtmlTag GetNgMessageTag(string validation, string errorMessage = "")
        {
            return new HtmlTag("p").Attr("ng-show", validation).AppendHtml(errorMessage);
        }

        private HtmlTag GetSpanMessageTag(string validation, string errorMessage = "")
        {
            return new HtmlTag("p").Attr("ng-show", validation).AppendHtml(errorMessage);
        }

        private void AddHintTag(string hintMessage = "")
        {
            DivInputTag.Append(new HtmlTag("small").AddClass("number-input-hint").AppendHtml(hintMessage));
        }

        private void ApplyValidationToInput(HtmlTag input, HtmlTag errorMessages, TypeForm typeForm, bool isRequired, PropLenght PropLenght = null, PropRange PropRange = null)
        {
            if (isRequired)
            {
                input.Attr("required", "");
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$error.required && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", "Harus Diisi!"));
            }

            if (typeForm == TypeForm.Email)
            {
                input.Attr("type", "email");
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$invalid && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", "Email tidak valid!"));
            }

            if (typeForm == TypeForm.Phone)
            {
                input.Attr("pattern", @"[\ 0-9()-]+");
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$invalid && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", "Phone tidak valid!"));
            }
            if (PropLenght != null)
            {
                input.Attr("ng-minlength", PropLenght.Min);
                input.Attr("ng-maxlength", PropLenght.Max);
                input.Attr("maxlength", PropLenght.Max);
                AddHintTag("Max Character : " + PropLenght.Max);
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$error.minlength && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", PropLenght.ErrorMessage));
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$error.maxlength && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", PropLenght.ErrorMessage));
            }

            if (PropRange != null)
            {
                //input.Attr("min", "-1"); //min set -1 supaya tidak bisa input decimal jika int dan pengetikan angka decimal di belakang koma benar
                input.Attr("min", PropRange.Min);
                input.Attr("start", "0");
                input.Attr("max", PropRange.Max);
                input.Attr("hint", PropRange.Min + " to " + PropRange.Max);
            }

        }
        private void ApplyValidationToInput(HtmlTag input, HtmlTag errorMessages, ModelMetadata metadata)
        {
            //check if bool type dont apply validation
            if (metadata.ModelType == typeof(bool) || metadata.ModelType == typeof(Boolean))
            {
                return;
            }

            if (metadata.IsRequired)
            {
                input.Attr("required", "");
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$error.required && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", "Harus Diisi!"));
            }

            if (metadata.DataTypeName == "EmailAddress")
            {
                input.Attr("type", "email");
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$invalid && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", "Email tidak valid!"));
            }

            if (metadata.DataTypeName == "PhoneNumber")
            {
                input.Attr("pattern", @"[\ 0-9()-]+");
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$invalid && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", "Phone tidak valid!"));
            }

            StringLengthAttribute stringLengthAttribute = GetAttribute<StringLengthAttribute>(metadata);
            if (stringLengthAttribute != null)
            {
                input.Attr("ng-minlength", stringLengthAttribute.MinimumLength);
                input.Attr("ng-maxlength", stringLengthAttribute.MaximumLength);
                input.Attr("maxlength", stringLengthAttribute.MaximumLength);
                AddHintTag("Max Character : " + stringLengthAttribute.MaximumLength);
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$error.minlength && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", stringLengthAttribute.ErrorMessage));
                errorMessages.Append(GetSpanMessageTag("vm.form." + input.Attr("name") + ".$error.maxlength && (vm.form." + input.Attr("name") + ".$touched || vm.form.$submitted)", stringLengthAttribute.ErrorMessage));
            }

            RangeAttribute rangeAttribute = GetAttribute<RangeAttribute>(metadata);
            if (rangeAttribute != null)
            {
                //input.Attr("min", "-1"); //min set -1 supaya tidak bisa input decimal jika int dan pengetikan angka decimal di belakang koma benar
                input.Attr("min", rangeAttribute.Minimum);
                input.Attr("start", "0");
                input.Attr("max", rangeAttribute.Maximum);
                input.Attr("hint", rangeAttribute.Minimum + " to " + rangeAttribute.Maximum);
            }
        }

    }
}