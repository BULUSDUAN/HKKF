using System;
using System.Web.Routing;
namespace System.Collections.Generic
{
	public static class StringObjectDictionaryExtensinon
	{
		public static void AddRange(this IDictionary<string, object> dict, object values, bool replace)
		{
			if (values != null)
			{
				System.Web.Routing.RouteValueDictionary d = new System.Web.Routing.RouteValueDictionary(values);
				dict.AddRange(d, replace);
			}
		}
		public static void AddRange(this IDictionary<string, object> dict, IDictionary<string, object> values, bool replace)
		{
			if (values != null)
			{
				foreach (KeyValuePair<string, object> item in values)
				{
					if (dict.ContainsKey(item.Key))
					{
						if (item.Key == "class")
						{
							string key;
							dict[key = item.Key] = dict[key] + " " + item.Value;
						}
						else
						{
							if (replace)
							{
								dict[item.Key] = item.Value;
							}
						}
					}
					else
					{
						dict.Add(item.Key, item.Value);
					}
				}
			}
		}
	}
}
