using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static TicTacToe3D.Core.GameField;
using static TicTacToe3D.Core.GameManager;
using static TicTacToe3D.Core.Slot;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Tic Tac Toe AI that randomly selects an empty slot to play.
	/// </summary>
	public class RandomAI : GameAI
	{
		[SerializeField,
			Range(0f, 10f),
			Tooltip("Time in seconds to think before making a move.")]
		public float thinkTime = 2f;

		public override void MakeMove(GameField field, Players player, UnityAction<Index3D> EndTurn)
		{
			StartCoroutine(Think(field, player, EndTurn));
		}

		private IEnumerator Think(GameField field, Players player, UnityAction<Index3D> EndTurn)
		{
			yield return new WaitForSeconds(thinkTime);
			List<int> slotIndexes = new();
			for (int x = 0; x < field.Slots.Count; x++)
			{
				if (field.Slots[x].State == SlotState.Empty)
				{
					slotIndexes.Add(x);
				}
			}
			int randomIndex = Random.Range(0, slotIndexes.Count);
			EndTurn(field.Slots[slotIndexes[randomIndex]].index);
		}
	}
}
