using System;

namespace XRUIToolkit.Core.VisualEffect
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DropdownMenuNameAttribute : Attribute
	{
		public string MenuName { get; set; }

		public DropdownMenuNameAttribute(string dropdownName)
		{
			MenuName = dropdownName;
		}
	}
}