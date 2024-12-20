using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "SpriteSwapVFX", menuName = "HHUI Toolkit/Visual Effects/UI/Sprite Swap VFX", order = 0)]
	[DropdownMenuName("UI/Sprite Swap VFX")]
	public class SpriteSwapVisualEffect : BaseVisualEffect
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

		private SpriteRenderer targetSpriteRenderer;

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			SpriteRenderer tmp;
			if (target != null && target.TryGetComponent(out tmp))
				targetSpriteRenderer = tmp;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (targetSpriteRenderer == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild Sprite Renderer. Effect is not yet initialized.");
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
			targetSpriteRenderer.sprite = sprite;
		}
	}
}
