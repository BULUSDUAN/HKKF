using JieNuo.Data.Exceptions;
using System;
namespace System.Web.Mvc
{
	public static class RuleExceptionExtension
	{
		public static void CopyToModelState(this RuleException ruleException, ModelStateDictionary modelState)
		{
			ruleException.CopyToModelState(modelState, "");
		}
		public static void CopyToModelState(this RuleException ruleException, ModelStateDictionary modelState, string prefix)
		{
			foreach (string key in ruleException.Errors)
			{
				string[] values = ruleException.Errors.GetValues(key);
				for (int i = 0; i < values.Length; i++)
				{
					string value = values[i];
					if (string.IsNullOrEmpty(prefix))
					{
						modelState.AddModelError(key, value);
					}
					else
					{
						modelState.AddModelError(prefix + "." + key, value);
					}
				}
			}
		}
	}
}
