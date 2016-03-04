using JieNuo.Data;
using System;
using System.Linq.Expressions;
namespace System.Web.Mvc.Html
{
	public static class HtmlHelpers
	{
		public static MvcHtmlString PrintLink<T>(this HtmlHelper<T> htmlHelper) where T : IModelWithCode
		{
			T model = htmlHelper.ViewData.Model;
			MvcHtmlString result;
			if (model == null)
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				result = htmlHelper.AuthorizedActionLink("打印", "Print", new
				{
					code = model.Code
				}, new
				{
					@class = "button"
				});
			}
			return result;
		}
		public static MvcHtmlString EditLink<T>(this HtmlHelper<T> htmlHelper) where T : IModelWithCode
		{
			T model = htmlHelper.ViewData.Model;
			MvcHtmlString result;
			if (model == null)
			{
				result = MvcHtmlString.Empty;
			}
			else
			{
				result = htmlHelper.AuthorizedActionLink("编辑", "Edit", new
				{
					code = model.Code
				}, new
				{
					@class = "button"
				});
			}
			return result;
		}
		public static MvcHtmlString ReturnToListLink(this HtmlHelper htmlHelper)
		{
			return htmlHelper.AuthorizedReturnToListLink("返回列表", "Index", new
			{
				@class = "button"
			});
		}
		public static MvcHtmlString DetailsLink<T>(this HtmlHelper<PagedData<T>> htmlHelper, T model) where T : IModelWithCode
		{
			return htmlHelper.AuthorizedActionLink("查看", "Details", new
			{
				code = model.Code, 
				controller = typeof(T).Name.Replace("Controller", "")
			}, new
			{
				@class = "details"
			});
		}
		public static MvcHtmlString EditLink<T>(this HtmlHelper<PagedData<T>> htmlHelper, T model) where T : IModelWithCode
		{
			return htmlHelper.AuthorizedActionLink("编辑", "Edit", new
			{
				code = model.Code
			}, new
			{
				@class = "modify"
			});
		}
		public static MvcHtmlString DeleteLink<T>(this HtmlHelper<PagedData<T>> htmlHelper, T model) where T : IModelWithCode
		{
			return htmlHelper.AuthorizedActionLink("删除", "Delete", new
			{
				code = model.Code
			}, new
			{
				@class = "delete", 
				title = model.ToString()
			});
		}
		public static MvcHtmlString SaveLink(this HtmlHelper htmlHelper, string formToSubmit)
		{
			return htmlHelper.ActionLink("保存", "Save", null, new
			{
				@class = "button", 
				onclick = "submit(\"{0}\"); return false;".FormatWith(new object[]
				{
					formToSubmit
				})
			});
		}
		public static MvcHtmlString SaveCreateLink(this HtmlHelper htmlHelper, string formToSubmit)
		{
			return htmlHelper.ActionLink("保存新增", "Save", new
			{
				redirectToCreate = true
			}, new
			{
				@class = "button", 
				onclick = "submit('{0}'); return false;".FormatWith(new object[]
				{
					formToSubmit
				})
			});
		}
		public static MvcHtmlString CancelLink<T>(this HtmlHelper<T> htmlHelper) where T : IModelWithCode
		{
			string arg_34_1 = "取消";
			string arg_34_2 = "Cancel";
			T model = htmlHelper.ViewData.Model;
			return htmlHelper.AuthorizedActionLink(arg_34_1, arg_34_2, new
			{
				code = model.Code
			}, new
			{
				@class = "button"
			});
		}
		public static MvcHtmlString PropertyNameFor<T, TProperty>(this HtmlHelper<PagedData<T>> htmlHelper, System.Linq.Expressions.Expression<System.Func<T, TProperty>> expression)
		{
			string name = ExpressionHelper.GetExpressionText(expression);
			return MvcHtmlString.Create(name);
		}
		public static MvcHtmlString PropertyNameFor<T, TProperty>(this HtmlHelper<T> htmlHelper, System.Linq.Expressions.Expression<System.Func<T, TProperty>> expression)
		{
			string name = ExpressionHelper.GetExpressionText(expression);
			return MvcHtmlString.Create(name);
		}
	}
}
