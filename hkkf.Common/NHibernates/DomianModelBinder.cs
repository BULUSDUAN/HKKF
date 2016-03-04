using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using JieNuo.ComponentModel;


namespace hkkf.Common
{
    public class DomianModelBinder : IModelBinder
    {
        private static CommonRepository repository = new CommonRepository();
        private static DefaultModelBinder defaultModelBinder = new DefaultModelBinder();

        #region IModelBinder 成员

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelName == "")
                return defaultModelBinder.BindModel(controllerContext, bindingContext);

            object result = null;
            bool keyExist = false;

            //if (result == null && keyExist == false)//根据业务ID获取//for valueBox
            //    result = GetbyBusinessID(bindingContext, bindingContext.ModelName + "_bid", out keyExist);
            //if (result == null && keyExist == false)
            //    result = GetbyDataBaseID(bindingContext, bindingContext.ModelName + "_id", out keyExist);
            //if (result == null && keyExist == false)
            //    result = GetbyDataBaseID(bindingContext, bindingContext.ModelName + ".ID", out keyExist);
            //if (result == null && keyExist == false)
            //    result = GetbyDataBaseID(bindingContext, bindingContext.ModelName + "ID", out keyExist);

            //if (result == null && keyExist == false)
            //    result = GetByCode(bindingContext, bindingContext.ModelName + ".Code", out keyExist);
            //if (result == null && keyExist == false)//根据 XXXXCode获取
            //    result = GetByCode(bindingContext, bindingContext.ModelName + "Code", out keyExist);
            //if (result == null && keyExist == false)
            //    result = GetByCode(bindingContext, "Code", out keyExist);


            if (result == null && keyExist == false)
                result = GetbyDataBaseID(bindingContext, "ID", out keyExist);

            if (result == null && keyExist == false)
                result = defaultModelBinder.BindModel(controllerContext, bindingContext);

            return result;
        }
        #endregion

        private object GetbyDataBaseID(ModelBindingContext bindingContext, string key, out bool keyExists)
        {
            var idValue = bindingContext.ValueProvider.GetValue(key);
            keyExists = idValue != null;
            if (idValue == null) return null;

            try
            {
                object id = idValue.ConvertTo(typeof(int), Thread.CurrentThread.CurrentCulture);
                object retValue = repository.GetByDatabaseID(bindingContext.ModelType, id);
                return retValue;
            }
            catch
            {
                return null;
            }
        }

        private object GetbyBusinessID(ModelBindingContext bindingContext, string key, out bool keyExists)
        {
            var idValue = bindingContext.ValueProvider.GetValue(key);
            keyExists = idValue != null;
            if (idValue == null) return null;

            try
            {
                TypeInfoAttribute typeInfo = TypeInfoAttribute.GetTypInfo(bindingContext.ModelType);
                object id = idValue.ConvertTo(typeInfo.BusinessIdPropertyType, Thread.CurrentThread.CurrentCulture);
                object retValue = repository.GetEntityByBusinessID(bindingContext.ModelType, id);
                return retValue;
            }
            catch
            {
                return null;
            }
        }

        private object GetByCode(ModelBindingContext bindingContext, string key, out bool keyExists)
        {
            var codeValue = bindingContext.ValueProvider.GetValue(key);
            keyExists = codeValue != null;
            if (codeValue == null) return null;

            string code = codeValue.AttemptedValue;
            if (string.IsNullOrEmpty(code)) return null;

            try
            {
                return repository.GetByCode(bindingContext.ModelType, code);
            }
            catch
            {
                return null;
            }
        }
    }
}
