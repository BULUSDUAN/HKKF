using System;
using System.Collections.Generic;
namespace System.Web.Mvc
{
	public class UniqueStringModelValidator : DataAnnotationsModelValidator<UniqueStringAttribute>
	{
		private System.Type type;
		private string property;
		public UniqueStringModelValidator(ModelMetadata metadata, ControllerContext context, UniqueStringAttribute attribute) : base(metadata, context, attribute)
		{
			this.type = metadata.ContainerType;
			this.property = metadata.PropertyName;
		}
		public override System.Collections.Generic.IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			return new UniqueStringModelClientValidationRule[]
			{
				new UniqueStringModelClientValidationRule(base.ErrorMessage, this.type, this.property)
			};
		}
	}
}
