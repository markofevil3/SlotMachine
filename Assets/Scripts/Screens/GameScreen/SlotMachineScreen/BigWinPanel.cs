using UnityEngine;
using System.Collections;

public class BigWinPanel : MonoBehaviour {

	private Vector3 minScale = new Vector3(0.5f, 0.5f, 1f);
	private int totalScore = 0;

	public UILabel winNumbLabel;

	public void FadeIn(int numb) {
		totalScore = numb;
		
		NGUITools.SetActive(gameObject, true);
		winNumbLabel.text = numb.ToString("N0");
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 1);
		tween.from = 0;
		transform.localScale = Vector3.one * 3;
		LeanTween.scale(gameObject, Vector3.one, 0.2f).setEase(LeanTweenType.easeInOutCubic);
		Invoke("FadeOut", 2f);
		ScreenManager.Instance.CurrentSlotScreen.PauseSpawnSkill();
	}

	public void FadeOut() {
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Add(tween.onFinished, Hide, true);
		ScreenManager.Instance.CurrentSlotScreen.slotMachine.UpdateScore(totalScore);
	}
	
	public void Hide() {
		NGUITools.SetActive(gameObject, false);
		if (ScreenManager.Instance.CurrentSlotScreen != null) {
			ScreenManager.Instance.CurrentSlotScreen.ResumeSpawnSkill();
		}
	}
}
