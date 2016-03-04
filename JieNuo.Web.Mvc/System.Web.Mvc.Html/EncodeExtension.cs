using System;
namespace System.Web.Mvc.Html
{
	public static class EncodeExtension
	{
		public static string Encode(this HtmlHelper html, string format, params object[] args)
		{
			return html.Encode(string.Format(format, args));
		}
	}
}
