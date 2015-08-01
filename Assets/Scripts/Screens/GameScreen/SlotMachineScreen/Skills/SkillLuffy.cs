using UnityEngine;
using System.Collections;

public class SkillLuffy : Skill {

	public LuffyHand[] hands;
	
	public override void Init(int level, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.damage = damage;
		int smallDamage = (int)(damage / level);
		for (int i = 0; i < level; i++) {
			if (i == level - 1 && level > 1) {
				SpawnParticle(i, (damage - ((level - 1) * smallDamage)));
			} else {
				SpawnParticle(i, smallDamage);
			}
		}
		Invoke("BossGetHit", 0.5f);
		
		Invoke("Destroy", 2f);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	void SpawnParticle(int index, int smallDamage) {
		LuffyHand hand = hands[index];
		hand.gameObject.SetActive(true);
		hand.Init(this.bossManager, smallDamage);
	}
}
