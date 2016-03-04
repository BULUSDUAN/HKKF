using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace JieNuo.Web.Mvc.Validation
{
	public class ValidatableModelValidator : ModelValidator
	{
		public ValidatableModelValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
		{
		}
		public override System.Collections.Generic.IEnumerable<ModelValidationResult> Validate(object container)
		{
			IValidatableModel model = base.Metadata.Model as IValidatableModel;
			System.Collections.Generic.IEnumerable<ModelValidationResult> result;
			if (model == null)
			{
				result = System.Linq.Enumerable.Empty<ModelValidationResult>();
			}
			else
			{
				result = model.ValidateModel();
			}
			return result;
		}
	}
}
