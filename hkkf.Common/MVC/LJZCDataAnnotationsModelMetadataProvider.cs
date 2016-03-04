using System.Collections.Generic;
using System.Linq;
using JieNuo.ComponentModel;
using System.ComponentModel;
using hkkf.Common.Attributes;

namespace System.Web.Mvc
{
    public class LJZCDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public static readonly string EditableWhenCreate_Key = "EditableWhenCreate";
        public static readonly string EditableWhenEdit_Key = "EditableWhenEdit";
        public static readonly string RealSort_Key = "RealSort";
        public static readonly string KeyValueInfoAttribute_Prefix = "KeyValueInfoAttribute";
        public static readonly string Icon_Prefix = "IconAttribute";
        public static readonly string CssClass_Prefix = "IconAttribute";
        public static readonly string ConverterTypeName_Key = "TypeConverter_ConverterTypeName";
        public static readonly string EnumerationAttribute_Key = "Enumeration";
        public static readonly string BoolAttribute_Key = "Bool";
        public static readonly string AllAttribute_Key = "AllAttribute";


        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            ModelMetadata metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            PropertyEditableAttribute editable = attributes.OfType<PropertyEditableAttribute>().FirstOrDefault();
            if (editable == null) editable = PropertyEditableAttribute.Default;
            metadata.AdditionalValues.Add(EditableWhenCreate_Key, editable.WhenCreate);
            metadata.AdditionalValues.Add(EditableWhenEdit_Key, editable.WhenEdit);

            metadata.AdditionalValues.Add(Icon_Prefix, attributes.OfType<IconAttribute>().Select(i => i.File).FirstOrDefault());
            metadata.AdditionalValues.Add(RealSort_Key, attributes.OfType<RealSortAttribute>().Select(s => s.Expression).FirstOrDefault());

            TypeConverterAttribute typeConverterAttribute = attributes.OfType<TypeConverterAttribute>().FirstOrDefault();
            if (typeConverterAttribute != null && typeConverterAttribute.ConverterTypeName != null)
                metadata.AdditionalValues.Add(ConverterTypeName_Key, typeConverterAttribute.ConverterTypeName);

            foreach (var item in attributes.OfType<KeyValueInfoAttribute>())
                metadata.AdditionalValues.Add(KeyValueInfoAttribute_Prefix + item.Key, item.Value);

            DataFormatAttribute dataFormatAttribute = attributes.OfType<DataFormatAttribute>().FirstOrDefault();
            if (dataFormatAttribute != null)
            {
                metadata.EditFormatString = dataFormatAttribute.EditFormatStringWithBraces;
                metadata.DisplayFormatString = dataFormatAttribute.DiaplayFormatStringWithBraces;
                metadata.NullDisplayText = dataFormatAttribute.NullDisplayText;
                metadata.ConvertEmptyStringToNull = dataFormatAttribute.ConvertEmptyStringToNull;
            }

            EnumerationAttribute enumerationAttribute = attributes.OfType<EnumerationAttribute>().FirstOrDefault();
            if (enumerationAttribute != null)
                metadata.AdditionalValues.Add(EnumerationAttribute_Key, enumerationAttribute);

            BoolAttribute boolAttribute = attributes.OfType<BoolAttribute>().FirstOrDefault();
            if (boolAttribute != null)
                metadata.AdditionalValues.Add(BoolAttribute_Key, boolAttribute);

            metadata.AdditionalValues.Add(AllAttribute_Key, attributes);

