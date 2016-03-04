using System;
namespace System.Web.Mvc
{
	public class HtmlPager
	{
		public static readonly int[] PageSizes = new int[]
		{
			10, 
			20, 
			30, 
			50,
            100,
            500,
            1000
		};
		public static readonly string PageStr = "QueryInfo.Page";
		public static readonly string PageSizeStr = "QueryInfo.PageSize";
		public static readonly string SepCssClass = "sep";
	}
}
