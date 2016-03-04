using JieNuo.Data;
using System;
using System.Collections.Generic;
namespace System.Web.Mvc.Html
{
	public static class PageIconExtension
	{
		public static MvcHtmlString PageIcon<T>(this HtmlHelper<PagedData<T>> helper)
		{
			return helper.PageIcon<T>();
		}
		public static MvcHtmlString PageIcon<T>(this HtmlHelper<System.Collections.Generic.IEnumerable<T>> helper)
		{
			return helper.PageIcon<T>();
		}
		public static MvcHtmlString PageIcon<T>(this HtmlHelper<T> helper)
		{
			return helper.PageIcon<T>();
		}
		public static MvcHtmlString PageIcon<T>(this HtmlHelper helper)
		{
			string path = string.Format("/content/images/{0}.png", typeof(T).FullName.Replace('.', '_'));
			return helper.PageIcon(path);
		}
		public static MvcHtmlString PageIcon(this HtmlHelper helper, string path)
		{
			return MvcHtmlString.Create(new TagBuilder("link")
			{
				Attributes = 
				{

					{
						"rel", 
						"icon"
					}, 

					{
						"type", 
						"/content/images/" + path.Match("(?<=[.])[^.]+$")
					}, 

					{
						"href", 
						path
					}
				}
			}.ToString());
		}
	}
}
