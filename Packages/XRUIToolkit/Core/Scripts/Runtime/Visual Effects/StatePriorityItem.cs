using System;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	/// <summary>
	/// Used to represent an interactable state in the state priority list.
	/// </summary>
	[Serializable]
	public struct StatePriorityItem
	{
		public StatePriorityItem(bool enabled, InteractableStates state)
		{
			this.enabled = enabled;
			this.state = state;
		}

		[Tooltip("Toggle this state's effects. If disabled, this state will not have any effect.")]
		public bool enabled;
		[SerializeField]
		public InteractableStates state;
		public string Name => state.ToString();
	}
}
