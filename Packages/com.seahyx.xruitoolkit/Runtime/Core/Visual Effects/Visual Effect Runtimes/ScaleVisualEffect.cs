using System.Collections;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "ScaleVFX", menuName = "HHUI Toolkit/Visual Effects/Transform/Scale VFX", order = 2)]
	[DropdownMenuName("Transform/Scale VFX")]
	public class ScaleVisualEffect : BaseAnimatedVisualEffect
	{
		protected enum TransformMode
		{
			Absolute,
			Relative,
		}

		[SerializeField,
			Tooltip("How this visual effect will apply its transformations to the target object.\n\n" +
			"Absolute: Directly sets the scale to the effect values.\n" +
			"Relative: Multiplies the effect values to the original scale of the target object.")]
		protected TransformMode transformMode = TransformMode.Relative;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_IDLE)]
		protected Vector3 idle = Vector3.one;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_HOVER)]
		protected Vector3 hover = Vector3.one;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_SELECT)]
		protected Vector3 selected = Vector3.one;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_FOCUS)]
		protected Vector3 focused = Vector3.one;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_ACTIVATED)]
		protected Vector3 activated = Vector3.one;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_DISABLED)]
		protected Vector3 disabled = Vector3.one;

		private Transform _targetTransform;

		/// <summary>
		/// Target transform on the GameObject.
		/// </summary>
		protected Transform TargetTransform
		{
			get { return _targetTransform; }
			set
			{
				_targetTransform = value;
				initialScale = value.localScale;
			}
		}

		/// <summary>
		/// The initial scale of the material.
		/// </summary>
		protected Vector3 initialScale = Vector3.zero;

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			if (target != null)
				TargetTransform = target.transform;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (TargetTransform == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild Transform. Effect is not yet initialized.");
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
						TargetTransform.localScale,
						CalculateScale(initialScale, idle, transformMode)));
					break;

				case InteractableStates.Hover:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.localScale,
						CalculateScale(initialScale, hover, transformMode)));
					break;

				case InteractableStates.Select:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.localScale,
						CalculateScale(initialScale, selected, transformMode)));
					break;

				case InteractableStates.Focus:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.localScale,
						CalculateScale(initialScale, focused, transformMode)));
					break;

				case InteractableStates.Activated:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.localScale,
						CalculateScale(initialScale, activated, transformMode)));
					break;

				case InteractableStates.Disabled:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.localScale,
						CalculateScale(initialScale, disabled, transformMode)));
					break;
			}
		}

		protected IEnumerator TransitionAnimation(Vector3 initial, Vector3 final)
		{
			yield return null; // Initialization frame

			float elapsedTime = 0; // Time since start of transition

			while (elapsedTime < transitionDuration)
			{
				elapsedTime += Time.deltaTime; // Elapse time

				float t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
				SetScale(Vector3.LerpUnclamped(initial, final, t));

				yield return null; // Wait next frame
			}

			// End of transition, snap to final value
			SetScale(final);
		}

		protected void SetScale(Vector3 scale)
		{
			TargetTransform.localScale = scale;
		}

		protected static Vector3 CalculateScale(Vector3 a, Vector3 b, TransformMode mode)
		{
			switch (mode)
			{
				case TransformMode.Absolute:
					return b;

				case TransformMode.Relative:
					return Vector3.Scale(a, b);

				default: return b;
			}
		}
	}
}
