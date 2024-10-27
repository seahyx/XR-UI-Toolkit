using UnityEngine;

namespace XRUIToolkit.Core.UIConfig
{
	public abstract class UIConfigItem: Object
	{
		[SerializeField, Tooltip("Name of this UI element")]
		public string Title = "UI Element";
	}
}

