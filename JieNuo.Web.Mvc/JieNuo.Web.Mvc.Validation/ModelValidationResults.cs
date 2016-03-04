using System;
using System.Linq.Expressions;
using System.Web.Mvc;
namespace JieNuo.Web.Mvc.Validation
{
	public class ModelValidationResults<TModel> : ModelValidationResults
	{
		public void Add<TMember>(System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expression, string message)
		{
			base.Add(ExpressionHelper2.GetExpressionText(expression), message);
		}
		public void Add<TMember>(System.Linq.Expressions.Expression<System.Func<TModel, TMember>> expression, string messageFormat, params object[] args)
		{
			base.Add(ExpressionHelper2.GetExpressionText(expression), string.Format(messageFormat, args));
		}
	}
}
