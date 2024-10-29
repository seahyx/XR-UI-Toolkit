using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "ColourEffect", menuName = "HHUI Toolkit/Visual Effects/Colour Effect", order = 1)]
	public class ColourVisualEffect : BaseAnimatedVisualEffect
	{
		protected enum ColourBlendMode
		{
			Normal,
			Multiply,
			Addition,
			Subtraction
		}

		[SerializeField,
			Tooltip("Colour blending mode between the original material colour and the visual effect colour.")]
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

		private Material _targetMaterial;

		/// <summary>
		/// Target material on the GameObject.
		/// </summary>
		protected Material TargetMaterial
		{
			get { return _targetMaterial; }
			set
			{
				_targetMaterial = value;
				initialColor = value.color;
			}
		}

		/// <summary>
		/// The initial color of the material.
		/// </summary>
		protected Color initialColor = Color.white;

		public override void Initialize(XRBaseInteractable interactable, GameObject target)
		{
			base.Initialize(interactable, target);

			Renderer renderer;
			if (target != null && target.TryGetComponent(out renderer))
				TargetMaterial = renderer.material;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (TargetMaterial == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild material. Effect is not yet initialized.");
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
			switch (currentState)
			{
				case InteractableStates.Idle:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetMaterial.color,
						BlendColours(initialColor, idle, colourBlendMode)));
					break;

				case InteractableStates.Hover:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetMaterial.color,
						BlendColours(initialColor, hover, colourBlendMode)));
					break;

				case InteractableStates.Select:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetMaterial.color,
						BlendColours(initialColor, selected, colourBlendMode)));
					break;

				case InteractableStates.Focus:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetMaterial.color,
						BlendColours(initialColor, focused, colourBlendMode)));
					break;

				case InteractableStates.Activated:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetMaterial.color,
						BlendColours(initialColor, activated, colourBlendMode)));
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
				SetColour(Color.Lerp(initial, final, t));

				yield return null; // Wait next frame
			}

			// End of transition, snap to final value
			SetColour(final);
		}

		protected void SetColour(Color color)
		{
			TargetMaterial.color = color;
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
