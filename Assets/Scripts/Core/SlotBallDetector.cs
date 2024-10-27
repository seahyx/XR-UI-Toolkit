using UnityEngine;
using static TicTacToe3D.Core.GameManager;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Detects a ball to be placed in a slot.
	/// </summary>
	public class SlotBallDetector : MonoBehaviour
	{
		[SerializeField]
		public Slot slot;

		[SerializeField,
			Tooltip("Collider detectors for this slot.")]
		public Collider detectorCollider;

		private void Awake()
		{
			slot.OnSlotStateChanged.AddListener(OnSlotStateChanged);
		}

		private void OnSlotStateChanged(GameField.Index3D index, Slot.SlotState state)
		{
			detectorCollider.enabled = state == Slot.SlotState.Empty;
		}

		public void OnBallHoverEnter(Players owner)
		{
			switch (owner)
			{
				case Players.Player1:
					slot.HoverState = Slot.SlotState.Player1;
					break;
				case Players.Player2:
					slot.HoverState = Slot.SlotState.Player2;
					break;
			}
		}

		public void OnBallHoverExit()
		{
			slot.HoverState = Slot.SlotState.Empty;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Ball ball))
			{
				ball.OnDetectorEnter(this);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Ball ball))
			{
				ball.OnDetectorExit(this);
			}
		}
	}
}
