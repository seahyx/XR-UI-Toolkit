using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	[AddComponentMenu("XR UI Toolkit/Visual Effects/XR Interactable Visual Effects Controller")]
	public class XRInteractableVisualEffectsController : BaseVisualEffectsController
	{
		[SerializeField, Tooltip("XR Base Interactable reference. If none, will set to the Interactable on this gameObject.")]
		public XRBaseInteractable Interactable;

		protected override void BindEvents()
		{
			if (Interactable == null)
				Interactable = GetComponent<XRBaseInteractable>();
			if (Interactable == null)
			{
				enabled = false;
				return;
			}

			// Clear any duplicate event bindings
			UnbindEvents();

			Interactable.firstHoverEntered.AddListener((e) => FirstHoverEntered.Invoke());
			Interactable.lastHoverExited.AddListener((e) => LastHoverExited.Invoke());
			Interactable.firstSelectEntered.AddListener((e) => FirstSelectEntered.Invoke());
			Interactable.lastSelectExited.AddListener((e) => LastSelectExited.Invoke());
			Interactable.firstFocusEntered.AddListener((e) => FirstFocusEntered.Invoke());
			Interactable.lastFocusExited.AddListener((e) => LastFocusExited.Invoke());
			Interactable.activated.AddListener((e) => Activated.Invoke());
			Interactable.deactivated.AddListener((e) => Deactivated.Invoke());
		}

		protected override void UnbindEvents()
		{
			if (Interactable == null)
				return;

			Interactable.firstHoverEntered.RemoveListener((e) => FirstHoverEntered.Invoke());
			Interactable.lastHoverExited.RemoveListener((e) => LastHoverExited.Invoke());
			Interactable.firstSelectEntered.RemoveListener((e) => FirstSelectEntered.Invoke());
			Interactable.lastSelectExited.RemoveListener((e) => LastSelectExited.Invoke());
			Interactable.firstFocusEntered.RemoveListener((e) => FirstFocusEntered.Invoke());
			Interactable.lastFocusExited.RemoveListener((e) => LastFocusExited.Invoke());
			Interactable.activated.RemoveListener((e) => Activated.Invoke());
			Interactable.deactivated.RemoveListener((e) => Deactivated.Invoke());
		}
	}
}
