using System;
using System.ComponentModel.DataAnnotations;
namespace System.Web.Mvc
{
	public class UniqueStringAttribute : ValidationAttribute
	{
		static UniqueStringAttribute()
		{
			DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(UniqueStringAttribute), new DataAnnotationsModelValidationFactory(UniqueStringAttribute.Create));
		}
		public UniqueStringAttribute() : base("该{0}已使用")
		{
		}
		public override bool IsValid(object value)
		{
			return true;
		}
		public static ModelValidator Create(ModelMetadata metadata, ControllerContext context, ValidationAttribute attribute)
		{
			return new UniqueStringModelValidator(metadata, context, (UniqueStringAttribute)attribute);
		}
	}
}
