using UnityEngine;
using System.Collections;

public class SkillKnife : Skill {

	public Knife[] knifes;

	private Vector3 fromPos;

	public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = level;
		this.fromPos = fromPos;
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.4f);
		Invoke("Destroy", 2f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.1f);
		NGUITools.SetActive(knifes[index].gameObject, true);
		knifes[index].Shoot(bossManager.GetBossMiddlePoint(), fromPos);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < knifes.Length; i++) {
			knifes[i].Destroy();
		}
		base.Destroy();
	}
}
