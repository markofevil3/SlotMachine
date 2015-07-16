using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class BigWinPanel : MonoBehaviour {

	private Vector3 minScale = new Vector3(0.5f, 0.5f, 1f);
	private int totalScore = 0;
	
	public UIPanel bigWinPanel;
	public GameObject bigWinView;
	public GameObject freeSpinView;
	public GameObject treasureView;
	public UILabel winNumbLabel;
	public UILabel freeSpinNumbLabel;
	public UILabel treasureCashLabel;
	public UILabel treasureGemLabel;
	public UITable treasureTable;
	public Transform effectPoint1;
	public Transform effectPoint2;
	public Transform effectPoint3;

	public virtual float BIG_WIN_FADEIN_DURATION {
		get { return 0.3f; }
	}

	public virtual float BIG_WIN_STAY_DURATION {
		get { return 2f; }
	}

	// Fade in Big Win
	public void FadeInBigWin(int numb) {
		Debug.Log("FadeInBigWin");
		totalScore = numb;
		NGUITools.SetActive(gameObject, true);
		NGUITools.SetActive(bigWinView, true);
		NGUITools.SetActive(freeSpinView, false);
		NGUITools.SetActive(treasureView, false);
		winNumbLabel.text = numb.ToString("N0");
		// TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 1);
		// tween.from = 0;
    // EventDelegate.Add(tween.onFinished, EventFinishFadeIn, true);
		// transform.localScale = Vector3.one * 3;
		bigWinPanel.alpha = 1;
		LeanTween.scale(gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutCubic);
		Invoke("FadeOutBigWin", BIG_WIN_STAY_DURATION);
		Invoke("EventFinishFadeIn", BIG_WIN_FADEIN_DURATION);
		ScreenManager.Instance.CurrentSlotScreen.PauseSpawnSkill();
	}

	public void FadeOutBigWin() {
		if (ScreenManager.Instance.CurrentSlotScreen.slotMachine.gotFreeSpin) {
			FadeInFreeSpin(ScreenManager.Instance.CurrentSlotScreen.slotMachine.freeSpinLeft, false);
		} else {
			TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
	    EventDelegate.Add(tween.onFinished, Hide, true);
		}
		ScreenManager.Instance.CurrentSlotScreen.slotMachine.UpdateScore(totalScore);
	}

	// Fade in Free Spin
	public void FadeInFreeSpin(int numb, bool shouldPause = true) {
		Debug.Log("FadeInFreeSpin");
		
		NGUITools.SetActive(gameObject, true);
		NGUITools.SetActive(bigWinView, false);
		NGUITools.SetActive(treasureView, false);
		NGUITools.SetActive(freeSpinView, true);
		freeSpinNumbLabel.text = numb.ToString("N0") + "\nFREE SPINS";
		bigWinPanel.alpha = 1;
		// TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 1);
		// tween.from = 0;
		// transform.localScale = Vector3.one * 3;
		// LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutCubic);
		Invoke("FadeOutFreeSpin", 2f);

		if (ScreenManager.Instance.CurrentSlotScreen != null) {
			if (shouldPause) {
				ScreenManager.Instance.CurrentSlotScreen.PauseSpawnSkill();
			}
			// Start background animation while in free spin
			ScreenManager.Instance.CurrentSlotScreen.ShowFreeSpinAnimation();
		}
	}

	public void FadeOutFreeSpin() {
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Add(tween.onFinished, Hide, true);
		// ScreenManager.Instance.CurrentSlotScreen.slotMachine.UpdateScore(totalScore);
	}

	// Fade in Treasure
	public void FadeInTreasure(int cash, int gem, JSONObject newBossData, bool shouldPause = true) {
		if (shouldPause) {
			ScreenManager.Instance.CurrentSlotScreen.PauseSpawnSkill();
		}
		NGUITools.SetActive(gameObject, true);
		NGUITools.SetActive(bigWinView, false);
		NGUITools.SetActive(freeSpinView, false);
		NGUITools.SetActive(treasureView, true);
		treasureCashLabel.text = cash.ToString("N0");
		treasureGemLabel.text = gem.ToString("N0");
		treasureTable.Reposition();
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 1);
		tween.from = 0;
		transform.localScale = Vector3.one * 3;
		LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutCubic);
		StartCoroutine(FadeOutTreasure(cash, gem, newBossData));
	}

	IEnumerator FadeOutTreasure(int dropCash, int dropGem, JSONObject newBossData) {
		yield return new WaitForSeconds(2.0f);
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Add(tween.onFinished, HideNoResumeSkill, true);
	}

	// Spawn effect of Big win panel
	void EventFinishFadeIn() {
		StartCoroutine(SpawnEffect());
	}

	IEnumerator SpawnEffect() {
		yield return null;
		Transform hpny1 = MyPoolManager.Instance.Spawn("HappyNewYear", effectPoint1.position, Quaternion.Euler(0, 0, 0), ScreenManager.Instance.CurrentSlotScreen.skillCamera.transform);
		MyPoolManager.Instance.Despawn(hpny1, 2f);
		yield return new WaitForSeconds(0.2f);
		Transform hpny2 = MyPoolManager.Instance.Spawn("HappyNewYear", effectPoint2.position, Quaternion.Euler(0, 0, 0), ScreenManager.Instance.CurrentSlotScreen.skillCamera.transform);
		MyPoolManager.Instance.Despawn(hpny2, 2f);
		yield return new WaitForSeconds(0.1f);
		Transform hpny3 = MyPoolManager.Instance.Spawn("HappyNewYear", effectPoint3.position, Quaternion.Euler(0, 0, 0), ScreenManager.Instance.CurrentSlotScreen.skillCamera.transform);
		MyPoolManager.Instance.Despawn(hpny3, 2f);
	}
	
	// Hide Treasure panel but dont resume skill - will resume after boss changed
	private void HideNoResumeSkill() {
		NGUITools.SetActive(gameObject, false);
	}
	
	// Hide Big win, free spin panel and resume skill
	public void Hide() {
		NGUITools.SetActive(gameObject, false);
		if (ScreenManager.Instance.CurrentSlotScreen != null && !ScreenManager.Instance.CurrentSlotScreen.isChangingBoss) {
			ScreenManager.Instance.CurrentSlotScreen.ResumeSpawnSkill();
		}
	}
}
