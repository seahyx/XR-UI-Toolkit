using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "ImageColourVFX", menuName = "HHUI Toolkit/Visual Effects/Image Colour VFX", order = 1)]
	public class ImageColourVisualEffect : BaseAnimatedVisualEffect
	{
		protected enum ColourBlendMode
		{
			Normal,
			Multiply,
			Addition,
			Subtraction
		}

		[SerializeField,
			Tooltip("Colour blending mode between the original image colour and the visual effect colour.")]
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

		private Image _targetImage;

		/// <summary>
		/// Target material on the GameObject.
		/// </summary>
		protected Image TargetImage
		{
			get { return _targetImage; }
			set
			{
				_targetImage = value;
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

			Image image;
			if (target != null && target.TryGetComponent(out image))
				TargetImage = image;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (TargetImage == null)
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
			switch (currentState)
			{
				case InteractableStates.Idle:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetImage.color,
						BlendColours(initialColor, idle, colourBlendMode)));
					break;

				case InteractableStates.Hover:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetImage.color,
						BlendColours(initialColor, hover, colourBlendMode)));
					break;

				case InteractableStates.Select:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetImage.color,
						BlendColours(initialColor, selected, colourBlendMode)));
					break;

				case InteractableStates.Focus:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetImage.color,
						BlendColours(initialColor, focused, colourBlendMode)));
					break;

				case InteractableStates.Activated:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetImage.color,
						BlendColours(initialColor, activated, colourBlendMode)));
					break;

				case InteractableStates.Disabled:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetImage.color,
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
			TargetImage.color = color;
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
