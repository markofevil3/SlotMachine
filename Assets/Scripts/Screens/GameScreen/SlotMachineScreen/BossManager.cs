using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class BossManager : MonoBehaviour {
	
	[HideInInspector]
	public Boss boss;
	
	public UIProgressBar hpProgressBar;
	public UILabel hpLabel;
	public Transform middlePoint;

	private int type;
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
	private BaseSlotMachineScreen slotMachineScreen;
	
	private string[] bossPiratePrefabs = new string[3] {"BossBoa", "BossCrocodile", "BossHawkEye"};
	private string[] bossDragonPrefabs = new string[3] {"BossBoa", "BossCrocodile", "BossHawkEye"};
	private string[] bossZombiePrefabs = new string[3] {"BossBoa", "BossCrocodile", "BossHawkEye"};

	public virtual string GetBossPrefabName(BaseSlotMachineScreen.GameType gameType, int type) {
		switch(gameType) {
			case BaseSlotMachineScreen.GameType.SLOT_PIRATE:
				return bossPiratePrefabs[type];
			break;
			case BaseSlotMachineScreen.GameType.SLOT_DRAGON:
				return bossDragonPrefabs[type];
			break;
			case BaseSlotMachineScreen.GameType.SLOT_ZOMBIE:
				return bossZombiePrefabs[type];
			break;
		}
		Debug.LogError("Cant find boss: " + gameType + " type: " + type);
		return string.Empty;
	}

	public virtual Vector3 GetBossMiddlePoint() {
		return boss.middlePoint.position;
	}

	public virtual void Init(int type, int currentHP, int maxHP, BaseSlotMachineScreen slotMachineScreen, string callback) {
		this.slotMachineScreen = slotMachineScreen;
		this.handler = slotMachineScreen.gameObject;
		this.callback = callback;
		Init(type, currentHP, maxHP);
	}
	
	public virtual void Init(int type, int currentHP, int maxHP) {
		this.type = type;
		this.maxHP = maxHP;
		this.currentHP = currentHP;
		// TEST CODE - fixed boss
		Transform bossTrans = MyPoolManager.Instance.Spawn(GetBossPrefabName(slotMachineScreen.gameType, type), transform);
		boss = bossTrans.GetComponent<Boss>();
		boss.Init();
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
		boss.Destroy();
		boss = null;
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
