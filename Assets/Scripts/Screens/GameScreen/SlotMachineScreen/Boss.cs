using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class Boss : MonoBehaviour {

  public enum BossType {
		DRAGON_FIRE, // Boa
		DRAGON_ICE, // Crocodile
		DRAGON_DARK // Hawk Eye
  }
	
	public UITexture bossSprite;
	public UIProgressBar hpProgressBar;
	public UILabel hpLabel;
	public Transform middlePoint;

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

	public virtual string GetBossImage(int type) {
		return string.Empty;
	}

	public virtual void Init(int type, int currentHP, int maxHP, GameObject handler, string callback) {
		Init(type, currentHP, maxHP);
		this.handler = handler;
		this.callback = callback;
	}
	
	public virtual void Init(int type, int currentHP, int maxHP) {
		this.type = (BossType)type;
		this.maxHP = maxHP;
		this.currentHP = currentHP;
		this.bossSprite.mainTexture = Resources.Load(GetBossImage(type)) as Texture2D;
		Debug.Log("Init boss " + type);
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
	
	// Partical effect for each boss
	void SpawnEffect() {
		Vector2 randomPos = Random.insideUnitCircle / 2.5f;		
		GameObject effect = CFX_SpawnSystem.GetRandom();
		effect.transform.position = new Vector3(middlePoint.position.x + randomPos.x, middlePoint.position.y + randomPos.y, 0);
		effect.transform.parent = transform;
		Invoke("SpawnEffect", Random.Range(1f, 10f));
	}
	
	
	// TEST CODe
	void Awake() {
		Invoke("SpawnEffect", Random.Range(1f, 10f));
	}
	
	// TEST CODE - need to refine
	public void Shake() {
		StopShake();
		tween1 = LeanTween.rotateAroundLocal( gameObject, new Vector3(0,0,1f), 0.5f, 0.2f).setEase( LeanTweenType.easeShake ).setLoopClamp().setRepeat(-1);
		tween2 = LeanTween.rotateAroundLocal( gameObject, new Vector3(0,0,-1f), 0.5f, 0.25f).setEase( LeanTweenType.easeShake ).setLoopClamp().setRepeat(-1).setDelay(0.05f);

		LeanTween.delayedCall(gameObject, 0.5f, StopShake);
	}
	
	LTDescr tween1;
	LTDescr tween2;

	void StopShake(){
		// tween1.setRepeat(1);
		// tween2.setRepeat(1);
		LeanTween.cancel(gameObject);
		transform.localRotation = Quaternion.Euler(0, 0, 0);
	}
	// $$$$$$$$$$
}
