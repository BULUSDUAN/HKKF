using JieNuo.Common;
using System;
using System.Web.Mvc;
namespace JieNuo.Web.Mvc
{
	public class TypeModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			string key = "type";
			ValueProviderResult v = bindingContext.ValueProvider.GetValue(key);
			object result;
			if (v == null || string.IsNullOrEmpty(v.AttemptedValue))
			{
				result = null;
			}
			else
			{
				System.Type type = null;
				string typeName = v.AttemptedValue;
				type = System.Type.GetType(typeName);
				if (type == null)
				{
					type = TypeHelper.GetTypeInCurrentDomain(typeName);
				}
				result = type;
			}
			return result;
		}
	}
}
