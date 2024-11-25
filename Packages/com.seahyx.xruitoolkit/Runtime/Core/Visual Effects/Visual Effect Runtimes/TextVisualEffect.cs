using TMPro;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "TextVisualEffect", menuName = "HHUI Toolkit/Visual Effects/Text Visual Effect", order = 0)]
	public class TextVisualEffect : BaseVisualEffect
	{
		[SerializeField,
			Tooltip("Text to be set when entering the idle state. An empty string will be set to the initial value.")]
		private string idle = "";

		[SerializeField,
			Tooltip("Text to be set when entering the hover state. An empty string will be set to the initial value.")]
		private string hover = "";

		[SerializeField,
			Tooltip("Text to be set when entering the selected state. An empty string will be set to the initial value.")]
		private string selected = "";

		[SerializeField,
			Tooltip("Text to be set when entering the focus state. An empty string will be set to the initial value.")]
		private string focused = "";

		[SerializeField,
			Tooltip("Text to be set when entering the activated state. An empty string will be set to the initial value.")]
		private string activated = "";

		[SerializeField,
			Tooltip("Text to be set when entering the disabled state. An empty string will be set to the initial value.")]
		private string disabled = "";

		private TextMeshPro targetTMP;

		private string initialText = "";

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			TextMeshPro tmp;
			if (target != null && target.TryGetComponent(out tmp))
			{
				targetTMP = tmp;
				initialText = targetTMP.text;
			}
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (targetTMP == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild Text Mesh Pro. Effect is not yet initialized.");
				return false;
			}
			return true;
		}

		protected override void OnChangeState(
			GameObject target,
			BaseVisualEffectsController controller,
			InteractableStates prevState,
			InteractableStates currentState)
		{
			switch(currentState)
			{
				case InteractableStates.Idle:
					targetTMP.text = idle != "" ? idle : initialText;
					break;
				case InteractableStates.Hover:
					targetTMP.text = hover != "" ? hover : initialText;
					break;
				case InteractableStates.Select:
					targetTMP.text = selected != "" ? selected : initialText;
					break;
				case InteractableStates.Focus:
					targetTMP.text = focused != "" ? focused : initialText;
					break;
				case InteractableStates.Activated:
					targetTMP.text = activated != "" ? activated : initialText;
					break;
				case InteractableStates.Disabled:
					targetTMP.text = disabled != "" ? disabled : initialText;
					break;
			}
		}
	}
}
