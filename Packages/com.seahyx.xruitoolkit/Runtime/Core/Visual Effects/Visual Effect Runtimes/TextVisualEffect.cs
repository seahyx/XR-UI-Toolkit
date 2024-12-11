using TMPro;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "TextVFX", menuName = "HHUI Toolkit/Visual Effects/UI/Text VFX", order = 0)]
	[DropdownMenuName("UI/Text VFX")]
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
		private TextMeshProUGUI targetTMPUGUI;

		private string initialText = "";

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			if (target != null && target.TryGetComponent(out TextMeshPro tmp))
			{
				targetTMP = tmp;
				initialText = targetTMP.text;
			} else if (target != null && target.TryGetComponent(out TextMeshProUGUI tmpugui))
			{
				targetTMPUGUI = tmpugui;
				initialText = targetTMP.text;
			}
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (targetTMP == null && targetTMPUGUI == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild TextMeshPro or TextMeshProUGUI. Effect is not yet initialized.");
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
					SetText(idle != "" ? idle : initialText);
					break;
				case InteractableStates.Hover:
					SetText(hover != "" ? hover : initialText);
					break;
				case InteractableStates.Select:
					SetText(selected != "" ? selected : initialText);
					break;
				case InteractableStates.Focus:
					SetText(focused != "" ? focused : initialText);
					break;
				case InteractableStates.Activated:
					SetText(activated != "" ? activated : initialText);
					break;
				case InteractableStates.Disabled:
					SetText(disabled != "" ? disabled : initialText);
					break;
			}
		}

		protected void SetText(string text) {
			if (targetTMP != null) targetTMP.text = text;
			if (targetTMPUGUI != null) targetTMPUGUI.text = text;
		}
	}
}
