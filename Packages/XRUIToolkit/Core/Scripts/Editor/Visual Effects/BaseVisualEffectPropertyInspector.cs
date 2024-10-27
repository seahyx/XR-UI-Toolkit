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
			SerializedProperty iter = effectPresetReference.GetIterator();
			if (iter.NextVisible(true)) // Access the first property
			{
				do
				{
					// Skip if property is the script
					if (iter.name.Equals("m_Script", System.StringComparison.Ordinal))
						continue;
					// Manually style the state priority listview
					if (iter.name.Equals("statePriorities", System.StringComparison.Ordinal))
					{
						Foldout priorityListFoldout = new();
						priorityListFoldout.text = iter.displayName;
						priorityListFoldout.tooltip = BaseVisualEffect.TOOLTIP_STATE_PRIORITIES;
						priorityListFoldout.style.marginLeft = 12;
						priorityListFoldout.style.marginRight = 8;
						priorityListFoldout.style.marginBottom = 4;
						priorityListFoldout.contentContainer.style.marginLeft = 4;
						priorityListFoldout.contentContainer.AddToClassList("hhui-border");
						priorityListFoldout.viewDataKey = "PriorityListFoldout";

						Label labelHighPriority = new("<b>Highest Priority</b>");
						labelHighPriority.style.paddingTop = 4;
						labelHighPriority.style.paddingBottom = 4;
						labelHighPriority.style.unityTextAlign = TextAnchor.MiddleCenter;
						labelHighPriority.style.backgroundColor = new Color(0, 0, 0, 0.1f);
						priorityListFoldout.Add(labelHighPriority);

						ListView statePriorityList = new();
						statePriorityList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
						statePriorityList.reorderable = true;
						statePriorityList.showAddRemoveFooter = false;
						statePriorityList.reorderMode = ListViewReorderMode.Animated;
						statePriorityList.showBoundCollectionSize = false;

						statePriorityList.BindProperty(iter);
						statePriorityList.Bind(effectPresetReference);

						priorityListFoldout.Add(statePriorityList);

						Label labelLowPriority = new("<b>Lowest Priority</b>");
						labelLowPriority.style.paddingTop = 4;
						labelLowPriority.style.paddingBottom = 4;
						labelLowPriority.style.unityTextAlign = TextAnchor.MiddleCenter;
						labelLowPriority.style.backgroundColor = new Color(0, 0, 0, 0.1f);
						priorityListFoldout.Add(labelLowPriority);

						// Add element to root container
						container.Add(priorityListFoldout);
						continue;
					}

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
