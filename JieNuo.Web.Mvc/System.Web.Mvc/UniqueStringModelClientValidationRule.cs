using System;
namespace System.Web.Mvc
{
	public class UniqueStringModelClientValidationRule : ModelClientValidationRule
	{
		public UniqueStringModelClientValidationRule(string errorMessage, System.Type type, string property)
		{
			base.ErrorMessage = errorMessage;
			base.ValidationType = "uniqueString";
			base.ValidationParameters["type"] = type.AssemblyQualifiedName;
			base.ValidationParameters["property"] = property;
		}
	}
}
