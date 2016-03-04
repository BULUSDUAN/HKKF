using System;
namespace System.Web.Mvc.Html
{
	public static class EmptyXXXXExtension
	{
		public static MvcHtmlString EmptyTrIf(this HtmlHelper htmlHelper, int colspan, bool b)
		{
			MvcHtmlString result;
			if (!b)
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				string html = "<tr>\r\n    <td colspan=\"{0}\" class=\"empty\">无</td>\r\n</tr>";
				result = MvcHtmlString.Create(string.Format(html, colspan));
			}
			return result;
		}
		public static MvcHtmlString EmptyTFootIf(this HtmlHelper htmlHelper, int colspan, bool b)
		{
			MvcHtmlString result;
			if (!b)
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				string html = "<tfoot>\r\n    <tr>\r\n        <td colspan=\"{0}\" class=\"empty\">无</td>\r\n    </tr>\r\n</tfoot>";
				result = MvcHtmlString.Create(string.Format(html, colspan));
			}
			return result;
		}
	}
}
