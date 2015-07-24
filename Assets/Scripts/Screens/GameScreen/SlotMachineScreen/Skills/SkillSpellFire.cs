using UnityEngine;
using System.Collections;

public class SkillSpellFire : Skill {
	
	public SpellFire[] spellFires;

	private Vector3 fromPos;

	public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = level;
		this.fromPos = fromPos;
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.6f);
		Invoke("Destroy", 3f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.2f);
		NGUITools.SetActive(spellFires[index].gameObject, true);
		spellFires[index].Shoot(bossManager.GetBossMiddlePoint(), fromPos);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < spellFires.Length; i++) {
			spellFires[i].Destroy();
		}
	}
}
