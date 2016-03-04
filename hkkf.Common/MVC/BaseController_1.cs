using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections;
using System.Text;

namespace System.Web.Mvc
{
    public class BaseController<T> : BaseController
        where T: class
    {
        protected readonly string SessionKeyPrefix;

        public BaseController()
        {
            SessionKeyPrefix = this.GetType().GUID.ToString() + "_Key_";
        }


        #region GetFromSession

        public T GetFromSession(Func<T, bool> predicate)
        {
            string prefix = GetPrefix<T>();
            return Session.Keys.Cast<string>()
                .Where(k => k.StartsWith(prefix))
                .Select(k => Session[k])
                .OfType<T>()
                .FirstOrDefault(predicate);
        }

        //public TModel GetFromSession<TModel>(string key)
        //    where TModel : class
        //{
        //    return GetFromSessionInternal(key) as TModel;
        //}

        public TModel GetFromSession<TModel>(string key)
            where TModel : class
        {
            return GetFromSessionInternal(GetSessionKey<TModel>(key)) as TModel;
        }

        public T GetFromSession(string code)
        {
            return GetFromSessionInternal(GetSessionKey<T>(code)) as T;
        }

        public T GetFromSession(int id)
        {
            return GetFromSessionInternal(GetSessionKey<T>(id)) as T;
        }

        public T GetFromSession(int id, string suffix)
        {
            return GetFromSessionInternal(GetSessionKey<T>(id, suffix)) as T;
        }
        #endregion

        #region SetToSession

        public void SetToSession<TModel>(string key, TModel model)
            where TModel: class
        {
            SetToSessionInternal(GetSessionKey<TModel>(key), model);
        }

        public void SetToSession(string code, T value)
        {
            SetToSessionInternal(GetSessionKey<T>(code), value);
        }

        public void SetToSession(int id, T value)
        {
            SetToSessionInternal(GetSessionKey<T>(id), value);
        }


        public void SetToSession(Guid guid, T value)
        {
            SetToSessionInternal(GetSessionKey<T>(guid), value);
        }

        public void SetToSession(int id, string suffix, T value)
        {
            SetToSessionInternal(GetSessionKey<T>(id, suffix), value);
        }
        #endregion

        #region ClearSession

        public void CleanSession<TModel>(string key)
            where TModel : class
        {
            CleanSessionInternal(GetSessionKey<TModel>(key));
        }

        public void ClearSession(string code)
        {
            CleanSessionInternal(GetSessionKey<T>(code));
        }

        public void ClearSession(int id)
        {
            CleanSessionInternal(GetSessionKey<T>(id));
        }

        public void ClearSession(int id, string suffix)
        {
            CleanSessionInternal(GetSessionKey<T>(id, suffix));
        }

        #endregion



        #region GetKey
        private string GetSessionKey<TModel>(string str)
        {
            return String.Format("{0}_{1:B}_{2}", GetPrefix<TModel>(), typeof(string).GUID, str);
        }

        private string GetSessionKey<TModel>(int id)
        {
            return String.Format("{0}_{1:B}_{2}", GetPrefix<TModel>(), typeof(int).GUID, id);
        }


        private string GetSessionKey<TModel>(Guid guid)
        {
            return String.Format("{0}_{1:B}_{2}", GetPrefix<TModel>(), typeof(Guid).GUID, guid);
        }

        private string GetSessionKey<TModel>(int id, string suffix)
        {
            return String.Format("{0}_{1:B}_{2:B}_{3}_{4}", GetPrefix<TModel>(), typeof(int).GUID, typeof(string).GUID, id, suffix);
        }

        private string GetPrefix<TModel>()
        {
            return string.Format("{0:B}_{1:B}_", this.GetType().GUID, typeof(TModel).GUID);
        }
        #endregion

        protected internal ViewResult View(string viewName, Type type, object model)
        {
            return View("{0}_{1}".FormatWith(viewName, type.Name), model);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}
