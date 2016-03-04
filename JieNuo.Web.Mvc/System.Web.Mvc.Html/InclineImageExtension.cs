using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
	public static class InclineImageExtension
	{
		private static readonly string srcFormat = "data:image/{0};base64,{1}";
		private static readonly string hrefFormat = "data:text/html;base64,{0}";
		public static MvcHtmlString InclineImage(this HtmlHelper helper, Image image)
		{
			return helper.InclineImage(image, null);
		}
		public static MvcHtmlString InclineImage(this HtmlHelper helper, Image image, object htmlAttribute)
		{
			return helper.InclineImage(image, ImageFormat.Jpeg, htmlAttribute);
		}
		public static MvcHtmlString InclineImage(this HtmlHelper helper, Image image, ImageFormat format, object htmlAttributes)
		{
			byte[] data;
			using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
			{
				image.Save(stream, format);
				data = stream.ToArray();
			}
			string src = string.Format(InclineImageExtension.srcFormat, format, System.Convert.ToBase64String(data));
			TagBuilder builder = new TagBuilder("img");
			builder.MergeAttribute("src", src);
			if (htmlAttributes != null)
			{
				System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(htmlAttributes);
				builder.MergeAttributes<string, object>(dict, false);
			}
			return MvcHtmlString.Create(builder.ToString());
		}
		public static MvcHtmlString InclineLink(this HtmlHelper htmlHelper, string linkText, MvcHtmlString s, object htmlAttributes)
		{
			TagBuilder builder = new TagBuilder("a");
			byte[] data = System.Text.Encoding.UTF8.GetBytes(s.ToHtmlString());
			string href = string.Format(InclineImageExtension.hrefFormat, System.Convert.ToBase64String(data));
			builder.MergeAttribute("href", href);
			builder.InnerHtml = htmlHelper.Encode(linkText);
			if (htmlAttributes != null)
			{
				System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary(htmlAttributes);
				builder.MergeAttributes<string, object>(dict, false);
			}
			return MvcHtmlString.Create(builder.ToString());
		}
	}
}
