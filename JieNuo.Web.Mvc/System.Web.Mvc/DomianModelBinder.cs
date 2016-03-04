using JieNuo.ComponentModel;
using JieNuo.Data.NHibernates;
using System;
using System.Threading;
namespace System.Web.Mvc
{
	public class DomianModelBinder : IModelBinder
	{
		private static CommonRepository repository = new CommonRepository();
		private static DefaultModelBinder defaultModelBinder = new DefaultModelBinder();
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			object result2;
			if (bindingContext.ModelName == "")
			{
				result2 = DomianModelBinder.defaultModelBinder.BindModel(controllerContext, bindingContext);
			}
			else
			{
				object result = null;
				bool keyExist = false;
				if (result == null && !keyExist)
				{
					result = this.GetbyBusinessID(bindingContext, bindingContext.ModelName + "_bid", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = this.GetbyDataBaseID(bindingContext, bindingContext.ModelName + "_id", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = this.GetbyDataBaseID(bindingContext, bindingContext.ModelName + ".ID", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = this.GetbyDataBaseID(bindingContext, bindingContext.ModelName + "ID", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = this.GetByCode(bindingContext, bindingContext.ModelName + ".Code", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = this.GetByCode(bindingContext, bindingContext.ModelName + "Code", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = this.GetByCode(bindingContext, "Code", out keyExist);
				}
				if (result == null && !keyExist)
				{
					result = DomianModelBinder.defaultModelBinder.BindModel(controllerContext, bindingContext);
				}
				if (result == null && !keyExist)
				{
					result = this.GetbyDataBaseID(bindingContext, "ID", out keyExist);
				}
				result2 = result;
			}
			return result2;
		}
		private object GetbyDataBaseID(ModelBindingContext bindingContext, string key, out bool keyExists)
		{
			ValueProviderResult idValue = bindingContext.ValueProvider.GetValue(key);
			keyExists = (idValue != null);
			object result;
			if (idValue == null)
			{
				result = null;
			}
			else
			{
				try
				{
					object id = idValue.ConvertTo(typeof(int), System.Threading.Thread.CurrentThread.CurrentCulture);
					object retValue = DomianModelBinder.repository.GetByDatabaseID(bindingContext.ModelType, id);
					result = retValue;
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}
		private object GetbyBusinessID(ModelBindingContext bindingContext, string key, out bool keyExists)
		{
			ValueProviderResult idValue = bindingContext.ValueProvider.GetValue(key);
			keyExists = (idValue != null);
			object result;
			if (idValue == null)
			{
				result = null;
			}
			else
			{
				try
				{
					TypeInfoAttribute typeInfo = TypeInfoAttribute.GetTypInfo(bindingContext.ModelType);
					object id = idValue.ConvertTo(typeInfo.BusinessIdPropertyType, System.Threading.Thread.CurrentThread.CurrentCulture);
					object retValue = DomianModelBinder.repository.GetEntityByBusinessID(bindingContext.ModelType, id);
					result = retValue;
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}
		private object GetByCode(ModelBindingContext bindingContext, string key, out bool keyExists)
		{
			ValueProviderResult codeValue = bindingContext.ValueProvider.GetValue(key);
			keyExists = (codeValue != null);
			object result;
			if (codeValue == null)
			{
				result = null;
			}
			else
			{
				string code = codeValue.AttemptedValue;
				if (string.IsNullOrEmpty(code))
				{
					result = null;
				}
				else
				{
					try
					{
						result = DomianModelBinder.repository.GetByCode(bindingContext.ModelType, code);
					}
					catch
					{
						result = null;
					}
				}
			}
			return result;
		}
	}
}
