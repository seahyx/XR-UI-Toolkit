using UnityEngine;
using UnityEngine.Events;

namespace XRUIToolkit.Core.VisualEffect
{
	public abstract class BaseVisualEffectsController : MonoBehaviour
	{
		#region Event Methods

		public UnityEvent FirstHoverEntered;
		public UnityEvent LastHoverExited;
		public UnityEvent FirstSelectEntered;
		public UnityEvent LastSelectExited;
		public UnityEvent FirstFocusEntered;
		public UnityEvent LastFocusExited;
		public UnityEvent Activated;
		public UnityEvent Deactivated;
		public UnityEvent ControllerEnabled;
		public UnityEvent ControllerDisabled;

		#endregion

		/// <summary>
		/// Initialize visual effects.
		/// </summary>
		protected void Start()
		{
			foreach (XRVisualEffect visualEffect in GetComponents<XRVisualEffect>())
			{
				visualEffect.InitializeEffect(this);
			}
		}

		/// <summary>
		/// Invoke controller enabled event.
		/// </summary>
		protected void OnEnable()
		{
			BindEvents();
			ControllerEnabled.Invoke();
		}

		/// <summary>
		/// Invoke controller disabled event.
		/// </summary>
		protected void OnDisable()
		{
			ControllerDisabled.Invoke();
		}

		/// <summary>
		/// Bind the interactable events to the event methods. Called in OnEnable.
		/// </summary>
		protected virtual void BindEvents() { }

		/// <summary>
		/// Unbind the interactable events from the event methods.
		/// </summary>
		protected virtual void UnbindEvents() { }
	}
}
