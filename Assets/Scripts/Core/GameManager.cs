using UnityEngine;
using UnityEngine.Events;
using static TicTacToe3D.Core.GameField;
using static TicTacToe3D.Core.Slot;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Game manager component, manages the game state and rules.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		private enum GameState
		{
			Playing,
			Player1Win,
			Player2Win,
			Draw
		}

		public enum Players
		{
			Player1,
			Player2
		}

		[SerializeField,
			Tooltip("Game field component.")]
		private GameField gameField;

		[SerializeReference,
			Tooltip("Player 2 AI.")]
		private GameAI player2AI;

		[SerializeField,
			Tooltip("Player 1 score.")]
		public int player1Score = 0;

		[SerializeField,
			Tooltip("Player 2 score.")]
		public int player2Score = 0;

		[SerializeField,
			Tooltip("Player 1 ball color.")]
		public Color player1Color = Color.red;

		[SerializeField,
			Tooltip("Player 2 ball color.")]
		public Color player2Color = Color.blue;

		[SerializeField,
			Tooltip("Invoked when the game is playing.")]
		public UnityEvent OnPlaying = new();

		[SerializeField,
			Tooltip("Invoked when player 1 wins.")]
		public UnityEvent OnPlayer1Win = new();

		[SerializeField,
			Tooltip("Invoked when player 2 wins.")]
		public UnityEvent OnPlayer2Win = new();

		[SerializeField,
			Tooltip("Invoked when the game is a draw.")]
		public UnityEvent OnDraw = new();

		[SerializeField,
			Tooltip("Invoked when the game is reset.")]
		public UnityEvent OnReset = new();

		[SerializeField,
			Tooltip("Invoked when the current player changes.")]
		public UnityEvent<Players> OnPlayerChange = new();


		/// <summary>
		/// Private backing field for the current game state.
		/// </summary>
		private GameState _gameState = GameState.Playing;

		/// <summary>
		/// The current game state.
		/// </summary>
		private GameState gameState
		{
			get => _gameState;
			set
			{
				_gameState = value;
				switch (value)
				{
					case GameState.Playing:
						OnPlaying.Invoke();
						Debug.Log("Playing!");
						break;
					case GameState.Player1Win:
						OnPlayer1Win.Invoke();
						Debug.Log("Player 1 wins!");
						break;
					case GameState.Player2Win:
						OnPlayer2Win.Invoke();
						Debug.Log("Player 2 wins!");
						break;
					case GameState.Draw:
						OnDraw.Invoke();
						Debug.Log("Draw!");
						break;
				}
			}
		}

		/// <summary>
		/// Private backing field for the current player.
		/// </summary>
		private Players _player = Players.Player1;

		/// <summary>
		/// The current player turn.
		/// </summary>
		private Players currentPlayer
		{
			get => _player;
			set
			{
				_player = value;
				OnPlayerChange.Invoke(value);
				
				switch (value)
				{
					case Players.Player1:
						Debug.Log("Player 1's turn!");
						break;
					case Players.Player2:
						Debug.Log("Player 2's turn!");
						player2AI.MakeMove(gameField, Players.Player2, (Index3D index) =>
						{
							if (gameField.IndexedSlots.TryGetValue(index, out Slot slot))
							{
								if (slot.State == SlotState.Empty)
									slot.State = SlotState.Player2;
							}
						});
						break;
				}
			}
		}

		/// <summary>
		/// Initialize the game field.
		/// </summary>
		private void Awake()
		{
			gameField.gameManager = this;
			gameField.Initialize();

			foreach (Slot slot in gameField.Slots)
			{
				slot.OnSlotStateChanged.AddListener(CheckField);
			}

			StartGame();
		}

		public void StartGame()
		{
			ResetGame();
			currentPlayer = Players.Player1;
			gameState = GameState.Playing;
		}

		/// <summary>
		/// Reset the game field.
		/// </summary>
		public void ResetGame()
		{
			gameField.ResetField();
			player1Score = 0;
			player2Score = 0;
		}


		/// <summary>
		/// Check if the game has been won.
		/// </summary>
		/// <param name="index">Index of the last changed slot.</param>
		/// <param name="state">State of the last changed slot.</param>
		private void CheckField(Index3D index, SlotState state)
		{
			if (state == SlotState.Empty)
				return;
			if (state == SlotState.Player1)
			{
				player1Score += gameField.CheckWin(index, SlotState.Player1);
				currentPlayer = Players.Player2;
			}
			else if (state == SlotState.Player2)
			{
				player2Score += gameField.CheckWin(index, SlotState.Player2);
				currentPlayer = Players.Player1;
			}
			if (gameField.CheckFull())
			{
				// Check final winner
				if (player1Score > player2Score)
				{
					gameState = GameState.Player1Win;
				}
				else if (player1Score < player2Score)
				{
					gameState = GameState.Player2Win;
				}
				else
				{
					gameState = GameState.Draw;
				}
			}
		}
	}
}