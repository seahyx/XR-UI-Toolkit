using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XRUIToolkit.Core.UIConfig
{
	public class TextMeshProUIConfig : UIConfigItem
	{
		[SerializeField,
			Tooltip("List of text mesh pro references for this UI config.")]
		protected List<TextMeshPro> tmpList = new();

		[SerializeField,
			Tooltip("Text to set.")]
		protected string text = "";
	}
}