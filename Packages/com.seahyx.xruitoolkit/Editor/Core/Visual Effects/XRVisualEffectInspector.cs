using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XRUIToolkit.Core.VisualEffect
{
	[CustomEditor(typeof(XRVisualEffect), true)]
	public class XRVisualEffectInspector : Editor
	{
		private const string ASSET_FOLDER_PATH = "Assets/XRUI/Visual Effects";

		public override VisualElement CreateInspectorGUI()
		{
			// Create root UI element
			VisualElement root = new();

			// Get component ref
			XRVisualEffect xrVisualEffect = target as XRVisualEffect;

			// Add title label
			Label titleLabel = new($"<b>{xrVisualEffect.SummaryRichName}</b>");
			titleLabel.style.fontSize = 16;
			titleLabel.style.paddingTop = 4;
			titleLabel.style.paddingBottom = 4;
			root.Add(titleLabel);

			// Add enabled toggle
			SerializedProperty enabledProp = serializedObject.FindProperty("isEnabled");
			PropertyField enabledField = new(enabledProp);
			root.Add(enabledField);

			// Add foldout for everything else
			Foldout foldout = new();
			foldout.text = "Settings";
			foldout.contentContainer.style.marginLeft = 0;
			foldout.viewDataKey = xrVisualEffect.SummaryRichName;

			// Get Target property
			SerializedProperty targetProp = serializedObject.FindProperty("Target");
			PropertyField targetField = new(targetProp);
			foldout.Add(targetField);

			// Add a button to "Add this GameObject to target" if Target field is empty
			Button addThisGOToTargetButton = new(() =>
			{
				// Get the component of this serialized object
				Component c = target as Component;
				if (c == null) return;

				// From the component, get the GameObject it is attached on
				targetProp.objectReferenceValue = c.gameObject;
				targetProp.serializedObject.ApplyModifiedProperties();
			});
			addThisGOToTargetButton.text = "Add This GameObject To Target";

			// Track changes in the Target field and show button if Target field is empty
			addThisGOToTargetButton.style.display = targetProp.objectReferenceValue == null ? DisplayStyle.Flex : DisplayStyle.None; // initial update
			targetField.TrackPropertyValue(targetProp, (SerializedProperty e) =>
			{
				addThisGOToTargetButton.style.display = targetProp.objectReferenceValue == null ? DisplayStyle.Flex : DisplayStyle.None;

				// Also update the title label
				titleLabel.text = $"<b>{xrVisualEffect.SummaryRichName}</b>";
			});
			foldout.Add(addThisGOToTargetButton);


			// Get ResetOnDisable property
			SerializedProperty resetProp = serializedObject.FindProperty("ResetOnDisable");
			PropertyField resetField = new(resetProp);
			foldout.Add(resetField);

			// Get Effect property
			SerializedProperty effectProp = serializedObject.FindProperty("Effect");
			PropertyField effectField = new(effectProp);
			foldout.Add(effectField);


			// Add a button to create new visual effect asset
			Button createNewEffectAssetButton = new(() => DisplayCreateEffectAssetMenu(effectField, effectProp));
			createNewEffectAssetButton.text = "Create New Visual Effect";

			// Track changes in the Preset field and show button if Preset field is empty
			createNewEffectAssetButton.style.display = effectProp.objectReferenceValue == null ? DisplayStyle.Flex : DisplayStyle.None; // initial update
			effectField.TrackPropertyValue(effectProp, (SerializedProperty e) =>
			{
				createNewEffectAssetButton.style.display = effectProp.objectReferenceValue == null ? DisplayStyle.Flex : DisplayStyle.None;

				// Also update the title label
				titleLabel.text = $"<b>{xrVisualEffect.SummaryRichName}</b>";
			});
			foldout.Add(createNewEffectAssetButton);

			root.Add(foldout);

			return root;
		}

		/// <summary>
		/// Get all usable visual effect classes using reflection and display in a dropdown menu.
		/// </summary>
		private void DisplayCreateEffectAssetMenu(VisualElement anchor, SerializedProperty effectProp)
		{
			// Create dropdown menu
			GenericMenu menu = new();

			// Get all usable visual effect classes using reflection
			Type baseType = typeof(BaseVisualEffect);
			Assembly assembly = baseType.Assembly;
			List<Type> effectTypeList = new();
			foreach (Type derivedType in assembly.GetTypes()
				.Where(t => !t.IsAbstract &&
							!t.IsInterface &&
							t.IsSubclassOf(baseType)))
			{
				string menuName = derivedType.Name;
				string filename = derivedType.Name;
				var dropdownNameAttr = derivedType.GetCustomAttribute<DropdownMenuNameAttribute>();
				if (dropdownNameAttr != null)
					menuName = dropdownNameAttr.MenuName;
				var assetMenuAttr = derivedType.GetCustomAttribute<CreateAssetMenuAttribute>();
				if (assetMenuAttr != null)
					filename = assetMenuAttr.fileName;

				effectTypeList.Add(derivedType);
				menu.AddItem(new GUIContent(menuName), false, () =>
				{
					// Create default folders if doesn't exist
					Directory.CreateDirectory(ASSET_FOLDER_PATH);

					// Create effect asset
					string path = EditorUtility.SaveFilePanelInProject(
						"Create New Visual Effect",
						filename,
						"asset",
						"Create a name and select a location for this visual effect",
						ASSET_FOLDER_PATH);
					if (path != "")
					{
						ScriptableObject instance = ScriptableObject.CreateInstance(derivedType);
						AssetDatabase.CreateAsset(instance, path);
						AssetDatabase.Refresh();
						effectProp.objectReferenceValue = instance;
						effectProp.serializedObject.ApplyModifiedProperties();
					}
				});
			}

			menu.ShowAsContext();
		}
	}
}