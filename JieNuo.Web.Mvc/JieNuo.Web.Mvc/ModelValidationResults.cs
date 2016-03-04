using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
namespace JieNuo.Web.Mvc
{
	public class ModelValidationResults : System.Collections.Generic.IEnumerable<ModelValidationResult>, System.Collections.IEnumerable
	{
		private System.Collections.Generic.List<ModelValidationResult> validationResults = new System.Collections.Generic.List<ModelValidationResult>();
		public void Add(string memberName, string message)
		{
			this.validationResults.Add(new ModelValidationResult
			{
				MemberName = memberName, 
				Message = message
			});
		}
		public virtual void Add(string memberName, string messageFormat, params object[] args)
		{
			this.Add(memberName, string.Format(messageFormat, args));
		}
		public virtual void Add<T, TMember>(System.Linq.Expressions.Expression<System.Func<T, TMember>> expression, string message)
		{
			this.Add(ExpressionHelper2.GetExpressionText(expression), message);
		}
		public void Add<T, TMember>(System.Linq.Expressions.Expression<System.Func<T, TMember>> expression, string messageFormat, params object[] args)
		{
			this.Add(ExpressionHelper2.GetExpressionText(expression), string.Format(messageFormat, args));
		}
		public System.Collections.Generic.IEnumerator<ModelValidationResult> GetEnumerator()
		{
			return this.validationResults.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.validationResults.GetEnumerator();
		}
	}
}
