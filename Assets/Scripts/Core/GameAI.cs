using UnityEngine;
using UnityEngine.Events;
using static TicTacToe3D.Core.GameField;
using static TicTacToe3D.Core.GameManager;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Tic Tac Toe AI interface.
	/// </summary>
	public abstract class GameAI : MonoBehaviour
	{
		/// <summary>
		/// Make a move on the game field. EndTurn must be called after the move is made.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="player"></param>
		public abstract void MakeMove(GameField field, Players player, UnityAction<Index3D> EndTurn);
	}
}
