using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using hkkf.Common.MVC;

namespace System.Web.Mvc
{
#if DEBUG
    [Stopwatch]
#endif
    [HandleError]
    //[DenyIE56("ie-version-is-too-low")]
    public class BaseController : Controller
    {
        //protected bool IsParamChanged(params object[] objs)
        //{
        //    bool changed = false;
        //    string hashValue = Hash.MD5(objs);
        //    string sessionKey = this.GetType().FullName + "_" + "QueryParamKey";//thiw//new StackTrace().GetFrame(1).GetMethod().ToString();
        //    if (Session[sessionKey] as string != hashValue) changed = true;
        //    Session[sessionKey] = hashValue;
        //    return changed;
        //}

        #region Session

        protected void SetToSessionInternal(string key, object value)
        {
            Session[key] = value;
        }

        protected object GetFromSessionInternal(string key)
        {
            return Session[key];
        }

        protected void CleanSessionInternal(string key)
        {
            Session.Remove(key);
        }
        #endregion

        #region TryUpdateModel

        protected internal bool TryUpdateModel(Type type, object model)
        {
            return this.TryUpdateModel(type, model, null, null, null, base.ValueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string prefix)
        {
            return this.TryUpdateModel(type, model, prefix, null, null, base.ValueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, IValueProvider valueProvider)
        {
            return this.TryUpdateModel(type, model, null, null, null, valueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string[] includeProperties)
        {
            return this.TryUpdateModel(type, model, null, includeProperties, null, base.ValueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string prefix, IValueProvider valueProvider)
        {
            return this.TryUpdateModel(type, model, prefix, null, null, valueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string[] includeProperties, IValueProvider valueProvider)
        {
            return this.TryUpdateModel(type, model, null, includeProperties, null, valueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string prefix, string[] includeProperties)
        {
            return this.TryUpdateModel(type, model, prefix, includeProperties, null, base.ValueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string prefix, string[] includeProperties, IValueProvider valueProvider)
        {
            return this.TryUpdateModel(type, model, prefix, includeProperties, null, valueProvider);
        }

        protected internal bool TryUpdateModel(Type type, object model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return this.TryUpdateModel(type, model, prefix, includeProperties, excludeProperties, base.ValueProvider);
        }


        protected internal bool TryUpdateModel(Type type, object model, string prefix, string[] includeProperties, string[] excludeProperties, IValueProvider valueProvider)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }
            Predicate<string> predicate = propertyName => IsPropertyAllowed(propertyName, includeProperties, excludeProperties);
            IModelBinder binder = this.Binders.GetBinder(type);
            ModelBindingContext bindingContext = new ModelBindingContext
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type),
                ModelName = prefix,
                ModelState = this.ModelState,
                PropertyFilter = predicate,
                ValueProvider = valueProvider
            };
            binder.BindModel(base.ControllerContext, bindingContext);
            return this.ModelState.IsValid;
        }
        #endregion

        internal static bool IsPropertyAllowed(string propertyName, string[] includeProperties, string[] excludeProperties)
        {
            bool flag = ((includeProperties == null) || (includeProperties.Length == 0)) || includeProperties.Contains<string>(propertyName, StringComparer.OrdinalIgnoreCase);
            bool flag2 = (excludeProperties != null) && excludeProperties.Contains<string>(propertyName, StringComparer.OrdinalIgnoreCase);
            return (flag && !flag2);
        }
    }
}
