using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XRUIToolkit.Core.VisualEffect
{
	[AddComponentMenu("XR UI Toolkit/Visual Effects/Unity UI Visual Effects Controller")]
	public class UnityUIVisualEffectsController : BaseVisualEffectsController, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		protected override void BindEvents()
		{
			return;
		}

		protected override void UnbindEvents()
		{
			return;
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
	}
}
