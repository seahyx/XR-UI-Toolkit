using System.Collections.Generic;
using UnityEngine;

namespace XRUIToolkit.Core.UIConfig
{
	public class UIConfigurator : MonoBehaviour
	{
		[SerializeField]
		protected List<UIConfigItem> uiConfigs = new();
	}
}
