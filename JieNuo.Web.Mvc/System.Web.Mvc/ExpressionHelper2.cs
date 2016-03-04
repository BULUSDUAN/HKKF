using System;
using System.Linq;
using System.Linq.Expressions;
namespace System.Web.Mvc
{
	public static class ExpressionHelper2
	{
		public static string GetExpressionText(System.Linq.Expressions.LambdaExpression expression)
		{
			if (expression.Body != null && expression.Body.NodeType == System.Linq.Expressions.ExpressionType.Convert)
			{
				System.Linq.Expressions.UnaryExpression exp = expression.Body as System.Linq.Expressions.UnaryExpression;
				expression = System.Linq.Expressions.Expression.Lambda(exp.Operand, expression.Parameters.ToArray<System.Linq.Expressions.ParameterExpression>());
			}
			return ExpressionHelper.GetExpressionText(expression);
		}
	}
}
