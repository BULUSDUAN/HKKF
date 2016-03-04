using System;
namespace System.Collections.Generic
{
	public static class StringTDictionaryExtensinon
	{
		public static T TryGetValue<T>(this IDictionary<string, T> dict, string key)
		{
			return dict.TryGetValue(key, default(T));
		}
		public static T TryGetValue<T>(this IDictionary<string, T> dict, string key, T defaultValue)
		{
			if (dict == null)
			{
				throw new System.ArgumentNullException("dict");
			}
			T result;
			if (dict.ContainsKey(key))
			{
				result = dict[key];
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}
	}
}
