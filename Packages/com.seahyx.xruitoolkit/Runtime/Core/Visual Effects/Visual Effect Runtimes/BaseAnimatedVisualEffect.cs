using System.Collections;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	public abstract class BaseAnimatedVisualEffect : BaseVisualEffect
	{
		[SerializeField,
			Tooltip("The animation curve that determines how the effect will transition from one state to another.")]
		protected AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[SerializeField,
			Tooltip("Duration of the transition animation in seconds.")]
		protected float transitionDuration = 0.2f;

		/// <summary>
		/// Currently active running transition coroutine.
		/// </summary>
		private IEnumerator transCoroutine = null;

		/// <summary>
		/// Stops any currently running transition coroutine, if any, and starts the transition animation coroutine.
		/// </summary>
		protected void BeginTransitionCoroutine(IEnumerator transitionCoroutine)
		{
			if (transCoroutine != null)
				Controller.StopCoroutine(transCoroutine);
			transCoroutine = transitionCoroutine;
			if (!Controller.isActiveAndEnabled)
				return;
			Controller.StartCoroutine(transitionCoroutine);
		}
	}
}