using UnityEngine;
using System.Collections;

public class SkillLuffy : Skill {

	public LuffyHand[] hands;
	
	private int attackDamage;

	public override void Init(int level, int damage, Boss boss) {
		this.boss = boss;
		this.attackDamage = damage;
		int smallDamage = (int)(damage / level);
		for (int i = 0; i < level; i++) {
			if (i == level - 1 && level > 1) {
				SpawnParticle(i, (damage - ((level - 1) * smallDamage)));
			} else {
				SpawnParticle(i, smallDamage);
			}
		}
		Invoke("Destroy", 3f);
	}
	
	void SpawnParticle(int index, int smallDamage) {
		Debug.Log(smallDamage);
		LuffyHand hand = hands[index];
		hand.gameObject.SetActive(true);
		hand.Init(this.boss, smallDamage);
	}
	
	void Destroy() {
		Destroy(gameObject);
	}
}
