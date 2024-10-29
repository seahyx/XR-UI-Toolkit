using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "AnimationTriggerVisualEffect", menuName = "HHUI Toolkit/Visual Effects/Animation Trigger Effect", order = 0)]
	public class AnimationTriggerVisualEffect : BaseVisualEffect
	{
		[SerializeField,
			Tooltip("Animation trigger to be activated when entering the idle state.")]
		private string idle = "";

		[SerializeField,
			Tooltip("Animation trigger to be activated when entering the hover state.")]
		private string hover = "";

		[SerializeField,
			Tooltip("Animation trigger to be activated when entering the selected state.")]
		private string selected = "";

		[SerializeField,
			Tooltip("Animation trigger to be activated when entering the focus state.")]
		private string focused = "";

		[SerializeField,
			Tooltip("Animation trigger to be activated when entering the activated state.")]
		private string activated = "";

		private Animator targetAnimator;

		public override void Initialize(XRBaseInteractable interactable, GameObject target)
		{
			base.Initialize(interactable, target);

			Animator animator;
			if (target != null && target.TryGetComponent(out animator))
				targetAnimator = animator;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (targetAnimator == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild animator. Effect is not yet initialized.");
				return false;
			}
			return true;
		}

		protected override void OnChangeState(
			GameObject target,
			XRBaseInteractable interactable,
			InteractableStates prevState,
			InteractableStates currentState)
		{
			switch(currentState)
			{
				case InteractableStates.Idle:
					targetAnimator.SetTrigger(idle);
					break;
				case InteractableStates.Hover:
					targetAnimator.SetTrigger(hover);
					break;
				case InteractableStates.Select:
					targetAnimator.SetTrigger(selected);
					break;
				case InteractableStates.Focus:
					targetAnimator.SetTrigger(focused);
					break;
				case InteractableStates.Activated:
					targetAnimator.SetTrigger(activated);
					break;
			}
		}
	}
}