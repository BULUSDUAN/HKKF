using System;
using System.Collections.Generic;
namespace JieNuo.Web.Mvc.Html
{
	public class ValueBox
	{
		public string Name
		{
			get;
			set;
		}
		public string HiddenInputName
		{
			get;
			set;
		}
		public string BusinessID
		{
			get;
			set;
		}
		public string Text
		{
			get;
			set;
		}
		public string ImageCssClass
		{
			get;
			set;
		}
		public bool IsSimple
		{
			get;
			set;
		}
		public string DataType
		{
			get;
			set;
		}
		public string Type
		{
			get;
			set;
		}
		public System.Collections.Generic.IDictionary<string, object> HtmlAttributes
		{
			get;
			set;
		}
		public string Icon
		{
			get;
			set;
		}
		public bool IsComplex
		{
			get
			{
				return !this.IsSimple;
			}
			set
			{
				this.IsSimple = !value;
			}
		}
	}
}
