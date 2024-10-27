using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	[AddComponentMenu("XR UI Toolkit/Visual Effects/XR Visual Effects Controller")]
	public class XRVisualEffectsController : MonoBehaviour
	{
		[SerializeField, Tooltip("XR Base Interactable reference. If none, will set to the Interactable on this gameObject.")]
		private XRBaseInteractable interactable;


		private void Awake()
		{
			if (interactable == null)
			{
				interactable = GetComponent<XRBaseInteractable>();
			}
		}

		/// <summary>
		/// Initialize visual effects.
		/// </summary>
		void OnEnable()
		{
			foreach (XRVisualEffect visualEffect in GetComponents<XRVisualEffect>())
			{
				visualEffect.InitializeEffect(interactable);
			}
		}

		/// <summary>
		/// Reset visual effects if needed.
		/// </summary>
		void OnDisable()
		{
			foreach (XRVisualEffect visualEffect in GetComponents<XRVisualEffect>())
			{
				if (!visualEffect.ReinitializeOnDisable) return;
				visualEffect.InitializeEffect(interactable);
			}
		}
	}
}
