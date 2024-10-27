using UnityEngine;
using UnityEngine.Events;
using static TicTacToe3D.Core.GameField;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Slot component, represents a slot in the 3D Tic-Tac-Toe game field.
	/// </summary>
	public class Slot : MonoBehaviour
{
		public enum SlotState
		{
			Empty,
			Player1,
			Player2
		}

		[SerializeField,
			Tooltip("Invoked when the slot state has changed.")]
		public UnityEvent<Index3D, SlotState> OnSlotStateChanged = new();

		[SerializeField,
			Tooltip("Invoked when the slot hover state has changed.")]
		public UnityEvent<Index3D, SlotState> OnHoverStateChanged = new();

		/// <summary>
		/// Game field this slot belongs in.
		/// </summary>
		public GameField gameField { get; set; }

		/// <summary>
		/// Private backing field for the current state of this slot.
		/// </summary>
		private SlotState _state = SlotState.Empty;

		/// <summary>
		/// Current state of this slot.
		/// </summary>
		public SlotState State
		{
			get => _state;
			set
			{
				_state = value;
				OnSlotStateChanged.Invoke(index, _state);
			}
		}

		/// <summary>
		/// Private backing field for the hover state of this slot.
		/// </summary>
		private SlotState _hoverState = SlotState.Empty;

		/// <summary>
		/// The hover state of this slot.
		/// </summary>
		public SlotState HoverState
		{
			get => _hoverState;
			set
			{
				_hoverState = value;
				OnHoverStateChanged.Invoke(index, _hoverState);
			}
		}

		/// <summary>
		/// Index of this slot in the game field.
		/// </summary>
		public Index3D index { get; set; }

		public void ResetSlot()
		{
			State = SlotState.Empty;
		}
	}
}
