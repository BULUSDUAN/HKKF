using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace JieNuo.Web.Mvc.Validation
{
	public interface IValidatableModel
	{
		System.Collections.Generic.IEnumerable<ModelValidationResult> ValidateModel();
	}
}
