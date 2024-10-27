using TicTacToe3D.Core;
using UnityEngine;

namespace TicTacToe3D.Helpers
{
	/// <summary>
	/// Changes the active mode of the slot platform based on the slot state.
	/// </summary>
	public class SlotPlatformActivate : MonoBehaviour
	{
		[SerializeField]
		public Slot slot;

		private void Awake()
		{
			slot.OnSlotStateChanged.AddListener(OnSlotStateChanged);
		}

		private void OnSlotStateChanged(GameField.Index3D index, Slot.SlotState state)
		{
			switch (state)
			{
				case Slot.SlotState.Empty:
					gameObject.SetActive(false);
					break;
				case Slot.SlotState.Player1:
					gameObject.SetActive(true);
					break;
				case Slot.SlotState.Player2:
					gameObject.SetActive(true);
					break;
			}
		}
	}
}
