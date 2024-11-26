using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XRUIToolkit.Core.VisualEffect
{
	[CustomEditor(typeof(UnityUIVisualEffectsController)), CanEditMultipleObjects]
	public class UnityUIVisualEffectsControllerInspector : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			// Create root UI element
			VisualElement root = new();


			// Helpbox for if toggle is detected, use Activated state
			HelpBox toggleInfo = new("A Toggle is found on this GameObject. The Activated state will represent the \"on\" state of the Toggle, and the Idle state will represent the \"off\" state.", HelpBoxMessageType.Info);
			toggleInfo.style.display = ShowInfo() ? DisplayStyle.Flex : DisplayStyle.None;
			root.Add(toggleInfo);


			// Show XR Visual Effects list

			// Get list of visual effect components
			UnityUIVisualEffectsController controller = target as UnityUIVisualEffectsController;
			List<XRVisualEffect> visualEffects = new(controller.GetComponents<XRVisualEffect>());

			// Create MultiColumnListView
			MultiColumnListView xrMultiListView = new()
			{
				showBoundCollectionSize = false,
				showBorder = true,
				horizontalScrollingEnabled = false,
			};
			xrMultiListView.itemsSource = visualEffects;
			xrMultiListView.columns.Add(new Column { name = "target", title = "Target", width = Length.Percent(100)});
			xrMultiListView.columns.Add(new Column { name = "type", title = "Type", width = 160 });
			xrMultiListView.columns.Add(new Column { name = "actions", title = "Actions", width = 140 });

			// Create column cell
			xrMultiListView.columns["target"].makeCell = MakeLabel;
			xrMultiListView.columns["type"].makeCell = MakeLabel;
			xrMultiListView.columns["actions"].makeCell = () =>
			{
				VisualElement cell = new();
				cell.style.flexDirection = FlexDirection.Row;
				cell.Add(new Button() { name = "reset", text = "Reset" });
				cell.Add(new Button() { name = "remove", text = "Remove" });
				return cell;
			};

			// For each column cell, create binding function
			xrMultiListView.columns["target"].bindCell = (e, i) => (e as Label).text = visualEffects[i].RichTargetName;
			xrMultiListView.columns["type"].bindCell = (e, i) => (e as Label).text = visualEffects[i].RichEffectName;
			xrMultiListView.columns["actions"].bindCell = (e, i) =>
			{
				Button resetBtn = e.Q("reset") as Button;
				resetBtn.tooltip = "Re-initialize the visual effect during runtime.";
				resetBtn.clicked += () => visualEffects[i].InitializeEffect(controller);
				resetBtn.SetEnabled(Application.isPlaying);
				Button removeBtn = e.Q("remove") as Button;
				removeBtn.tooltip = "Deletes the visual effect component.";
				removeBtn.clicked += () => Undo.DestroyObjectImmediate(visualEffects[i]);
			};

			// Create foldout
			Foldout visualEffectListFoldout = new();
			visualEffectListFoldout.text = "Visual Effects";
			visualEffectListFoldout.contentContainer.style.marginLeft = 0;

			VisualElement foldoutButtons = new();
			foldoutButtons.style.flexDirection = FlexDirection.RowReverse;

			// Add refresh button to foldout
			Button refreshButton = new(xrMultiListView.RefreshItems);
			refreshButton.text = "Refresh List";
			refreshButton.tooltip = "Refresh the list of visual effects.";
			refreshButton.style.flexGrow = 0;
			foldoutButtons.Add(refreshButton);

			visualEffectListFoldout.Add(foldoutButtons);
			visualEffectListFoldout.Add(xrMultiListView);
			root.Add(visualEffectListFoldout);

			// Button to add new visual effect
			Button addNewEffectButton = new(() => Undo.AddComponent<XRVisualEffect>(controller.gameObject));
			addNewEffectButton.text = "Add New Visual Effect";
			addNewEffectButton.tooltip = "Add a new Visual Effect component to this GameObject.";
			root.Add(addNewEffectButton);

			return root;
		}

		/// <summary>
		/// Checks whether to show the toggle info helpbox.
		/// </summary>
		/// <returns></returns>
		private bool ShowInfo()
		{
			return (target as UnityUIVisualEffectsController).GetComponent<UnityEngine.UI.Toggle>() != null;
		}

		/// <summary>
		/// Creates a vertically centered label for the MultiColumnListView.
		/// </summary>
		/// <returns></returns>
		private Label MakeLabel()
		{
			Label label = new();
			label.style.marginLeft = 6;
			label.style.marginRight = 6;
			label.style.height = Length.Percent(100);
			label.style.unityTextAlign = TextAnchor.MiddleLeft;
			return label;
		}
	}
}
