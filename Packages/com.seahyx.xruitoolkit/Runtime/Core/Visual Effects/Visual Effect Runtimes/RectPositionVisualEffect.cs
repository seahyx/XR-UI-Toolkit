using System.Collections;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "RectPositionEffect", menuName = "HHUI Toolkit/Visual Effects/RectTransform Position Effect", order = 2)]
	public class RectPositionVisualEffect : BaseAnimatedVisualEffect
	{
		protected enum TransformMode
		{
			Absolute,
			Relative,
		}

		[SerializeField,
			Tooltip("How this visual effect will apply its transformations to the target object.\n\n" +
			"Absolute: Directly sets the position to the effect values.\n" +
			"Relative: Adds the effect values to the original position of the target object.")]
		protected TransformMode transformMode = TransformMode.Relative;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_IDLE)]
		protected Vector3 idle = Vector3.zero;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_HOVER)]
		protected Vector3 hover = Vector3.zero;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_SELECT)]
		protected Vector3 selected = Vector3.zero;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_FOCUS)]
		protected Vector3 focused = Vector3.zero;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_ACTIVATED)]
		protected Vector3 activated = Vector3.zero;

		[SerializeField,
			Tooltip(TOOLTIP_STATE_DISABLED)]
		protected Vector3 disabled = Vector3.zero;

		private RectTransform _targetRectTransform;

		/// <summary>
		/// Target transform on the GameObject.
		/// </summary>
		protected RectTransform TargetRectTransform
		{
			get { return _targetRectTransform; }
			set
			{
				_targetRectTransform = value;
				initialPosition = value.anchoredPosition3D;
			}
		}

		/// <summary>
		/// The initial position of the material.
		/// </summary>
		protected Vector3 initialPosition = Vector3.zero;

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			if (target != null)
				TargetRectTransform = target.GetComponent<RectTransform>();
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (TargetRectTransform == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild RectTransform. Effect is not yet initialized.");
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
						TargetRectTransform.anchoredPosition3D,
						CalculatePosition(initialPosition, idle, transformMode)));
					break;

				case InteractableStates.Hover:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetRectTransform.anchoredPosition3D,
						CalculatePosition(initialPosition, hover, transformMode)));
					break;

				case InteractableStates.Select:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetRectTransform.anchoredPosition3D,
						CalculatePosition(initialPosition, selected, transformMode)));
					break;

				case InteractableStates.Focus:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetRectTransform.anchoredPosition3D,
						CalculatePosition(initialPosition, focused, transformMode)));
					break;

				case InteractableStates.Activated:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetRectTransform.anchoredPosition3D,
						CalculatePosition(initialPosition, activated, transformMode)));
					break;

				case InteractableStates.Disabled:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetRectTransform.anchoredPosition3D,
						CalculatePosition(initialPosition, disabled, transformMode)));
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
				SetPosition(Vector3.Lerp(initial, final, t));

				yield return null; // Wait next frame
			}

			// End of transition, snap to final value
			SetPosition(final);
		}

		protected void SetPosition(Vector3 position)
		{
			TargetRectTransform.anchoredPosition3D = position;
		}

		protected static Vector3 CalculatePosition(Vector3 a, Vector3 b, TransformMode mode)
		{
			switch (mode)
			{
				case TransformMode.Absolute:
					return b;

				case TransformMode.Relative:
					return a + b;

				default: return b;
			}
		}
	}
}
