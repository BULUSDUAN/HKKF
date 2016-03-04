using System;
using System.Globalization;
using System.Text;
namespace System.Web.Mvc.Ajax
{
	public class JQueryOptions
	{
		public string LoadingElement
		{
			get;
			set;
		}
		public string UpdateTarget
		{
			get;
			set;
		}
		public string InputToFocus
		{
			get;
			set;
		}
		public string Confirm
		{
			get;
			set;
		}
		public InsertionMode? InsertionMode
		{
			get;
			set;
		}
		public FormMethod? FormMethod
		{
			get;
			set;
		}
		public string OnBeforeSend
		{
			get;
			set;
		}
		public string OnSuccess
		{
			get;
			set;
		}
		public string OnComplete
		{
			get;
			set;
		}
		public string OnError
		{
			get;
			set;
		}
		public string Data
		{
			get;
			set;
		}
		public string Precondition
		{
			get;
			set;
		}
		public string GetData
		{
			get;
			set;
		}
		public JQueryOptions(string updateTarget = null, string inputToFocus = null)
		{
			this.UpdateTarget = updateTarget;
			this.InputToFocus = inputToFocus;
			this.InsertionMode = new InsertionMode?(System.Web.Mvc.Ajax.InsertionMode.Replace);
			this.FormMethod = new FormMethod?(System.Web.Mvc.FormMethod.Post);
		}
		private static string PropertyStringIfSpecified(string propertyName, string propertyValue)
		{
			string result;
			if (!string.IsNullOrEmpty(propertyValue))
			{
				string str = propertyValue.Replace("'", "\\'");
				result = string.Format(System.Globalization.CultureInfo.InvariantCulture, " {0}: '{1}',", new object[]
				{
					propertyName, 
					str
				});
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		private static string EventStringIfSpecified(string propertyName, string handler)
		{
			string result;
			if (!string.IsNullOrEmpty(handler))
			{
				result = string.Format(System.Globalization.CultureInfo.InvariantCulture, " {0}: function(){{{1}}},", new object[]
				{
					propertyName, 
					handler
				});
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		internal string ToJavascriptString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("{");
			builder.Append(JQueryOptions.PropertyStringIfSpecified("loadingElement", this.LoadingElement));
			builder.Append(JQueryOptions.PropertyStringIfSpecified("updateTarget", this.UpdateTarget));
			builder.Append(JQueryOptions.PropertyStringIfSpecified("inputToFocus", this.InputToFocus));
			builder.Append(JQueryOptions.PropertyStringIfSpecified("data", this.Data));
			builder.Append(JQueryOptions.PropertyStringIfSpecified("confirm", this.Confirm));
			FormMethod? formMethod = this.FormMethod;
			if (((formMethod.GetValueOrDefault() != System.Web.Mvc.FormMethod.Post) ? 1 : ((!formMethod.HasValue) ? 1 : 0)) != 0)
			{
				builder.Append(string.Format("type: {0}", this.FormMethod));
			}
			InsertionMode? insertionMode = this.InsertionMode;
			if (((insertionMode.GetValueOrDefault() != System.Web.Mvc.Ajax.InsertionMode.Replace) ? 1 : ((!insertionMode.HasValue) ? 1 : 0)) != 0)
			{
				builder.Append(string.Format(" insertionMode: {0},", this.InsertionMode));
			}
			builder.Append(JQueryOptions.EventStringIfSpecified("beforeSend", this.OnBeforeSend));
			builder.Append(JQueryOptions.EventStringIfSpecified("beforeSend", this.OnBeforeSend));
			builder.Append(JQueryOptions.EventStringIfSpecified("complete", this.OnComplete));
			builder.Append(JQueryOptions.EventStringIfSpecified("error", this.OnError));
			builder.Append(JQueryOptions.EventStringIfSpecified("success", this.OnSuccess));
			builder.Append(JQueryOptions.EventStringIfSpecified("getData", this.GetData));
			builder.Append(JQueryOptions.EventStringIfSpecified("precondition", this.Precondition));
			builder.Length--;
			builder.Append(" }");
			return builder.ToString();
		}
	}
}
