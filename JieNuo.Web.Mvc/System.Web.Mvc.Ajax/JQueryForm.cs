using System;
using System.IO;
namespace System.Web.Mvc.Ajax
{
	public class JQueryForm : System.IDisposable
	{
		private bool _disposed;
		private readonly FormContext _originalFormContext;
		private readonly ViewContext _viewContext;
		private readonly System.IO.TextWriter _writer;
		private JQueryOptions _Options;
		private string _Id;
		public JQueryOptions Options
		{
			get
			{
				return this._Options;
			}
			set
			{
				this._Options = value;
			}
		}
		public string Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				this._Id = value;
			}
		}
		public JQueryForm(ViewContext viewContext, string id, JQueryOptions options)
		{
			if (viewContext == null)
			{
				throw new System.ArgumentNullException("viewContext");
			}
			this._viewContext = viewContext;
			this._writer = viewContext.Writer;
			this._originalFormContext = viewContext.FormContext;
			this._Id = id;
			this._Options = (options ?? new JQueryOptions(null, null));
			viewContext.FormContext = new FormContext();
		}
		public void Dispose()
		{
			this.Dispose(true);
			System.GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				this._disposed = true;
				this._writer.Write("</form>");
				this._writer.Write("<script type=\"text/javascript\">$('#{0}').submit(function(){{ $(this).ajaxForm({1});return false;}})</script>", this.Id, this.Options.ToJavascriptString());
				if (this._viewContext != null)
				{
					this._viewContext.OutputClientValidation();
					this._viewContext.FormContext = this._originalFormContext;
				}
			}
		}
		public void EndForm()
		{
			this.Dispose(true);
		}
	}
}
