using System;
namespace CSSD.Web.Common
{
	[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class EditableAttribute : System.Attribute
	{
		public static readonly EditableAttribute Default = new EditableAttribute();
		public bool WhenCreate
		{
			get;
			set;
		}
		public bool WhenEdit
		{
			get;
			set;
		}
		public EditableAttribute()
		{
			this.WhenCreate = true;
			this.WhenEdit = true;
		}
		public EditableAttribute(bool whenCreate, bool whenEdit)
		{
			this.WhenCreate = whenCreate;
			this.WhenEdit = whenEdit;
		}
	}
}
