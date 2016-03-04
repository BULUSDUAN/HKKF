using System;
using System.Configuration;
namespace System.Web.Mvc.Html
{
	public static class AppSettingsIfExtension
	{
		public static MvcHtmlString AppSettingsIf(this HtmlHelper htmlHelper, string key, string value, System.Func<MvcHtmlString> mvcHtmlString)
		{
			MvcHtmlString result;
			if (ConfigurationManager.AppSettings[key] == value)
			{
				result = mvcHtmlString();
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static MvcHtmlString AppSettingsIf(this HtmlHelper htmlHelper, string key, string value, MvcHtmlString mvcHtmlString)
		{
			MvcHtmlString result;
			if (ConfigurationManager.AppSettings[key] == value)
			{
				result = mvcHtmlString;
			}
			else
			{
				result = MvcHtmlString.Empty;
			}
			return result;
		}
		public static string AppSettingsIf(this HtmlHelper htmlHelper, string key, string value, System.Func<string> str)
		{
			string result;
			if (ConfigurationManager.AppSettings[key] == value)
			{
				result = str();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public static string AppSettingsIf(this HtmlHelper htmlHelper, string key, string value, string str)
		{
			string result;
			if (ConfigurationManager.AppSettings[key] == value)
			{
				result = str;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public static string AppSettings(this HtmlHelper htmlHelper, string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
		public static T? AppSettings<T>(this HtmlHelper htmlHelper, string key) where T : struct
		{
			string value = ConfigurationManager.AppSettings[key];
			object obj = value.ConvertTo<T>();
			T? result;
			if (obj is T)
			{
				result = new T?((T)obj);
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
