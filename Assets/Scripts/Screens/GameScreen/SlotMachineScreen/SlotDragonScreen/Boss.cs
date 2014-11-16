using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class Boss : MonoBehaviour {

  public enum BossType {
		DRAGON_FIRE,
		DRAGON_ICE,
		DRAGON_DARK
  }
	private string[] bossSpriteNames = new string[3] {"Battle_Portriat_Giant", "Battle_Portriat_Thief", "Battle_Portriat_Monkey"};

	public UISprite bossSprite;
	public UIProgressBar hpProgressBar;
	public UILabel hpLabel;

	private BossType type;
	private int maxHP;
	private int currentHP;
	private float progressTarget;
	
	// Callback event
	private GameObject handler;
	// Get Hit callback
	private string callback = string.Empty;
	// FadeIn callback
	private string fadeInCallback = string.Empty;
	private JSONObject newBossData;

	public void Init(int type, int currentHP, int maxHP, GameObject handler, string callback) {
		Init(type, currentHP, maxHP);
		this.handler = handler;
		this.callback = callback;
	}
	
	public void Init(int type, int currentHP, int maxHP) {
		this.type = (BossType)type;
		this.maxHP = maxHP;
		this.currentHP = currentHP;
		bossSprite.spriteName = bossSpriteNames[type];
		UpdateHPBar();
	}
	
	public void ChangeBoss(JSONObject jsonData, string fadeInCallback) {
		this.fadeInCallback = fadeInCallback;
		newBossData = jsonData;
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Add(tween.onFinished, ChangeBossFadeIn, true);
	}
	
	private void ChangeBossFadeIn() {
		Init(newBossData.GetInt("dIndex"), newBossData.GetInt("dHP"), newBossData.GetInt("dMaxHP"));
		FadeIn();
	}
	
	private void FadeIn() {
		TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 1);
    EventDelegate.Add(tween.onFinished, FadeInCallback, true);
	}
	
	private void FadeInCallback() {
		if (handler != null && fadeInCallback != string.Empty) {
			handler.SendMessage(fadeInCallback, null, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void GetHit(int damage) {
		currentHP = Mathf.Max(0, currentHP - damage);		
		UpdateHPBar();
		if (handler != null && callback != string.Empty) {
			handler.SendMessage(callback, null, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	private void UpdateHPBar() {
		hpLabel.text = currentHP.ToString("N0");
		progressTarget = (float)currentHP / (float)maxHP;
	}
	
	void Update() {
		if (progressTarget != hpProgressBar.value) {
	    hpProgressBar.value = Mathf.Lerp(hpProgressBar.value, progressTarget, Time.deltaTime * 5f);
		}
	}
}
