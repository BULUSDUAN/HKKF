using System;
using System.Text.RegularExpressions;
namespace System.Web.Mvc.Html
{
	public static class HtmlHighlighter
	{
		//private static readonly string STR_Highlight = "<span class={1}>{0}</span>";
        private static readonly string STR_Highlight = "{0}";
		public static string Highlight(this string s, string key)
		{
			return s.Highlight(key, "highlight");
		}
		public static string Highlight(this string s, string key, string cssClass)
		{
			string result;
			if (string.IsNullOrEmpty(key))
			{
				result = s;
			}
			else
			{
				result = Regex.Replace(s, HtmlHighlighter.FormatKey(key), (Match match) => string.Format(HtmlHighlighter.STR_Highlight, match.Value, cssClass), RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
			return result;
		}
		public static string HighlightIf(this string s, bool condition, string key, string cssClass)
		{
			string result;
			if (condition)
			{
				result = s.Highlight(key, cssClass);
			}
			else
			{
				result = s;
			}
			return result;
		}
		public static string HighlightIf(this string s, bool condition, string key)
		{
			string result;
			if (condition)
			{
				result = s.Highlight(key);
			}
			else
			{
				result = s;
			}
			return result;
		}
		private static string FormatKey(string key)
		{
			return Regex.Escape(key).Replace("_", ".").Replace("%", ".*");
		}
	}
}
