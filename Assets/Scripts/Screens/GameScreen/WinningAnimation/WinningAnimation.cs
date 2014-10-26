using UnityEngine;
using System.Collections;

public class WinningAnimation : MonoBehaviour {
	
	[HideInInspector]
	public SlotMachine slotMachine;
	
	public UILabel winningNumLabel;

	private float fadeInDuration = 0.3f;
	private float fadeOutDuration = 0.3f;

	public virtual void Init(SlotMachine mSlotMachine) {
		slotMachine = mSlotMachine;
	}

	public virtual void SetData(int winningVal) {
		winningNumLabel.text = winningVal.ToString("N0");
	}

	public virtual void FadeIn(bool shouldFadeOut = false, float duration = 0) {
		Utils.SetActive(gameObject, true);
		TweenAlpha tween = TweenAlpha.Begin(gameObject, fadeInDuration, 1f);
		tween.from = 0;
		if (shouldFadeOut) {
			Invoke("FadeOut", duration);
		}
	}
	
	public virtual void FadeOut() {
		TweenAlpha tween = TweenAlpha.Begin(gameObject, fadeOutDuration, 0f);
    EventDelegate.Set(tween.onFinished, Destroy);
	}

	public virtual void Hide() {
		Utils.SetActive(gameObject, false);
	}
	
	public virtual void Destroy() {
		if (slotMachine != null) {
			slotMachine.Resume();
		}
		Destroy(gameObject);
	}
}
