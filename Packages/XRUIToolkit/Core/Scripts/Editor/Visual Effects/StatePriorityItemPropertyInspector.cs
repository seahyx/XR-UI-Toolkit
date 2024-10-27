using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace XRUIToolkit.Core.VisualEffect
{
	[CustomPropertyDrawer(typeof(StatePriorityItem))]
	public class StatePriorityItemPropertyInspector : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement root = new();
			root.style.flexDirection = FlexDirection.Row;

			SerializedProperty enabledProp = property.FindPropertyRelative("enabled");
			SerializedProperty nameProp = property.FindPropertyRelative("state");

			Toggle enabledField = new();
			enabledField.style.paddingLeft = 0;
			enabledField.style.paddingRight = 8;
			enabledField.BindProperty(enabledProp);
			root.Add(enabledField);

			Label nameLabel = new() { text = nameProp.enumDisplayNames[nameProp.enumValueIndex] };
			nameLabel.tooltip = BaseVisualEffect.GetTooltip(nameProp.enumDisplayNames[nameProp.enumValueIndex]);
			root.Add(nameLabel);

			return root;
		}
	}
}