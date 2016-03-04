using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;

namespace System.Collections.Generic
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary Clone(this RouteValueDictionary dict)
        {
            return new RouteValueDictionary(dict);
        }

        public static RouteValueDictionary SelfAdd(this RouteValueDictionary dict, string key, object value, bool replace = true)
        {
            if (dict.ContainsKey(key))
            {
                if(replace) dict[key] = value;
            }
            else dict.Add(key, value);
            return dict;
        }
        

        public static RouteValueDictionary SelfAdd(this RouteValueDictionary dict, IDictionary<string, object> values, bool replace)
        {
            dict.AddRange(values, replace);
            return dict;
        }


        public static RouteValueDictionary SelfAdd(this RouteValueDictionary dict, object values, bool replace)
        {
            dict.AddRange(new RouteValueDictionary(values), replace);
            return dict;
        }


        public static IDictionary<string, object> Unit(this RouteValueDictionary dict, RouteValueDictionary dict2)
        {
            RouteValueDictionary d = new RouteValueDictionary(dict);
            d.AddRange(dict2, true);
            return d;
        }

    }
}
