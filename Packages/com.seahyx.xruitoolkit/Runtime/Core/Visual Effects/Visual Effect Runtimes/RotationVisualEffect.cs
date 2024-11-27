using System.Collections;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "RotationVFX", menuName = "HHUI Toolkit/Visual Effects/Rotation VFX", order = 3)]
	public class RotationVisualEffect : BaseAnimatedVisualEffect
	{
		protected enum TransformMode
		{
			Absolute,
			Relative,
		}

		[SerializeField,
			Tooltip("How this visual effect will apply its transformations to the target object.\n\n" +
			"Absolute: Directly sets the rotation to the effect values.\n" +
			"Relative: Adds the effect values to the original rotation of the target object.")]
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
				initialRotation = value.rotation;
			}
		}

		/// <summary>
		/// The initial rotation of the material.
		/// </summary>
		protected Quaternion initialRotation = Quaternion.identity;

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
						TargetTransform.rotation,
						CalculateRotation(initialRotation, idle, transformMode)));
					break;

				case InteractableStates.Hover:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.rotation,
						CalculateRotation(initialRotation, hover, transformMode)));
					break;

				case InteractableStates.Select:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.rotation,
						CalculateRotation(initialRotation, selected, transformMode)));
					break;

				case InteractableStates.Focus:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.rotation,
						CalculateRotation(initialRotation, focused, transformMode)));
					break;

				case InteractableStates.Activated:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.rotation,
						CalculateRotation(initialRotation, activated, transformMode)));
					break;

				case InteractableStates.Disabled:
					BeginTransitionCoroutine(TransitionAnimation(
						TargetTransform.rotation,
						CalculateRotation(initialRotation, disabled, transformMode)));
					break;
			}
		}

		protected IEnumerator TransitionAnimation(Quaternion initial, Quaternion final)
		{
			yield return null; // Initialization frame

			float elapsedTime = 0; // Time since start of transition

			while (elapsedTime < transitionDuration)
			{
				elapsedTime += Time.deltaTime; // Elapse time

				float t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
				SetRotation(Quaternion.SlerpUnclamped(initial, final, t));

				yield return null; // Wait next frame
			}

			// End of transition, snap to final value
			SetRotation(final);
		}

		protected void SetRotation(Quaternion rotation)
		{
			TargetTransform.rotation = rotation;
		}

		protected static Quaternion CalculateRotation(Quaternion a, Vector3 b, TransformMode mode)
		{
			Quaternion qb = Quaternion.Euler(b);
			switch (mode)
			{
				case TransformMode.Absolute:
					return qb;

				case TransformMode.Relative:
					return a * qb;

				default: return qb;
			}
		}
	}
}
