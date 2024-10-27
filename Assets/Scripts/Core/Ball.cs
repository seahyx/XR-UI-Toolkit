using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using static TicTacToe3D.Core.GameField;
using static TicTacToe3D.Core.GameManager;

namespace TicTacToe3D.Core
{
	/// <summary>
	/// Ball component that the player can place on the game field.
	/// </summary>
	[RequireComponent(typeof(XRGrabInteractable))]
	public class Ball : MonoBehaviour
	{
		/// <summary>
		/// Keep track of all trigger colliders that this ball entered.
		/// </summary>
		private List<SlotBallDetector> detectors = new();

		/// <summary>
		/// Game manager reference.
		/// </summary>
		public GameManager gameManager;

		/// <summary>
		/// Private backing field for the owner of this ball.
		/// </summary>
		private Players _owner = Players.Player1;

		/// <summary>
		/// Owner of this ball.
		/// </summary>
		public Players Owner
		{
			get => _owner;
			set
			{
				_owner = value;
				Color color = Color.white;
				switch (_owner)
				{
					case Players.Player1:
						color = gameManager.player1Color;
						break;
					case Players.Player2:
						color = gameManager.player2Color;
						break;
				}
				GetComponent<Renderer>().material.color = color;
			}
		}

		private SlotBallDetector _nearestSlot;
		private SlotBallDetector NearestSlot
		{
			get => _nearestSlot;
			set
			{
				if (value == _nearestSlot)
					return;
				if (_nearestSlot != null)
					_nearestSlot.OnBallHoverExit();
				_nearestSlot = value;
				if (_nearestSlot != null)
					_nearestSlot.OnBallHoverEnter(Owner);
			}
		}

		private XRGrabInteractable grabInteractable;

		private void Awake()
		{
			grabInteractable = GetComponent<XRGrabInteractable>();
			grabInteractable.lastSelectExited.AddListener((SelectExitEventArgs e) =>  ReleaseBall());
		}

		public void OnDetectorEnter(SlotBallDetector other)
		{
			detectors.Add(other);
		}

		public void OnDetectorExit(SlotBallDetector other)
		{
			detectors.Remove(other);
		}

		/// <summary>
		/// Get the nearest slot to this ball.
		/// </summary>
		private void FixedUpdate()
		{
			// Find the nearest slot
			SlotBallDetector nearestSlot = null;
			float nearestDistance = float.MaxValue;
			foreach (var detector in detectors)
			{
				float distance = Vector3.Distance(transform.position, detector.transform.position);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearestSlot = detector;
				}
			}
			NearestSlot = nearestSlot;
		}

		public void ReleaseBall()
		{
			if (NearestSlot != null)
			{
				switch (Owner)
				{
					case Players.Player1:
						NearestSlot.slot.State = Slot.SlotState.Player1;
						break;
					case Players.Player2:
						NearestSlot.slot.State = Slot.SlotState.Player2;
						break;
				}

				Destroy(gameObject);
			}
		}

	}
}
