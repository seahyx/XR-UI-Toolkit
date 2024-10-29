using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XRUIToolkit.Core.VisualEffect
{
	[CustomPropertyDrawer(typeof(BaseVisualEffect))]
	public class BaseVisualEffectPropertyInspector : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			// Create root UI element
			VisualElement root = new();
			
			// Load stylesheet
			StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AssetPaths.stylesheetDir);
			root.styleSheets.Add(styleSheet);

			// EffectPreset field
			PropertyField presetField = new PropertyField(property);

			// Add preset field to root container
			root.Add(presetField);
			
			// Foldout container for preset settings
			GroupBox groupBox = new();
			groupBox.AddToClassList("hhui-box-border");
			root.Add(groupBox);

			// Add update effect preset to presetField's callback
			groupBox.TrackPropertyValue(property, (SerializedProperty e) =>
			{
				UpdateEffectPreset(property, groupBox);
			});
			// Update once on create
			UpdateEffectPreset(property, groupBox);

			return root;
		}

		private void UpdateEffectPreset(SerializedProperty property, GroupBox groupBox)
		{
			if (property.objectReferenceValue == null)
			{
				// No effect preset assigned, hide group
				groupBox.style.display = DisplayStyle.None;
				return;
			}

			// Otherwise, clear group children to refresh
			groupBox.style.display = DisplayStyle.Flex;
			groupBox.Clear();

			// Add effect label text
			Label label = new($"<b>{property.objectReferenceValue.name}</b>");
			label.style.paddingLeft = 4;
			label.style.paddingBottom = 4;
			groupBox.Add(label);

			SerializedObject effectPresetReference = new(property.objectReferenceValue);
			DrawProperties(effectPresetReference, groupBox);
		}

		public static void DrawProperties(SerializedObject effectPresetReference, VisualElement container)
		{
			SerializedProperty statePrioritiesProp = effectPresetReference.FindProperty("statePriorities");
			SerializedProperty deactivateDeselectProp = effectPresetReference.FindProperty("deactivateOnDeselect");

			// Manually style the state priority listview
			// Style the foldout
			Foldout priorityListFoldout = new();
			priorityListFoldout.text = statePrioritiesProp.displayName;
			priorityListFoldout.tooltip = BaseVisualEffect.TOOLTIP_STATE_PRIORITIES;
			priorityListFoldout.style.marginLeft = 12;
			priorityListFoldout.style.marginRight = 8;
			priorityListFoldout.style.marginBottom = 4;
			priorityListFoldout.contentContainer.style.marginLeft = 4;
			priorityListFoldout.contentContainer.AddToClassList("hhui-border");
			priorityListFoldout.viewDataKey = "PriorityListFoldout";

			// Highest priority label
			Label labelHighPriority = new("<b>Highest Priority</b>");
			labelHighPriority.style.paddingTop = 4;
			labelHighPriority.style.paddingBottom = 4;
			labelHighPriority.style.unityTextAlign = TextAnchor.MiddleCenter;
			labelHighPriority.style.backgroundColor = new Color(0, 0, 0, 0.1f);
			priorityListFoldout.Add(labelHighPriority);

			// Actual list
			ListView statePriorityList = new();
			statePriorityList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
			statePriorityList.reorderable = true;
			statePriorityList.showAddRemoveFooter = false;
			statePriorityList.reorderMode = ListViewReorderMode.Animated;
			statePriorityList.showBoundCollectionSize = false;

			statePriorityList.BindProperty(statePrioritiesProp);
			statePriorityList.Bind(effectPresetReference);

			priorityListFoldout.Add(statePriorityList);

			// Lowest priority label
			Label labelLowPriority = new("<b>Lowest Priority</b>");
			labelLowPriority.style.paddingTop = 4;
			labelLowPriority.style.paddingBottom = 4;
			labelLowPriority.style.unityTextAlign = TextAnchor.MiddleCenter;
			labelLowPriority.style.backgroundColor = new Color(0, 0, 0, 0.1f);
			priorityListFoldout.Add(labelLowPriority);

			// Reset list button
			Button resetListButton = new(() =>
			{
				Undo.RecordObject(effectPresetReference.targetObject, "Reset state priorities.");
				(effectPresetReference.targetObject as BaseVisualEffect).ResetStatePriorities();
				EditorApplication.delayCall += () => statePriorityList.RefreshItems();
			});
			resetListButton.text = "Reset";
			resetListButton.tooltip = "Reset the state priority list to the default order.";
			resetListButton.style.position = Position.Absolute;
			resetListButton.style.right = 0;
			resetListButton.style.top = 0;
			priorityListFoldout.Add(resetListButton);

			// Deactivate on deselect toggle
			Toggle deactivateOnDeselectToggle = new();
			deactivateOnDeselectToggle.text = deactivateDeselectProp.displayName;
			deactivateOnDeselectToggle.tooltip = BaseVisualEffect.TOOLTIP_DEACTIVATE_ON_DESELECT;
			deactivateOnDeselectToggle.BindProperty(deactivateDeselectProp);
			priorityListFoldout.Add(deactivateOnDeselectToggle);

			// Add element to root container
			container.Add(priorityListFoldout);

			// Get serialized property iterator for all the other stuff
			SerializedProperty iter = effectPresetReference.GetIterator();

			if (iter.NextVisible(true)) // Access the first property
			{
				do
				{
					// Skip if property is the script reference
					if (iter.name.Equals("m_Script", System.StringComparison.Ordinal))
						continue;
					// Skip state priority listview as it is handled separately
					if (iter.name.Equals("statePriorities", System.StringComparison.Ordinal))
						continue;
					// Skip deactivate on deselect as it is handled separately
					if (iter.name.Equals("deactivateOnDeselect", System.StringComparison.Ordinal))
						continue;

					// Create property fields for each visible field in the ScriptableObject
					PropertyField nestedField = new(iter);
					nestedField.Bind(effectPresetReference);

					// Add nested field to root container
					container.Add(nestedField);
				}
				while (iter.NextVisible(false));
			}
		}
	}
}
