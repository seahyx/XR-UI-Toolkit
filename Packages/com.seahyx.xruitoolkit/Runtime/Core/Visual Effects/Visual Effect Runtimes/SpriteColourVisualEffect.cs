using System.Collections;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "SpriteColourVFX", menuName = "HHUI Toolkit/Visual Effects/Colour/Sprite Colour VFX", order = 1)]
	[DropdownMenuName("Colour/Sprite Colour VFX")]
	public class SpriteColourVisualEffect : BaseAnimatedVisualEffect
	{
		protected enum ColourBlendMode
		{
			Normal,
			Multiply,
			Addition,
			Subtraction
		}

		[SerializeField,
			Tooltip("Colour blending mode between the original sprite colour and the visual effect colour.")]
		protected ColourBlendMode colourBlendMode = ColourBlendMode.Normal;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_IDLE)]
		protected Color idle = Color.white;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_HOVER)]
		protected Color hover = Color.white;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_SELECT)]
		protected Color selected = Color.white;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_FOCUS)]
		protected Color focused = Color.white;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_ACTIVATED)]
		protected Color activated = Color.white;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_DISABLED)]
		protected Color disabled = Color.white;

		private SpriteRenderer _targetSprite;

		/// <summary>
		/// Target material on the GameObject.
		/// </summary>
		protected SpriteRenderer TargetSprite
		{
			get { return _targetSprite; }
			set
			{
				_targetSprite = value;
				initialColor = value.color;
			}
		}

		/// <summary>
		/// The initial color of the material.
		/// </summary>
		protected Color initialColor = Color.white;

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			SpriteRenderer renderer;
			if (target != null && target.TryGetComponent(out renderer))
				TargetSprite = renderer;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (TargetSprite == null)
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
			switch (currentState)
			{
				case InteractableStates.Idle:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetSprite.color,
						BlendColours(initialColor, idle, colourBlendMode)));
					break;

				case InteractableStates.Hover:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetSprite.color,
						BlendColours(initialColor, hover, colourBlendMode)));
					break;

				case InteractableStates.Select:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetSprite.color,
						BlendColours(initialColor, selected, colourBlendMode)));
					break;

				case InteractableStates.Focus:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetSprite.color,
						BlendColours(initialColor, focused, colourBlendMode)));
					break;

				case InteractableStates.Activated:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetSprite.color,
						BlendColours(initialColor, activated, colourBlendMode)));
					break;

				case InteractableStates.Disabled:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetSprite.color,
						BlendColours(initialColor, disabled, colourBlendMode)));
					break;
			}
		}

		protected IEnumerator TransitionAnimation(Color initial, Color final)
		{
			yield return null; // Initialization frame

			float elapsedTime = 0; // Time since start of transition
			float t = 0; // Transition value

			while (elapsedTime < transitionDuration)
			{
				elapsedTime += Time.deltaTime;

				t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
				SetColour(Color.LerpUnclamped(initial, final, t));

				yield return null; // Wait next frame
			}

			// End of transition, snap to final value
			SetColour(final);
		}

		protected void SetColour(Color color)
		{
			TargetSprite.color = color;
		}

		protected static Color BlendColours(Color a, Color b, ColourBlendMode blendMode)
		{
			switch (blendMode)
			{
				case ColourBlendMode.Normal:
					return b;

				case ColourBlendMode.Multiply:
					return a * b;

				case ColourBlendMode.Addition:
					return a + b;

				case ColourBlendMode.Subtraction:
					return a - b;

				default: return b;
			}
		}
	}
}