            return metadata;
        }
    }

    public static class ModelMetadataExtension
    {
        public static bool EditableWhenCreate(this ModelMetadata modelMetadata)
        {
            object editable = modelMetadata.AdditionalValues[LJZCDataAnnotationsModelMetadataProvider.EditableWhenCreate_Key];
            return editable is bool && (bool)editable;
        }

        public static bool EditableWhenEdit(this ModelMetadata modelMetadata)
        {
            object editable = modelMetadata.AdditionalValues[LJZCDataAnnotationsModelMetadataProvider.EditableWhenEdit_Key];
            return editable is bool && (bool)editable;
        }

        public static string RealSort(this ModelMetadata modelMetadata)
        {
            object editable = modelMetadata.AdditionalValues[LJZCDataAnnotationsModelMetadataProvider.RealSort_Key];
            return editable as string;
        }

        public static string Icon(this ModelMetadata modelMetadata)
        {
            object icon = modelMetadata.AdditionalValues[LJZCDataAnnotationsModelMetadataProvider.Icon_Prefix];
            return icon as string;
        }

        public static string CssClass(this ModelMetadata modelMetadata)
        {
            object cssClass = modelMetadata.AdditionalValues[LJZCDataAnnotationsModelMetadataProvider.CssClass_Prefix];
            return cssClass as string;
        }

        public static T KeyValueAttributeValue<T>(this ModelMetadata modelMetadata, string key)
        {
            string keyValueAttributeKey = LJZCDataAnnotationsModelMetadataProvider.KeyValueInfoAttribute_Prefix + key;
            if (modelMetadata.AdditionalValues.ContainsKey(keyValueAttributeKey))
            {
                object value = modelMetadata.AdditionalValues[keyValueAttributeKey];
                if (value is T) return (T)value;
            }
            return default(T);
        }

        public static bool ExistsKeyValueAttribute<T>(this ModelMetadata modelMetadata, string key)
        {
            string keyValueAttributeKey = LJZCDataAnnotationsModelMetadataProvider.KeyValueInfoAttribute_Prefix + key;
            if (modelMetadata.AdditionalValues.ContainsKey(keyValueAttributeKey) == false) return false;

            return modelMetadata.AdditionalValues[keyValueAttributeKey] is T;
        }


        private static Dictionary<string, TypeConverter> typeConverterDict = new Dictionary<string, TypeConverter>();

        public static TypeConverter TypeConverter(this ModelMetadata modelMetadata)
        {
            string converterTypeName = modelMetadata.AdditionalValues.TryGetValue(LJZCDataAnnotationsModelMetadataProvider.ConverterTypeName_Key) as string;

            if (converterTypeName.IsNotNullAndEmpty())
            {
                if (typeConverterDict.ContainsKey(converterTypeName)) return typeConverterDict[converterTypeName];

                TypeConverter convert = null;
                try
                {
                    Type type = Type.GetType(converterTypeName);
                    convert = Activator.CreateInstance(type) as TypeConverter;
                }
                catch
                {
                    convert = TypeDescriptor.GetConverter(modelMetadata.ModelType);
                }

                typeConverterDict.Add(converterTypeName, convert);
                return convert;
            }
            return TypeDescriptor.GetConverter(modelMetadata.ModelType);
        }

        public static bool IsEnumeration(this ModelMetadata modelMetadata)
        {
            return modelMetadata.AdditionalValues.ContainsKey(LJZCDataAnnotationsModelMetadataProvider.EnumerationAttribute_Key);
        }

        public static EnumerationAttribute EnumerationAttribute(this ModelMetadata modelMetadata)
        {
            var key = LJZCDataAnnotationsModelMetadataProvider.EnumerationAttribute_Key;
            if (modelMetadata.AdditionalValues.ContainsKey(key) == false) return null;
            return  modelMetadata.AdditionalValues[key] as EnumerationAttribute;
        }


        public static BoolAttribute BoolAttribute(this ModelMetadata modelMetadata)
        {
            var key = LJZCDataAnnotationsModelMetadataProvider.BoolAttribute_Key;
            if (modelMetadata.AdditionalValues.ContainsKey(key) == false) return null;
            return modelMetadata.AdditionalValues[key] as BoolAttribute;
        }


        public static IEnumerable<Attribute> AllAttribute(this ModelMetadata modelMetadata)
        {
            var key = LJZCDataAnnotationsModelMetadataProvider.AllAttribute_Key;
            if (modelMetadata.AdditionalValues.ContainsKey(key) == false)
                throw new InvalidOperationException(" AllAttribute 必须与 LJZCDataAnnotationsModelMetadataProvider配合使用。");
            return modelMetadata.AdditionalValues[key] as IEnumerable<Attribute>;

        }

    }
}
