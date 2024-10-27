using System.Collections.Generic;
using UnityEngine;
using static TicTacToe3D.Core.Slot;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Generates a 3x3x3 grid (without center) field of slots for a 3D Tic-Tac-Toe game.
	/// </summary>
	public class GameField : MonoBehaviour
	{
		/// <summary>
		/// 3-dimensional integer index value.
		/// </summary>
		public struct Index3D
		{
			public int x;
			public int y;
			public int z;
		}

		[SerializeField,
			Tooltip("TTT slot prefab.")]
		private Slot slotPrefab;

		[SerializeField,
			Tooltip("Spacing in meters between each slot.")]
		private float slotSpacing = 0.2f;

		/// <summary>
		/// Game manager reference.
		/// </summary>
		public GameManager gameManager { get; set; }

		/// <summary>
		/// Indices list of the game field slots.
		/// </summary>
		public List<Index3D> Indices { get; private set; } = GenerateIndices();

		/// <summary>
		/// List of all instantiated slots in the game field.
		/// </summary>
		public List<Slot> Slots { get; private set; } = new();

		/// <summary>
		/// Dictionary of all slots in the game field indexed by their 3D index.
		/// </summary>
		public Dictionary<Index3D, Slot> IndexedSlots { get; private set; } = new();

		/// <summary>
		/// Generate all 26 indices of the game field.
		/// </summary>
		/// <returns></returns>
		private static List<Index3D> GenerateIndices()
		{
			List<Index3D> indices = new();
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					for (int z = -1; z <= 1; z++)
					{
						if (x == 0 && y == 0 && z == 0)
							continue;
						indices.Add(new Index3D { x = x, y = y, z = z });
					}
				}
			}
			return indices;
		}

		/// <summary>
		/// Calculate all positions of the game field.
		/// </summary>
		/// <param name="indices">Slot indices of the game field.</param>
		/// <returns>All position vectors of the slots in the same order as <paramref name="indices"/>.</returns>
		private static List<Vector3> CalculatePositions(List<Index3D> indices, float spacing)
		{
			List<Vector3> positions = new();
			for (int i = 0; i < indices.Count; i++)
			{
				Index3D index = indices[i];
				positions.Add(new Vector3(index.x, index.y, index.z) * spacing);
			}
			return positions;
		}

		private void OnDrawGizmos()
		{
			List<Index3D> indices = GenerateIndices();
			List<Vector3> positions = CalculatePositions(indices, slotSpacing);

			Gizmos.color = Color.green;
			foreach (Vector3 position in positions)
			{
				Gizmos.DrawWireSphere(transform.rotation * (position + transform.position), 0.1f);
			}
		}

		/// <summary>
		/// Generate the game field.
		/// </summary>
		public void Initialize()
		{
			List<Index3D> indices = GenerateIndices();
			List<Vector3> positions = CalculatePositions(indices, slotSpacing);

			for (int i = 0; i < indices.Count; i++)
			{
				Slot slot = Instantiate(slotPrefab, transform);
				slot.transform.localPosition = positions[i];
				slot.index = indices[i];
				slot.gameField = this;
				slot.State = SlotState.Empty;
				Slots.Add(slot);
				IndexedSlots.Add(indices[i], slot);
			}
		}

		public void ResetField()
		{
			foreach (Slot slot in Slots)
			{
				slot.ResetSlot();
			}
		}

		/// <summary>
		/// Check if the newly placed slot has any matching slots in the same row, column, or diagonal.
		/// </summary>
		/// <param name="index">Index of the newly placed slot.</param>
		/// <param name="state">State of the newly placed slot.</param>
		/// <returns>Score earned from the newly placed slot.</returns>
		public int CheckWin(Index3D index, SlotState state)
		{
			if (state == SlotState.Empty)
				return 0;

			// Check all 13 directions for a win.
			int score = 0;
			foreach (Index3D direction in Indices)
			{
				if (CheckDirection(index, state, direction))
					score++;
			}
			// Since we check twice for each direction, divide the score by 2.
			score /= 2;

			return score;
		}

		/// <summary>
		/// Check if the slot at the given index has a win in the given direction.
		/// </summary>
		/// <param name="index">Slot index.</param>
		/// <param name="state">Slot state.</param>
		/// <param name="direction">Direction to check.</param>
		/// <returns>True if a line is formed, false otherwise.</returns>
		private bool CheckDirection(Index3D index, SlotState state, Index3D direction)
		{
			Index3D current = index;
			bool reversed = false;
			for (int i = 0; i < 2; i++)
			{
				if (reversed)
				{
					current.x -= direction.x;
					current.y -= direction.y;
					current.z -= direction.z;
				}
				else
				{
					current.x += direction.x;
					current.y += direction.y;
					current.z += direction.z;
				}
				// Check for out-of-bounds indices, reverse the direction if necessary.
				if (current.x < -1 || current.x > 1 ||
					current.y < -1 || current.y > 1 ||
					current.z < -1 || current.z > 1)
				{
					if (reversed) // If already reversed once, the 2nd time is invalid.
						return false;

					current = index;
					current.x -= direction.x;
					current.y -= direction.y;
					current.z -= direction.z;
					reversed = true;
				}

				if (IndexedSlots.TryGetValue(current, out Slot slot))
				{
					if (slot.State != state)
						return false;
				}
				else
					return false; // Center slot is always empty.
			}
			return true;
		}

		/// <summary>
		/// Check if the game field is full.
		/// </summary>
		/// <returns>True the game field is full, false otherwise.</returns>
		public bool CheckFull()
		{
			foreach (Slot slot in Slots)
			{
				if (slot.State == SlotState.Empty)
					return false;
			}
			return true;
		}

		
	}
}