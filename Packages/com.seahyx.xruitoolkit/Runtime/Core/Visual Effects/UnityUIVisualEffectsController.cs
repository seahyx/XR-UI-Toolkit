using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XRUIToolkit.Core.VisualEffect
{
	[AddComponentMenu("XR UI Toolkit/Visual Effects/Unity UI Visual Effects Controller")]
	public class UnityUIVisualEffectsController : BaseVisualEffectsController, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		private Toggle toggle;

		protected override void BindEvents()
		{
			// If there is a toggle component on this GameObject, bind to it
			toggle = GetComponent<Toggle>();
			if (toggle != null)
			{
				// Clear any duplicate event bindings
				UnbindEvents();

				BindToggleEvents();
			}
		}

		protected override void UnbindEvents()
		{
			UnbindToggleEvents();
		}

		private void BindToggleEvents()
		{
			if (toggle == null)
				return;

			toggle.onValueChanged.AddListener(OnToggleValueChanged);
		}

		private void UnbindToggleEvents()
		{
			if (toggle == null)
				return;

			toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			FirstHoverEntered.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			LastHoverExited.Invoke();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			FirstSelectEntered.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			LastSelectExited.Invoke();
		}

		public void OnSelect(BaseEventData eventData)
		{
			FirstFocusEntered.Invoke();
		}

		public void OnDeselect(BaseEventData eventData)
		{
			LastFocusExited.Invoke();
		}

		private void OnToggleValueChanged(bool value)
		{
			if (value)
				Activated.Invoke();
			else
				Deactivated.Invoke();
		}
	}
}
