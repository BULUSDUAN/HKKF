using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace System.Web.Mvc
{
	public static class ModelMetadataExtension
	{
		private static System.Collections.Generic.Dictionary<string, TypeConverter> typeConverterDict = new System.Collections.Generic.Dictionary<string, TypeConverter>();
		public static bool EditableWhenCreate(this ModelMetadata modelMetadata)
		{
			object editable = modelMetadata.AdditionalValues[MyDataAnnotationsModelMetadataProvider.EditableWhenCreate_Key];
			return editable is bool && (bool)editable;
		}
		public static bool EditableWhenEdit(this ModelMetadata modelMetadata)
		{
			object editable = modelMetadata.AdditionalValues[MyDataAnnotationsModelMetadataProvider.EditableWhenEdit_Key];
			return editable is bool && (bool)editable;
		}
		public static string RealSort(this ModelMetadata modelMetadata)
		{
			object editable = modelMetadata.AdditionalValues[MyDataAnnotationsModelMetadataProvider.RealSort_Key];
			return editable as string;
		}
		public static string Icon(this ModelMetadata modelMetadata)
		{
			object icon = modelMetadata.AdditionalValues[MyDataAnnotationsModelMetadataProvider.Icon_Prefix];
			return icon as string;
		}
		public static string CssClass(this ModelMetadata modelMetadata)
		{
			object cssClass = modelMetadata.AdditionalValues[MyDataAnnotationsModelMetadataProvider.CssClass_Prefix];
			return cssClass as string;
		}
		public static T KeyValueAttributeValue<T>(this ModelMetadata modelMetadata, string key)
		{
			string keyValueAttributeKey = MyDataAnnotationsModelMetadataProvider.KeyValueInfoAttribute_Prefix + key;
			T result;
			if (modelMetadata.AdditionalValues.ContainsKey(keyValueAttributeKey))
			{
				object value = modelMetadata.AdditionalValues[keyValueAttributeKey];
				if (value is T)
				{
					result = (T)value;
					return result;
				}
			}
			result = default(T);
			return result;
		}
		public static bool ExistsKeyValueAttribute<T>(this ModelMetadata modelMetadata, string key)
		{
			string keyValueAttributeKey = MyDataAnnotationsModelMetadataProvider.KeyValueInfoAttribute_Prefix + key;
			return modelMetadata.AdditionalValues.ContainsKey(keyValueAttributeKey) && modelMetadata.AdditionalValues[keyValueAttributeKey] is T;
		}
		public static TypeConverter TypeConverter(this ModelMetadata modelMetadata)
		{
			string converterTypeName = modelMetadata.AdditionalValues.TryGetValue(MyDataAnnotationsModelMetadataProvider.ConverterTypeName_Key) as string;
			TypeConverter result;
			if (converterTypeName.IsNotNullAndEmpty())
			{
				if (ModelMetadataExtension.typeConverterDict.ContainsKey(converterTypeName))
				{
					result = ModelMetadataExtension.typeConverterDict[converterTypeName];
				}
				else
				{
					TypeConverter convert = null;
					try
					{
						System.Type type = System.Type.GetType(converterTypeName);
						convert = (System.Activator.CreateInstance(type) as TypeConverter);
					}
					catch
					{
						convert = TypeDescriptor.GetConverter(modelMetadata.ModelType);
					}
					ModelMetadataExtension.typeConverterDict.Add(converterTypeName, convert);
					result = convert;
				}
			}
			else
			{
				result = TypeDescriptor.GetConverter(modelMetadata.ModelType);
			}
			return result;
		}
	}
}
