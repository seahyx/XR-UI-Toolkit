using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace XRUIToolkit.Core.VisualEffect
{
	/// <summary>
	/// Base abstract class for a scriptable object visual effect for interactables. Extend this class to provide additional functionalities to the visual effect.
	/// </summary>
	public abstract class BaseVisualEffect : ScriptableObject
	{
		#region Type Definitions and Constants

		public const string TOOLTIP_STATE_PRIORITIES = "Adjust or enable/disable effect interactions with the interactable states.\n\nPriorities for each interactable state is rank top to bottom, from highest to lowest priority, repectively.\n\nEffects from a higher priority state will overwrite the effects from a lower priority state when both are active at the same time. A disabled effect state will not trigger any effects.";
		public const string TOOLTIP_STATE_IDLE = "The default state when none of the other states are active.";
		public const string TOOLTIP_STATE_HOVER = "The state when there is an interactor hovering over this interactable. Similar to a mouse-over state.";
		public const string TOOLTIP_STATE_SELECT = "The state when an interactor is selecting/clicking on this interactable. Similar to a mouse-down state.";
		public const string TOOLTIP_STATE_FOCUS = "An Interactable is focused when it is selected. This focus persists until another Interactable is selected or the Interactable explicitly attempts to select nothing.";
		public const string TOOLTIP_STATE_ACTIVATED = "Activation is an extra action, typically mapped to a button or trigger that affects the currently selected object. This lets the user further interact with an object they've selected.";
		public const string TOOLTIP_STATE_DISABLED = "Disabled state is when the visual effect controller is disabled or invalid.";

		public const string TOOLTIP_DEACTIVATE_ON_DESELECT = "When the interactable is still in the activated state when it exits the selected state, the visual effect will still be stuck in the activated state, until an interactor re-selects the interactable and activates the visual effect again.\nThis flag will allow this visual effect to exit the activated state when the interactable is deselected.";

		#endregion

		[SerializeField, Tooltip(TOOLTIP_STATE_PRIORITIES)]
		protected List<StatePriorityItem> statePriorities = new()
		{
			new StatePriorityItem(true, InteractableStates.Disabled),
			new StatePriorityItem(true, InteractableStates.Activated),
			new StatePriorityItem(true, InteractableStates.Select),
			new StatePriorityItem(true, InteractableStates.Hover),
			new StatePriorityItem(true, InteractableStates.Focus),
			new StatePriorityItem(true, InteractableStates.Idle),
		};

		/// <summary>
		/// Default state priorities list.
		/// </summary>
		private static ReadOnlyCollection<StatePriorityItem> defaultStatePriorities = new
		(
			new[] {
				new StatePriorityItem(true, InteractableStates.Disabled),
				new StatePriorityItem(true, InteractableStates.Activated),
				new StatePriorityItem(true, InteractableStates.Select),
				new StatePriorityItem(true, InteractableStates.Hover),
				new StatePriorityItem(true, InteractableStates.Focus),
				new StatePriorityItem(true, InteractableStates.Idle),
			}
		);

		[SerializeField, Tooltip(TOOLTIP_DEACTIVATE_ON_DESELECT)]
		protected bool deactivateOnDeselect = true;

		/// <summary>
		/// The <see cref="GameObject"/> that this effect will apply on.
		/// </summary>
		public GameObject Target { get; private set; }

		/// <summary>
		/// The <see cref="BaseVisualEffectsController">controller</see> instance that this effect is attached to.
		/// </summary>
		public BaseVisualEffectsController Controller { get; private set; }

		/// <summary>
		/// Current states.
		/// </summary>
		private InteractableStates currStates = InteractableStates.Idle;

		/// <summary>
		/// Previous states.
		/// </summary>
		private InteractableStates prevStates = InteractableStates.Idle;

		/// <summary>
		/// The currently active states of this <see cref="XRBaseInteractable">interactable</see>.
		/// Whenever this value is changed, it will check if the active state has changed and invoke the event
		/// callback if it has.
		/// </summary>
		protected InteractableStates CurrentStates
		{
			get => currStates;
			set
			{
				prevStates = currStates;
				currStates = value;

				// Check if the currently active state has changed
				if (GetHighestPriorityActiveState(prevStates, statePriorities) != GetHighestPriorityActiveState(currStates, statePriorities))
				{
					// Active state has changed, update previous active state and new current active state
					PreviousActiveState = CurrentActiveState;
					CurrentActiveState = GetHighestPriorityActiveState(currStates, statePriorities);

					// Validation checks
					if (!CheckInitialization()) return;

					// Callbacks
					OnChangeState(Target, Controller, PreviousActiveState, CurrentActiveState);
				}
			}
		}

		/// <summary>
		/// The currently highest priority state.
		/// The currently active state can only be a single state, while currentStates represent the status of all
		/// the states on this interactable.
		/// Defaults to <see cref="InteractableStates.Idle"/> if no state priorities are enabled.
		/// </summary>
		public InteractableStates CurrentActiveState { get; private set; }

		/// <summary>
		/// The previously highest priority state.
		/// The previously active state can only be a single state, while currentStates represent the status of all
		/// the states on this interactable.
		/// Defaults to <see cref="InteractableStates.Idle"/> if no state priorities are enabled.
		/// </summary>
		public InteractableStates PreviousActiveState { get; private set; }

		#region Initialization Methods

		/// <summary>
		/// Initialization method for this effect. This methods must be called at least once
		/// before this effect is being used.
		/// <see cref="CheckInitialization"/> will verify
		/// whether this effect is initialized on every event call, stopping the effect
		/// from executing if not initialized.
		/// <br/><br/>
		/// Extend this method to add additional intialization functionality.
		/// </summary>
		/// <param name="controller">The <see cref="XRBaseInteractable">interactable</see> instance that this effect is attached to.</param>
		/// <param name="target">The <see cref="GameObject"/> that this effect will apply on.</param>
		public virtual void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			Controller = controller;
			Target = target;

			BindEventMethods(controller);
		}

		/// <summary>
		/// Checks whether the effect has been properly initialized and ready to be used.
		/// <br/><br/>
		/// Extend this method to add additional checks and verifications if needed.
		/// </summary>
		/// <returns>Whether this effect passed the initialization checks.</returns>
		protected virtual bool CheckInitialization()
		{
			if (Controller == null)
			{
				PrintInitWarning("Controller is null. Effect is not yet initialized.");
				return false;
			}
			if (Target == null)
			{
				PrintInitWarning("Target GameObject is null. Effect is not yet initialized.");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Binds the event methods on this effect to the interactable event callbacks.
		/// It will first remove existing callbacks on the interactable before binding to prevent duplicate callbacks.
		/// </summary>
		/// <param name="controller">The <see cref="XRBaseInteractable">interactable</see> instance that this effect is attached to.</param>
		private void BindEventMethods(BaseVisualEffectsController controller)
		{
			if (controller == null) return;

			// Remove listeners to prevent duplicate callbacks
			UnbindEventMethods(controller);

			// Add listeners
			controller.FirstHoverEntered.AddListener(FirstHoverEntered);
			controller.LastHoverExited.AddListener(LastHoverExited);
			controller.FirstSelectEntered.AddListener(FirstSelectEntered);
			controller.LastSelectExited.AddListener(LastSelectExited);
			controller.FirstFocusEntered.AddListener(FirstFocusEntered);
			controller.LastFocusExited.AddListener(LastFocusExited);
			controller.Activated.AddListener(Activated);
			controller.Deactivated.AddListener(Deactivated);
			controller.ControllerEnabled.AddListener(ControllerEnabled);
			controller.ControllerDisabled.AddListener(ControllerDisabled);
		}

		/// <summary>
		/// Unbinds the event methods on this effect from the interactable event callbacks.
		/// </summary>
		/// <param name="controller"></param>
		private void UnbindEventMethods(BaseVisualEffectsController controller)
		{
			if (controller == null) return;

			// Remove listeners
			controller.FirstHoverEntered.RemoveListener(FirstHoverEntered);
			controller.LastHoverExited.RemoveListener(LastHoverExited);
			controller.FirstSelectEntered.RemoveListener(FirstSelectEntered);
			controller.LastSelectExited.RemoveListener(LastSelectExited);
			controller.FirstFocusEntered.RemoveListener(FirstFocusEntered);
			controller.LastFocusExited.RemoveListener(LastFocusExited);
			controller.Activated.RemoveListener(Activated);
			controller.Deactivated.RemoveListener(Deactivated);
			controller.ControllerEnabled.RemoveListener(ControllerEnabled);
			controller.ControllerDisabled.RemoveListener(ControllerDisabled);
		}

		#endregion

		#region Lifecycle Methods

		/// <summary>
		/// Disable this effect. Sets the visual effect to idle state.
		/// </summary>
		public void Disable()
		{
			// Enables disabled state
			CurrentStates |= InteractableStates.EffectDisabled;

			// Invoke callbacks for further cleanup
			OnEffectDisable();
		}

		/// <summary>
		/// Enables this effect.
		/// </summary>
		public void Enable()
		{
			// Disables disabled state
			CurrentStates &= ~InteractableStates.EffectDisabled;

			// Invoke callbacks for further initialization
			OnEffectEnable();
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Get the highest priority and enabled state according to the statePriorities list, from the currently active states.
		/// <br/><br/>
		/// Defaults to <see cref="InteractableStates.Idle"/> if no state priorities are enabled.
		/// </summary>
		/// <param name="states">Currently active states.</param>
		/// <param name="statePriorities">State priority list.</param>
		/// <returns>The single highest priority active state among all active states.</returns>
		public static InteractableStates GetHighestPriorityActiveState(InteractableStates states, List<StatePriorityItem> statePriorities)
		{
			// If effect disabled state is active, return the idle state (effect disabled state has the highest priority)
			if (states.HasFlag(InteractableStates.EffectDisabled))
				return InteractableStates.Idle;
			// Index 0 = highest priority, check from highest priority to lowest
			for (int i = 0; i < statePriorities.Count; i++)
			{
				// Skip check if the state priority is disabled
				if (!statePriorities[i].enabled) continue;

				// Check if it's idle state
				// Idle state cannot use HasFlag as it will always return true.
				// Instead, we have to directly compare the value.
				if (statePriorities[i].state == InteractableStates.Idle)
				{
					if (states == InteractableStates.Idle)
						return InteractableStates.Idle;
					else
						continue;
				}

				// Check if the current state matches the current state priority
				if (states.HasFlag(statePriorities[i].state))
					return statePriorities[i].state;
			}
			// If none of the state priorities are enabled, return the idle state by default.
			return InteractableStates.Idle;
		}

		/// <summary>
		/// Get the helper tooltip string about the different interactable states.
		/// </summary>
		/// <param name="state">Interactable state to retrieve tooltip information about.</param>
		/// <returns>Tooltip string about the input interactable state.</returns>
		public static string GetTooltip(string state)
		{
			switch (state.ToLower())
			{
				case "idle": return TOOLTIP_STATE_IDLE;
				case "hover": return TOOLTIP_STATE_HOVER;
				case "select": return TOOLTIP_STATE_SELECT;
				case "focus": return TOOLTIP_STATE_FOCUS;
				case "activated": return TOOLTIP_STATE_ACTIVATED;
				default: return "Unknown state.";
			}
		}

		/// <summary>
		/// Wrapper for logging messages in the initialization step.
		/// </summary>
		/// <param name="message">Debug message.</param>
		protected void PrintInitWarning(string message) => Debug.Log(FormatDebugMessage(message));

		/// <summary>
		/// Helper tool for formatting debug messages.
		/// </summary>
		/// <param name="message">Debug message to be formatted.</param>
		/// <returns>Formatted debug message.</returns>
		protected string FormatDebugMessage(string message) => $"[{GetType().Name}] {name}: {message}";

		public void ResetStatePriorities()
		{
			statePriorities = new List<StatePriorityItem>(defaultStatePriorities);
		}

		#endregion

		#region Binding Event Method Wrappers
		// These methods are called by the controller instance
		// as wrappers for the virtual event methods

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void FirstHoverEntered()
		{
			CurrentStates |= InteractableStates.Hover; // Set flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void LastHoverExited()
		{
			CurrentStates &= ~InteractableStates.Hover; // Clear flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void FirstSelectEntered()
		{
			CurrentStates |= InteractableStates.Select; // Set flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void LastSelectExited()
		{
			CurrentStates &= ~InteractableStates.Select; // Clear flag

			if (deactivateOnDeselect)
				CurrentStates &= ~InteractableStates.Activated; // Clear flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void FirstFocusEntered()
		{
			CurrentStates |= InteractableStates.Focus; // Set flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void LastFocusExited()
		{
			CurrentStates &= ~InteractableStates.Focus; // Clear flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void Activated()
		{
			CurrentStates |= InteractableStates.Activated; // Set flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void Deactivated()
		{
			CurrentStates &= ~InteractableStates.Activated; // Clear flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void ControllerEnabled()
		{
			CurrentStates &= ~InteractableStates.Disabled; // Clear flag
		}

		/// <summary>
		/// Base event method called by the controller.
		/// </summary>
		protected void ControllerDisabled()
		{
			CurrentStates |= InteractableStates.Disabled; // Set flag
		}

		#endregion

		#region Virtual Event Methods
		/// <summary>
		/// Called when the interactable enters a new active state.
		/// Active state is the highest priority, enabled state that is currently active on the interactable.
		/// <br></br><br></br>
		/// This method call guarantees <see cref="CheckInitialization"/> to be true before executing.
		/// </summary>
		/// <param name="prevState">Previous active state. Only has a single state value.</param>
		/// <param name="currentState">Current active state. Only has a single state value.</param>
		protected virtual void OnChangeState(GameObject target, BaseVisualEffectsController controller, InteractableStates prevState, InteractableStates currentState) { }

		/// <summary>
		/// Perform any cleanups when the interactable is disabled if necessary.
		/// </summary>
		protected virtual void OnEffectDisable() { }

		/// <summary>
		/// Perform any initializations when the interactable is enabled. This callback is invoked after the initialization step.
		/// </summary>
		protected virtual void OnEffectEnable() { }

		#endregion

	}
}
