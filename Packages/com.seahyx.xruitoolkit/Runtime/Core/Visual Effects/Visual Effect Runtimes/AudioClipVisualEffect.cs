using UnityEngine;
using UnityEngine.UI;

namespace XRUIToolkit.Core.VisualEffect
{
	[CreateAssetMenu(fileName = "AudioClipVFX", menuName = "HHUI Toolkit/Visual Effects/Audio/Audio Clip VFX", order = 0)]
	[DropdownMenuName("Audio/Audio Clip VFX")]
	public class AudioClipVisualEffect : BaseVisualEffect
	{
		[SerializeField,
			Tooltip("Audio clip to be played when entering the idle state.")]
		private AudioClip idle;

		[SerializeField,
			Tooltip("Audio clip to be played when entering the hover state.")]
		private AudioClip hover;

		[SerializeField,
			Tooltip("Audio clip to be played when entering the selected state.")]
		private AudioClip selected;

		[SerializeField,
			Tooltip("Audio clip to be played when entering the focus state.")]
		private AudioClip focused;

		[SerializeField,
			Tooltip("Audio clip to be played when entering the activated state.")]
		private AudioClip activated;

		[SerializeField,
			Tooltip("Audio clip to be played when entering the disabled state.")]
		private AudioClip disabled;

		private AudioSource targetAudio;

		public override void Initialize(BaseVisualEffectsController controller, GameObject target)
		{
			base.Initialize(controller, target);

			AudioSource tmp;
			if (target != null && target.TryGetComponent(out tmp))
				targetAudio = tmp;
		}

		protected override bool CheckInitialization()
		{
			if (!base.CheckInitialization()) return false;
			if (targetAudio == null)
			{
				PrintInitWarning("Target GameObject does not have a vaild AudioSource. Effect is not yet initialized.");
				return false;
			}
			return true;
		}

		protected override void OnChangeState(
			GameObject target,
			BaseVisualEffectsController controller,
			InteractableStates prevState,
			InteractableStates currentState)
		{
			switch(currentState)
			{
				case InteractableStates.Idle:
					PlayAudio(idle);
					break;
				case InteractableStates.Hover:
					PlayAudio(hover);
					break;
				case InteractableStates.Select:
					PlayAudio(selected);
					break;
				case InteractableStates.Focus:
					PlayAudio(focused);
					break;
				case InteractableStates.Activated:
					PlayAudio(activated);
					break;
				case InteractableStates.Disabled:
					PlayAudio(disabled);
					break;
			}
		}

		protected void PlayAudio(AudioClip audio)
		{
			targetAudio.PlayOneShot(audio);
		}
	}
}
