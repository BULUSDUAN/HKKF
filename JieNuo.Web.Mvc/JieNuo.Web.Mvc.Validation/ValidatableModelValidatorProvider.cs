using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace JieNuo.Web.Mvc.Validation
{
	public class ValidatableModelValidatorProvider : ModelValidatorProvider
	{
		public override System.Collections.Generic.IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
		{
			if (metadata.Model is IValidatableModel)
			{
				yield return new ValidatableModelValidator(metadata, context);
			}
			yield break;
		}
	}
}
