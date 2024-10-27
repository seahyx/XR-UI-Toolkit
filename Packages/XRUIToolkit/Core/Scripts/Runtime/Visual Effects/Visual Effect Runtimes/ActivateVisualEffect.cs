using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "ActivateEffect", menuName = "HHUI Toolkit/Visual Effects/Activate Effect", order = 0)]
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

		protected override void OnChangeState(
			GameObject target,
			XRBaseInteractable interactable,
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
			}
		}
	}
}