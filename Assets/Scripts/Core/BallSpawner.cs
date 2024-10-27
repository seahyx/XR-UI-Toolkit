using UnityEngine;
using static TicTacToe3D.Core.GameManager;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Spawns the ball on the player's turn.
	/// </summary>
	public class BallSpawner : MonoBehaviour
	{
		[SerializeField,
			Tooltip("Game manager reference.")]
		public GameManager gameManager;

		[SerializeField,
			Tooltip("Ball prefab to spawn.")]
		public Ball ballPrefab;

		private void Awake()
		{
			gameManager.OnPlayerChange.AddListener(OnPlayerChange);
		}

		private void OnPlayerChange(Players player)
		{
			if (player == Players.Player1)
			{
				SpawnBall();
			}
		}

		private void SpawnBall()
		{
			Ball ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
			ball.gameManager = gameManager;
			ball.Owner = Players.Player1;
		}
	}
}
