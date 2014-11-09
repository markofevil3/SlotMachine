using UnityEngine;
using System.Collections;

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

	public void Init(int type, int currentHP, int maxHP) {
		this.type = (BossType)type;
		this.maxHP = maxHP;
		this.currentHP = currentHP;
		bossSprite.spriteName = bossSpriteNames[type];
		UpdateHPBar();
	}
	
	public void GetHit(int damage) {
		currentHP = Mathf.Max(0, currentHP - damage);		
		UpdateHPBar();
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
