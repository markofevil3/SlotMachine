using UnityEngine;
using System.Collections;

public class SkillIcePurple : Skill {

	public GameObject ice;

	public override void Init(int aLevel, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = aLevel;
		Shoot();
		Invoke("BossGetHit", 0.1f);
		Invoke("Destroy", 0.5f);
	}
	
	void Shoot() {
		NGUITools.SetActive(ice, true);
	}

	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		NGUITools.SetActive(ice, false);
		base.Destroy();
	}
}
