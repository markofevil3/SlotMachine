using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class BigWinPanelDragon : BigWinPanel {
	
	public UIWidget paperScroll;
	
	// Fade in Treasure
	public override void FadeInTreasure(int cash, int gem, JSONObject newBossData, bool shouldPause = true) {
		if (shouldPause) {
			ScreenManager.Instance.CurrentSlotScreen.PauseSpawnSkill();
		}
		NGUITools.SetActive(gameObject, true);
		NGUITools.SetActive(bigWinView, false);
		NGUITools.SetActive(freeSpinView, false);
		NGUITools.SetActive(treasureView, true);
		paperScroll.height = 46;
		treasureCashLabel.text = cash.ToString("N0");
		treasureGemLabel.text = gem.ToString("N0");
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 1);
		tween.from = 0;
		transform.localScale = Vector3.one * 3;
		LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutCubic);
		StartCoroutine(DropPaperScrollDown());
		StartCoroutine(FadeOutTreasure(cash, gem, newBossData));
	}

	IEnumerator DropPaperScrollDown() {
		yield return new WaitForSeconds(0.5f);
		TweenHeight tween = TweenHeight.Begin(paperScroll, 0.5f, 136);
	}

	public IEnumerator FadeOutTreasure(int dropCash, int dropGem, JSONObject newBossData) {
		yield return new WaitForSeconds(3.0f);
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Add(tween.onFinished, HideNoResumeSkill, true);
	}
	
}
