using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "ActivateVFX", menuName = "HHUI Toolkit/Visual Effects/General/Activate VFX", order = 0)]
	[DropdownMenuName("General/Activate VFX")]
	public class ActivateVisualEffect : BaseVisualEffect
	{
		[SerializeField,
			Tooltip("Whether the target object will be active in the idle state.")]
		private bool idle = true;

		[SerializeField,
			Tooltip("Whether the target object will be active in the hover state.")]
		private bool hover = true;

		[SerializeField,
			Tooltip("Whether the target object will be active in the selected state.")]
		private bool selected = true;

		[SerializeField,
			Tooltip("Whether the target object will be active in the focus state.")]
		private bool focused = true;

		[SerializeField,
			Tooltip("Whether the target object will be active in the activated state.")]
		private bool activated = true;

		[SerializeField,
			Tooltip("Whether the target object will be active in the disabled state.")]
		private bool disabled = true;

		protected override void OnChangeState(
			GameObject target,
			BaseVisualEffectsController controller,
			InteractableStates prevState,
			InteractableStates currentState)
		{
			switch(currentState)
			{
				case InteractableStates.Idle:
					target.SetActive(idle);
					break;
				case InteractableStates.Hover:
					target.SetActive(hover);
					break;
				case InteractableStates.Select:
					target.SetActive(selected);
					break;
				case InteractableStates.Focus:
					target.SetActive(focused);
					break;
				case InteractableStates.Activated:
					target.SetActive(activated);
					break;
				case InteractableStates.Disabled:
					target.SetActive(disabled);
					break;
			}
		}
	}
}
