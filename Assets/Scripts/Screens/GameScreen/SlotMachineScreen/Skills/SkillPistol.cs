using UnityEngine;
using System.Collections;

public class SkillPistol : Skill {

	public override void Init(int aLevel, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = aLevel;
		SpawnParticle();
		transform.position = bossManager.GetBossMiddlePoint();
		// StartCoroutine("CheckIfAlive");
		StartCoroutine("BossGetHit");
		base.Init();
	}
	
	void SpawnParticle() {
		for (int i = 0; i < listParticles.Length; i++) {
			NGUITools.SetActive(listParticles[i].gameObject, true);
		}
	}

	IEnumerator BossGetHit() {
		yield return new WaitForSeconds(0.1f);
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
}
