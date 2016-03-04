using JieNuo.ComponentModel;
using JieNuo.Data;
using JieNuo.Web.Mvc.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class ValueBoxHelper
	{
		public static readonly string BusinessIDSuffix = "_bid";
		public static readonly string DatabseIDSuffix = "_id";
		private static readonly string ValueBox_Image_Icon = "ValueBox_Image_Icon";
		private static System.Web.Routing.RouteValueDictionary ValueBoxHtmlAttributes(this System.Type type)
		{
			System.Type _type = type.IsNullableType() ? type.GetNonNullableType() : type;
			return new System.Web.Routing.RouteValueDictionary
			{

				{
					"datatype", 
					_type.FullName.Replace('.', '_')
				}
			};
		}
		public static MvcHtmlString ValueBox(this HtmlHelper htmlHelper, string name, object model, System.Collections.Generic.IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null)
			{
				htmlAttributes = new System.Web.Routing.RouteValueDictionary();
			}
			if (htmlAttributes.ContainsKey("class"))
			{
				string @class = htmlAttributes["class"] as string;
				if (!Regex.Match(@class, "(^| )valuebox( |$)", RegexOptions.Compiled).Success)
				{
					htmlAttributes["class"] = @class + " valuebox";
				}
			}
			else
			{
				htmlAttributes.Add("class", "valuebox");
			}
			if (!htmlAttributes.ContainsKey("dataType"))
			{
				htmlAttributes.Add("dataType", "System_String");
			}
			string dataType = htmlAttributes["dataType"] as string;
			ValueBox valueBox = new ValueBox
			{
				Name = name, 
				DataType = dataType, 
				Type = (htmlAttributes.TryGetValue("type") as string) ?? "text", 
				HiddenInputName = name + ValueBoxHelper.BusinessIDSuffix, 
				ImageCssClass = "valuebox_img " + dataType, 
				HtmlAttributes = htmlAttributes, 
				Icon = htmlHelper.ViewData[ValueBoxHelper.ValueBox_Image_Icon] as string
			};
			if (Regex.Match(dataType, "^(System_|_)", RegexOptions.Compiled).Success)
			{
				valueBox.IsSimple = true;
				valueBox.Name = name;
				valueBox.Text = System.Convert.ToString(model);
			}
			else
			{
				object textPropertyValue = null;
				object businessIdValue = null;
				if (model is string)
				{
					businessIdValue = (textPropertyValue = (model as string));
				}
				else
				{
					if (model != null)
					{
						textPropertyValue = model.GetTextPropertyValue();
						businessIdValue = model.GetBusinessIdValue();
					}
				}
				valueBox.IsSimple = false;
				valueBox.BusinessID = System.Convert.ToString(businessIdValue);
				valueBox.Text = System.Convert.ToString(textPropertyValue);
			}
			return ValueBoxHelper.GenerateValueBox(valueBox);
		}
		public static MvcHtmlString ValueBox(this HtmlHelper htmlHelper, string name, object model, object htmlAttributes = null)
		{
			return htmlHelper.ValueBox(name, model, new System.Web.Routing.RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString ValueBox(this HtmlHelper htmlHelper, string name, object htmlAttributes = null)
		{
			string value = htmlHelper.ViewContext.RequestContext.HttpContext.Request[name];
			return htmlHelper.ValueBox(name, value, htmlAttributes);
		}
		public static MvcHtmlString ValueBox(this HtmlHelper htmlHelper, System.Type type, string name, object model, System.Collections.Generic.IDictionary<string, object> htmlAttributes = null)
		{
			if (htmlAttributes == null)
			{
				htmlAttributes = new System.Web.Routing.RouteValueDictionary();
			}
			htmlAttributes.AddRange(type.ValueBoxHtmlAttributes(), false);
			return htmlHelper.ValueBox(name, model, htmlAttributes);
		}
		public static MvcHtmlString ValueBox(this HtmlHelper htmlHelper, System.Type type, string name, object model, object htmlAttributes)
		{
			return htmlHelper.ValueBox(type, name, model, new System.Web.Routing.RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString ValueBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			System.Web.Routing.RouteValueDictionary htmlAttributesDict = new System.Web.Routing.RouteValueDictionary(htmlAttributes ?? new
			{

			});
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
			string propertyName = ExpressionHelper.GetExpressionText(expression);
			string icon = metadata.Icon();
			if (icon.IsNotNullAndEmpty())
			{
				htmlHelper.ViewData[ValueBoxHelper.ValueBox_Image_Icon] = icon;
			}
			if (metadata.DataTypeName == DataType.Password.ToString())
			{
				if (htmlAttributesDict == null)
				{
					htmlAttributesDict = new System.Web.Routing.RouteValueDictionary();
				}
				if (htmlAttributesDict.ContainsKey("type"))
				{
					htmlAttributesDict["type"] = "password";
				}
				else
				{
					htmlAttributesDict.Add("type", "password");
				}
			}
			TModel model = htmlHelper.ViewData.Model;
			object propertyValue = metadata.Model;
			if (string.IsNullOrEmpty(name))
			{
				name = propertyName;
			}
			MvcHtmlString result;
			if ((htmlHelper.ViewMode() == ViewMode.Create && !metadata.EditableWhenCreate()) || (htmlHelper.ViewMode() == ViewMode.Edit && !metadata.EditableWhenEdit()))
			{
				string text = string.Format(metadata.EditFormatString ?? "{0}", propertyValue);
				result = MvcHtmlString.Create(htmlHelper.Encode(text));
			}
			else
			{
				if (typeof(TProperty).FullName.StartsWith("System"))
				{
					string text = string.Format(metadata.EditFormatString ?? "{0}", propertyValue);
					result = htmlHelper.ValueBox(typeof(TProperty), name, text, htmlAttributesDict);
				}
				else
				{
					result = htmlHelper.ValueBox(typeof(TProperty), name, propertyValue, htmlAttributesDict);
				}
			}
			return result;
		}
		public static MvcHtmlString ValueBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			return htmlHelper.ValueBoxFor("", expression, htmlAttributes);
		}
		public static MvcHtmlString ValueBoxFor<TModel>(this HtmlHelper htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, object>> expression, object value, object htmlAttributes = null)
		{
			string name = ExpressionHelper2.GetExpressionText(expression);
			System.Type propertyType = expression.Body.Type;
			return htmlHelper.ValueBox(propertyType, name, value, htmlAttributes);
		}
		public static MvcHtmlString ValueBoxFor<TModel>(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes = null)
		{
			return htmlHelper.ValueBox(typeof(TModel), name, value, htmlAttributes);
		}
		[System.Obsolete]
		public static MvcHtmlString ValueBoxForItem<TModel, TProperty>(this HtmlHelper<PagedData<TModel>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression, object value, string name = "", object htmlAttributes = null)
		{
			string propertyName = ExpressionHelper.GetExpressionText(expression);
			MvcHtmlString result;
			if (value is TProperty)
			{
				ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(TModel), propertyName);
				if (name.IsNullOrEmpty())
				{
					name = propertyName;
				}
				string text = string.Format(metadata.EditFormatString ?? "{0}", value);
				result = htmlHelper.ValueBoxFor<TProperty>(name, text, htmlAttributes);
			}
			else
			{
				result = htmlHelper.ValueBoxFor<TProperty>(name, value, htmlAttributes);
			}
			return result;
		}
		[System.Obsolete]
		public static MvcHtmlString ValueBoxForItem<TModel, TProperty>(this HtmlHelper<PagedData<TModel>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			string name = ExpressionHelper.GetExpressionText(expression);
			return htmlHelper.ValueBox(typeof(TProperty), name, null, htmlAttributes);
		}
		[System.Obsolete]
		public static MvcHtmlString ValueBoxForItem<TModel, TProperty>(this HtmlHelper<PagedData<TModel>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression)
		{
			string name = ExpressionHelper.GetExpressionText(expression);
			return htmlHelper.ValueBox(typeof(TProperty), name, null, null);
		}
		[System.Obsolete]
		public static MvcHtmlString ValueBoxForItem<TModel, TProperty>(this HtmlHelper<System.Collections.Generic.IEnumerable<TModel>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression, object value, string name = "", object htmlAttributes = null)
		{
			string propertyName = ExpressionHelper.GetExpressionText(expression);
			MvcHtmlString result;
			if (value is TProperty)
			{
				ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(TModel), propertyName);
				if (name.IsNullOrEmpty())
				{
					name = propertyName;
				}
				string text = string.Format(metadata.EditFormatString ?? "{0}", value);
				result = htmlHelper.ValueBoxFor<TProperty>(name, text, htmlAttributes);
			}
			else
			{
				result = htmlHelper.ValueBoxFor<TProperty>(name, value, htmlAttributes);
			}
			return result;
		}
		[System.Obsolete]
		public static MvcHtmlString ValueBoxForItem<TModel, TProperty>(this HtmlHelper<System.Collections.Generic.IEnumerable<TModel>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			string name = ExpressionHelper.GetExpressionText(expression);
			return htmlHelper.ValueBox(typeof(TProperty), name, null, htmlAttributes);
		}
		[System.Obsolete]
		public static MvcHtmlString ValueBoxForItem<TModel, TProperty>(this HtmlHelper<System.Collections.Generic.IEnumerable<TModel>> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TProperty>> expression)
		{
			string name = ExpressionHelper.GetExpressionText(expression);
			return htmlHelper.ValueBox(typeof(TProperty), name, null, null);
		}
		private static MvcHtmlString GenerateValueBox(ValueBox valueBox)
		{
			TagBuilder div = new TagBuilder("div");
			div.AddCssClass("valuebox_div");
			System.Text.StringBuilder innerHtml = new System.Text.StringBuilder();
			TagBuilder iconLink = new TagBuilder("a");
            iconLink.AddCssClass(valueBox.ImageCssClass);
            iconLink.MergeAttribute("href", "#");
            iconLink.MergeAttribute("tabIndex", "-1");
			iconLink.MergeAttribute("target", "_blank");
			if (valueBox.Icon.IsNotNullAndEmpty())
			{
				iconLink.MergeAttribute("style", "background-image:url('../content/images/{0}')".FormatWith(new object[]
				{
					valueBox.Icon
				}));
			}
			innerHtml.AppendLine(iconLink.ToString());
			TagBuilder nameTextBox = new TagBuilder("input");
			nameTextBox.GenerateId(valueBox.Name);
			nameTextBox.MergeAttribute("name", valueBox.Name);
			nameTextBox.MergeAttribute("type", valueBox.Type);
			nameTextBox.MergeAttribute("value", valueBox.Text);
			nameTextBox.MergeAttributes<string, object>(valueBox.HtmlAttributes);
			innerHtml.AppendLine(nameTextBox.ToString(TagRenderMode.SelfClosing));
			if (valueBox.IsComplex)
			{
				TagBuilder buttonSpan = new TagBuilder("span");
				buttonSpan.AddCssClass("valuebox_button");
				innerHtml.AppendLine(buttonSpan.ToString());
				TagBuilder hiddenInput = new TagBuilder("input");
				hiddenInput.GenerateId(valueBox.HiddenInputName);
				hiddenInput.MergeAttribute("name", valueBox.HiddenInputName);
				hiddenInput.MergeAttribute("type", "hidden");
				hiddenInput.MergeAttribute("value", valueBox.BusinessID);
				innerHtml.AppendLine(hiddenInput.ToString(TagRenderMode.SelfClosing));
			}
			div.InnerHtml = innerHtml.ToString();
			return MvcHtmlString.Create(div.ToString());
		}
	}
}
