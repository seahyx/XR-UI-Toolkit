using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	[CustomEditor(typeof(XRVisualEffectsController), true), CanEditMultipleObjects]
	public class XRVisualEffectsControllerInspector : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			// Create root UI element
			VisualElement root = new();

			// Get interactable property
			SerializedProperty interactableProp = serializedObject.FindProperty("interactable");
			PropertyField interactableField = new(interactableProp);
			root.Add(interactableField);

			// Warning label group
			VisualElement interactableWarningGroup = new();
			HelpBox warningLabel = new("No XRBaseInteractable found on this GameObject. Add one to use as the interactable.", HelpBoxMessageType.Warning);
			interactableWarningGroup.Add(warningLabel);
			Button addInteractableButton = new(() =>
			{
				// Add XRBaseInteractable to each GameObject that does not have one
				foreach (XRVisualEffectsController target in targets)
				{
					SerializedObject serTarget = new(target);
					SerializedProperty serTargetInteractableProp = serTarget.FindProperty(interactableProp.propertyPath);
					if (serTargetInteractableProp.objectReferenceValue == null && target.GetComponent<XRBaseInteractable>() == null)
						Undo.AddComponent<XRSimpleInteractable>(target.gameObject);
				}
				// Refresh the UI
				//CreateInspectorGUI();
			});
			addInteractableButton.text = "Add XRSimpleInteractable";
			interactableWarningGroup.Add(addInteractableButton);
			root.Add(interactableWarningGroup);

			// Update warning label group visibility
			interactableWarningGroup.style.display = ShowWarning(interactableProp) ? DisplayStyle.Flex : DisplayStyle.None;
			interactableField.TrackPropertyValue(interactableProp, evt =>
			{
				interactableWarningGroup.style.display = ShowWarning(interactableProp) ? DisplayStyle.Flex : DisplayStyle.None;
			});


			// Show XR Visual Effects list

			// Get list of visual effect components
			XRVisualEffectsController controller = target as XRVisualEffectsController;
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
			xrMultiListView.columns.Add(new Column { name = "actions", title = "Actions", width = 80 });

			// Create column cell
			xrMultiListView.columns["target"].makeCell = MakeLabel;
			xrMultiListView.columns["type"].makeCell = MakeLabel;
			xrMultiListView.columns["actions"].makeCell = () => new Button() { text = "Remove" };

			// For each column cell, create binding function
			xrMultiListView.columns["target"].bindCell = (e, i) => (e as Label).text = visualEffects[i].RichTargetName;
			xrMultiListView.columns["type"].bindCell = (e, i) => (e as Label).text = visualEffects[i].RichEffectName;
			xrMultiListView.columns["actions"].bindCell = (e, i) =>
			{
				Button button = e as Button;
				button.clicked += () => Undo.DestroyObjectImmediate(visualEffects[i]);
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
			refreshButton.style.flexGrow = 0;
			foldoutButtons.Add(refreshButton);

			visualEffectListFoldout.Add(foldoutButtons);
			visualEffectListFoldout.Add(xrMultiListView);
			root.Add(visualEffectListFoldout);

			// Button to add new visual effect
			Button addNewEffectButton = new(() => Undo.AddComponent<XRVisualEffect>(controller.gameObject));
			addNewEffectButton.text = "Add New Visual Effect";
			root.Add(addNewEffectButton);

			return root;
		}


		/// <summary>
		/// Checks whether to show the warning label.
		/// </summary>
		/// <param name="interactableProp"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private bool ShowWarning(SerializedProperty interactableProp)
		{
			bool showWarning = false;
			foreach (XRVisualEffectsController target in targets)
			{
				SerializedObject serTarget = new(target);
				SerializedProperty serTargetInteractableProp = serTarget.FindProperty(interactableProp.propertyPath);
				if (serTargetInteractableProp.objectReferenceValue == null && target.GetComponent<XRBaseInteractable>() == null)
				{
					showWarning = true;
					break;
				}
			}
			return showWarning;
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
