using Codice.Client.Common.GameUI;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRUIToolkit.Core.VisualEffect
{
	/// <summary>
	/// XR UI Visual Effect component.
	/// </summary>
	[AddComponentMenu("XR UI Toolkit/Visual Effects/XR Visual Effect")]
	[RequireComponent(typeof(XRVisualEffectsController))]
	public class XRVisualEffect : MonoBehaviour
	{
		[SerializeField,
			Tooltip("GameObject to apply the effect onto.")]
		public GameObject Target;

		[SerializeField,
			Tooltip("Preset for this effect.")]
		public BaseVisualEffect Effect;

		[SerializeField,
			Tooltip("Whether to re-initialize the effect when the controller of this visual effect is disabled.")]
		public bool ReinitializeOnDisable = false;

		/// <summary>
		/// Effect instance is created during runtime, using <see cref="Effect"/> as the preset.
		/// </summary>
		private BaseVisualEffect EffectInstance { get; set; } = null;

		/// <summary>
		/// Summary of the effect and target.
		/// </summary>
		public string SummaryName
		{
			get
			{
				string t1 = Target != null
				? Target.name
				: "No GameObject Set";

				string t2 = Effect != null
					? Regex.Replace(Effect.name, "([A-Z])", " $1").Trim()
					: "No Effect Set";

				return $"{t1} - {t2}";
			}
		}

		/// <summary>
		/// Summary of the effect and target with rich text colours.
		/// </summary>
		public string SummaryRichName
		{
			get
			{
				string t1 = Effect != null
					? Regex.Replace(Effect.name, "([A-Z])", " $1").Trim()
					: "<color=#ff4444>No Effect Set</color>";

				string t2 = Target != null
					? Target.name
					: "<color=#ff4444>No GameObject Set</color>";

				return $"{t1} - {t2}";
			}
		}

		/// <summary>
		/// The name of the target of this visual effect in rich text colours.
		/// </summary>
		public string RichTargetName
		{
			get
			{
				string t1 = Target != null
				? Target.name
				: "<color=#ff4444>None</color>";

				return $"{t1}";
			}
		}

		/// <summary>
		/// The name of the effect of this visual effect in rich text colours.
		/// </summary>
		public string RichEffectName
		{
			get
			{
				string t2 = Effect != null
					? Regex.Replace(Effect.name, "([A-Z])", " $1").Trim()
					: "<color=#ff4444>None</color>";

				return $"{t2}";
			}
		}

		/// <summary>
		/// Instantiate and initialize visual effects.
		/// <br/><br/>
		/// We need to create an instance of the effect, otherwise all the effects from the same SO will share their private variables (e.g. Target GameObject and parent interactable).
		/// <br/>
		/// This will also mean that in play mode, editor changes to the visual effect will not have an immediate effect unless we repeat this process.
		/// </summary>
		public void InitializeEffect(XRBaseInteractable interactable)
		{
			Destroy(EffectInstance);

			if (Effect == null) return;
			EffectInstance = Instantiate(Effect);
			EffectInstance.Initialize(interactable, Target);
		}
	}
}
