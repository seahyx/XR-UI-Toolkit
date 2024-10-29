using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace XRUIToolkit.Core.UIConfig
{
	[CustomEditor(typeof(UIConfigurator))]
	public class UIConfiguratorInspector : Editor
    {
		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new();

			// Get ui config list prop
			SerializedProperty uiConfigListProp = serializedObject.FindProperty("uiConfigs");
			for (int i = 0;  i < uiConfigListProp.arraySize; i++)
			{
				// Draw array element
				GroupBox box = new();
				box.text = "UI Config";

				SerializedProperty configItem = uiConfigListProp.GetArrayElementAtIndex(i);
				SerializedObject configObj = new(configItem.objectReferenceValue);
				DrawProperties(configObj, box);

				root.Add(box);
			}

			Button addButton = new(() =>
			{
				uiConfigListProp.InsertArrayElementAtIndex(uiConfigListProp.arraySize);
				uiConfigListProp.GetArrayElementAtIndex(uiConfigListProp.arraySize - 1).objectReferenceValue = new TextMeshProUIConfig();
				serializedObject.ApplyModifiedProperties();
			})
			{ text = "Add UI Config" };
			root.Add(addButton);

			Button removeButton = new(() =>
			{
				uiConfigListProp.DeleteArrayElementAtIndex(uiConfigListProp.arraySize - 1);
			})
			{ text  = "Remove UI Config" };
			root.Add(removeButton);

			return root;
		}

		public static void DrawProperties(SerializedObject serializedObj, VisualElement container)
		{
			SerializedProperty iter = serializedObj.GetIterator();
			if (iter.NextVisible(true)) // Access the first property
			{
				do
				{
					// Skip if property is the script
					if (iter.name.Equals("m_Script", System.StringComparison.Ordinal))
						continue;

					// Create property fields for each visible field in the serialized object
					PropertyField nestedField = new(iter);
					nestedField.Bind(serializedObj);

					// Add nested field to root container
					container.Add(nestedField);
				}
				while (iter.NextVisible(false));
			}
		}
	}
}
