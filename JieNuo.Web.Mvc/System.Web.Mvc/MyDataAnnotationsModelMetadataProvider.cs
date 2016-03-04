using JieNuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace System.Web.Mvc
{
	public class MyDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		public static readonly string EditableWhenCreate_Key = "EditableWhenCreate";
		public static readonly string EditableWhenEdit_Key = "EditableWhenEdit";
		public static readonly string RealSort_Key = "RealSort";
		public static readonly string KeyValueInfoAttribute_Prefix = "KeyValueInfoAttribute";
		public static readonly string Icon_Prefix = "IconAttribute";
		public static readonly string CssClass_Prefix = "IconAttribute";
		public static readonly string ConverterTypeName_Key = "TypeConverter_ConverterTypeName";
		protected override ModelMetadata CreateMetadata(System.Collections.Generic.IEnumerable<System.Attribute> attributes, System.Type containerType, System.Func<object> modelAccessor, System.Type modelType, string propertyName)
		{
			ModelMetadata metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
			PropertyEditableAttribute editable = attributes.OfType<PropertyEditableAttribute>().FirstOrDefault<PropertyEditableAttribute>();
			if (editable == null)
			{
				editable = PropertyEditableAttribute.Default;
			}
			metadata.AdditionalValues.Add(MyDataAnnotationsModelMetadataProvider.EditableWhenCreate_Key, editable.WhenCreate);
			metadata.AdditionalValues.Add(MyDataAnnotationsModelMetadataProvider.EditableWhenEdit_Key, editable.WhenEdit);
			metadata.AdditionalValues.Add(MyDataAnnotationsModelMetadataProvider.Icon_Prefix, (
				from i in attributes.OfType<IconAttribute>()
				select i.File).FirstOrDefault<string>());
			metadata.AdditionalValues.Add(MyDataAnnotationsModelMetadataProvider.RealSort_Key, (
				from s in attributes.OfType<RealSortAttribute>()
				select s.Expression).FirstOrDefault<string>());
			TypeConverterAttribute typeConverterAttribute = attributes.OfType<TypeConverterAttribute>().FirstOrDefault<TypeConverterAttribute>();
			if (typeConverterAttribute != null && typeConverterAttribute.ConverterTypeName != null)
			{
				metadata.AdditionalValues.Add(MyDataAnnotationsModelMetadataProvider.ConverterTypeName_Key, typeConverterAttribute.ConverterTypeName);
			}
			foreach (KeyValueInfoAttribute item in attributes.OfType<KeyValueInfoAttribute>())
			{
				metadata.AdditionalValues.Add(MyDataAnnotationsModelMetadataProvider.KeyValueInfoAttribute_Prefix + item.Key, item.Value);
			}
			DataFormatAttribute dataFormatAttribute = attributes.OfType<DataFormatAttribute>().FirstOrDefault<DataFormatAttribute>();
			if (dataFormatAttribute != null)
			{
				metadata.EditFormatString = dataFormatAttribute.EditFormatStringWithBraces;
				metadata.DisplayFormatString = dataFormatAttribute.DiaplayFormatStringWithBraces;
				metadata.NullDisplayText = dataFormatAttribute.NullDisplayText;
				metadata.ConvertEmptyStringToNull = dataFormatAttribute.ConvertEmptyStringToNull;
			}
			return metadata;
		}
	}
}
