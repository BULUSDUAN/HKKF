using JieNuo.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class AuthorizedActionLinkOrPlainTextExtension
	{
		public static MvcHtmlString AuthorizedActionLinkOrPlainText(this HtmlHelper helper, string linkText, string action, string controller = null, System.Web.Routing.RouteValueDictionary routeValue = null, System.Collections.Generic.IDictionary<string, object> htmlAttributes = null)
		{
			MvcHtmlString s = helper.AuthorizedActionLink(linkText, action, controller, routeValue, htmlAttributes);
			MvcHtmlString result;
			if (MvcHtmlString.IsNullOrEmpty(s))
			{
				result = MvcHtmlString.Create(helper.Encode(linkText));
			}
			else
			{
				result = s;
			}
			return result;
		}
		public static MvcHtmlString AuthorizedActionLinkOrPlainText(this HtmlHelper helper, string linkText, string action, string controller = null, object routeValue = null, object htmlAttributes = null)
		{
			return helper.AuthorizedActionLinkOrPlainText(linkText, action, controller, (routeValue == null) ? null : new System.Web.Routing.RouteValueDictionary(routeValue), (htmlAttributes == null) ? null : new System.Web.Routing.RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString AuthorizedActionLinkOrPlainText<TModel, TMember>(this HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expresion, string action, string controller, object routeValue, object htmlAttributes)
		{
			MvcHtmlString text = htmlHelper.DisplayValueFor(htmlHelper.ViewData.Model, expresion);
			MvcHtmlString result;
			if (MvcHtmlString.IsNullOrEmpty(text))
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				result = htmlHelper.AuthorizedActionLinkOrPlainText(text.ToHtmlString(), action, controller, routeValue, htmlAttributes);
			}
			return result;
		}
		public static MvcHtmlString AuthorizedActionLinkOrPlainText<TModel, TMember>(this HtmlHelper htmlHelper, TModel model, System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expresion, string action, string controller = null, object routeValue = null, object htmlAttributes = null)
		{
			MvcHtmlString text = htmlHelper.DisplayValueFor(model, expresion);
			MvcHtmlString result;
			if (MvcHtmlString.IsNullOrEmpty(text))
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				result = htmlHelper.AuthorizedActionLinkOrPlainText(text.ToHtmlString(), action, controller, routeValue, htmlAttributes);
			}
			return result;
		}
		public static MvcHtmlString AuthorizedActionLinkOrPlainText<TModel, TMember>(this HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expresion, object htmlAttributes = null)
		{
			return htmlHelper.AuthorizedActionLinkOrPlainText(htmlHelper.ViewData.Model, expresion, htmlAttributes);
		}
		public static MvcHtmlString AuthorizedActionLinkOrPlainText<TModel, TMember>(this HtmlHelper htmlHelper, TModel model, System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expresion, object htmlAttributes = null)
		{
			MvcHtmlString result;
			if (model == null || expresion == null)
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				TMember member = expresion.Compile()(model);
				if (member == null)
				{
					result = MvcHtmlString.Empty;
				}
				else
				{
					string linkText = HttpUtility.HtmlDecode(htmlHelper.DisplayValueFor(model, expresion).ToHtmlString());
					if (member is IModelWithCode)
					{
						string controller = typeof(TMember).Name;
						string code = (member as IModelWithCode).Code;
						result = htmlHelper.AuthorizedActionLinkOrPlainText(linkText, "Details", controller, new
						{
							area = "", 
							code = code
						}, htmlAttributes);
					}
					else
					{
                        //if (member is IModelWithID)
                        //{
                        //    string controller = typeof(TMember).Name;
                        //    int id = (member as IModelWithID).get_ID();
                        //    result = htmlHelper.AuthorizedActionLinkOrPlainText(linkText, "Details", controller, new
                        //    {
                        //        area = "", 
                        //        id = id
                        //    }, htmlAttributes);
                        //}
                        //else
                        //{
							if (model is IModelWithCode)
							{
								string controller = typeof(TModel).Name;
								string code = (model as IModelWithCode).Code;
								result = htmlHelper.AuthorizedActionLinkOrPlainText(linkText, "Details", controller, new
								{
									area = "", 
									code = code
								}, htmlAttributes);
							}
							else
							{
                                //if (model is IModelWithID)
                                //{
                                //    string controller = typeof(TModel).Name;
                                //    int id = (model as IModelWithID).get_ID();
                                //    result = htmlHelper.AuthorizedActionLinkOrPlainText(linkText, "Details", controller, new
                                //    {
                                //        area = "", 
                                //        id = id
                                //    }, htmlAttributes);
                                //}
                                //else
                                //{
									result = htmlHelper.DisplayValueFor(model, expresion);
                                //}
							}
                        //}
					}
				}
			}
			return result;
		}
	}
}
