using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace XRUIToolkit.Core.VisualEffect
{
	[CustomEditor(typeof(BaseVisualEffect), true)]
	public class BaseVisualEffectInspector : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			// Create root UI element
			VisualElement root = new();
			
			// Load stylesheet
			StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AssetPaths.stylesheetDir);
			root.styleSheets.Add(styleSheet);

			// Update once on create
			BaseVisualEffectPropertyInspector.DrawProperties(serializedObject, root);

			return root;
		}
	}
}
