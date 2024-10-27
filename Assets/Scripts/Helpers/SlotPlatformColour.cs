using TicTacToe3D.Core;
using UnityEngine;

namespace TicTacToe3D.Helpers
{
	/// <summary>
	/// Changes the colour of the slot platform based on the slot state.
	/// </summary>
	public class SlotPlatformColour : MonoBehaviour
	{
		[SerializeField]
		public Slot slot;
		[SerializeField]
		public Renderer slotRenderer;

		[SerializeField]
		public Color emptyColour = Color.white;

		private void Awake()
		{
			slot.OnSlotStateChanged.AddListener(OnSlotStateChanged);
			slot.OnHoverStateChanged.AddListener(OnHoverStateChanged);
			slotRenderer = GetComponent<Renderer>();
		}

		private void OnSlotStateChanged(GameField.Index3D index, Slot.SlotState state)
		{
			if (slotRenderer == null)
				return;
			switch (state)
			{
				case Slot.SlotState.Empty:
					slotRenderer.material.color = emptyColour;
					break;
				case Slot.SlotState.Player1:
					slotRenderer.material.color = slot.gameField.gameManager.player1Color;
					break;
				case Slot.SlotState.Player2:
					slotRenderer.material.color = slot.gameField.gameManager.player2Color;
					break;
			}
		}

		private void OnHoverStateChanged(GameField.Index3D index, Slot.SlotState state)
		{
			if (slotRenderer == null)
				return;
			// Slot state takes precedence over hover state
			if (slot.State != Slot.SlotState.Empty)
				return;
			switch (state)
			{
				case Slot.SlotState.Empty:
					slotRenderer.material.color = emptyColour;
					break;
				case Slot.SlotState.Player1:
					slotRenderer.material.color = slot.gameField.gameManager.player1Color;
					break;
				case Slot.SlotState.Player2:
					slotRenderer.material.color = slot.gameField.gameManager.player2Color;
					break;
			}
		}
	}
}
