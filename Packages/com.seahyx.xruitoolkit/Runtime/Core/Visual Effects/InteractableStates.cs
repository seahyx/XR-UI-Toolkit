using System;

namespace XRUIToolkit.Core.VisualEffect
{
	/// <summary>
	/// The visual effect states that an interactable can have.
	/// </summary>
	[Flags]
	public enum InteractableStates
	{
		Idle = 0,
		Hover = 1 << 0,
		Select = 1 << 1,
		Focus = 1 << 2,
		Activated = 1 << 3,
		Disabled = 1 << 4,
		EffectDisabled = 1 << 5,
	}
}
