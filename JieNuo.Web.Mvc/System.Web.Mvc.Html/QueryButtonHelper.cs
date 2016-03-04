using System;
namespace System.Web.Mvc.Html
{
	public static class QueryButtonHelper
	{
		public static string QueryButton(this HtmlHelper htmlHelper)
		{
			return htmlHelper.QueryButton(null);
		}
		public static string QueryButton(this HtmlHelper htmlHelper, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				text = "查询";
			}
			TagBuilder builder = new TagBuilder("a");
			builder.AddCssClass("queryButton button");
			builder.InnerHtml = text;
			return builder.ToString();
		}
	}
}
