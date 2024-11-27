using UnityEngine;
using UnityEngine.UI;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "ImageSwapVFX", menuName = "HHUI Toolkit/Visual Effects/UI/Image Swap VFX", order = 0)]
	[DropdownMenuName("UI/Image Swap VFX")]
	public class ImageSwapVisualEffect : BaseVisualEffect
	{
		[SerializeField,
			Tooltip("Sprite to be set when entering the idle state.")]
		private Sprite idle;

		[SerializeField,
			Tooltip("Sprite to be set when entering the hover state.")]
		private Sprite hover;

		[SerializeField,
			Tooltip("Sprite to be set when entering the selected state.")]
		private Sprite selected;

		[SerializeField,
			Tooltip("Sprite to be set when entering the focus state.")]
		private Sprite focused;

		[SerializeField,
			Tooltip("Sprite to be set when entering the activated state.")]
		private Sprite activated;

		[SerializeField,
			Tooltip("Sprite to be set when entering the disabled state.")]
		private Sprite disabled;

		private Image targetImage;

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			Image tmp;
			if (target != null && target.TryGetComponent(out tmp))
				targetImage = tmp;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (targetImage == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild UGUI Image. Effect is not yet initialized.");
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
					SetSprite(idle);
					break;
				case InteractableStates.Hover:
					SetSprite(hover);
					break;
				case InteractableStates.Select:
					SetSprite(selected);
					break;
				case InteractableStates.Focus:
					SetSprite(focused);
					break;
				case InteractableStates.Activated:
					SetSprite(activated);
					break;
				case InteractableStates.Disabled:
					SetSprite(disabled);
					break;
			}
		}

		protected void SetSprite(Sprite sprite)
		{
			targetImage.sprite = sprite;
		}
	}
}
