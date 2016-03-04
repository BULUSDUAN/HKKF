using JieNuo.Data;
using System;
using System.Linq;
using System.Text;
namespace System.Web.Mvc.Html
{
	public static class HtmlPagerHelper
	{
		public static MvcHtmlString Pager(this HtmlHelper htmlHelper, Pager pager)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			builder.AppendLine("<div class=\"pager\">");
			builder.AppendLine("    <div class=\"sizeInfo\">每页数据</div>");
			builder.AppendLine("    <div class=\"size\">");
			MvcHtmlString dropDown = htmlHelper.DropDownList(HtmlPager.PageSizeStr, 
				from size in HtmlPager.PageSizes
				select new SelectListItem
				{
					Text = size.ToString(), 
					Value = size.ToString(), 
					Selected = pager.PageSize == size
				}, new
			{
				onchange = "$(this).parents('form')[0].extraData = 'QueryInfo.Page=1&';submitParentForm(this)"
			});
			builder.Append("        ");
			builder.AppendLine(dropDown.ToHtmlString());
			builder.AppendLine("    </div>");
			builder.AppendFormat("    <div class=\"first{0}\">{1}</div>\r\n", pager.CanMoveToFirstPage ? "" : "_disabled", pager.FristPage);
			builder.AppendFormat("    <div class=\"{0}\"></div>\r\n", HtmlPager.SepCssClass);
			builder.AppendFormat("    <div class=\"prev{0}\">{1}</div>\r\n", pager.CanMovePrev ? "" : "_disabled", pager.PreviousPage);
			builder.AppendFormat("    <div class=\"{0}\"></div>\r\n", HtmlPager.SepCssClass);
			builder.AppendLine("    <div class=\"current\">");
			builder.AppendFormat("        <input class=\"page\" id=\"QueryInfo_Page\" name=\"QueryInfo.Page\" type=\"text\" value=\"{0}\" />", pager.CurrentPage);
			builder.AppendFormat("&nbsp;/&nbsp; {0}\r\n", pager.PageCount);
			builder.AppendLine("    </div>");
			builder.AppendFormat("    <div class=\"{0}\"></div>\r\n", HtmlPager.SepCssClass);
			builder.AppendFormat("    <div class=\"next{0}\">{1}</div>\r\n", pager.CanMoveNext ? "" : "_disabled", pager.NextPage);
			builder.AppendFormat("    <div class=\"{0}\"></div>\r\n", HtmlPager.SepCssClass);
			builder.AppendFormat("    <div class=\"last{0}\">{1}</div>\r\n", pager.CanMoveToLastPage ? "" : "_disabled", pager.LastPage);
			builder.AppendFormat("    <div class=\"{0}\"></div>\r\n", HtmlPager.SepCssClass);
			builder.AppendFormat("    <div class=\"refresh\">刷新</div>\r\n", new object[0]);
			builder.AppendFormat("    <div class=\"{0}\"></div>\r\n", HtmlPager.SepCssClass);
			builder.AppendFormat("    <div class=\"loading\">正在加载数据，请稍候...</div>\r\n", new object[0]);
			builder.AppendFormat("    <div class=\"info\">共 {0} 条记录</div>\r\n", pager.TotalCount);
			builder.AppendFormat("    <div class=\"clear\"></div>\r\n", new object[0]);
			builder.AppendFormat("    <input type=\"hidden\" id=\"QueryInfo_OrderBy\" name=\"QueryInfo.OrderBy\" value=\"{0}\" />\r\n", pager.OrderBy);
			builder.AppendFormat("    <input type=\"hidden\" id=\"QueryInfo_IsDesc\" name=\"QueryInfo.IsDesc\" value=\"{0}\" />\r\n", pager.IsDesc);
			builder.AppendLine("</div>");
			return MvcHtmlString.Create(builder.ToString());
		}
	}
}
