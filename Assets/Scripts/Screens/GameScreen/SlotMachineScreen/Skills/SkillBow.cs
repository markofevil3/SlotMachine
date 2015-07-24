using UnityEngine;
using System.Collections;

public class SkillBow : Skill {

	public BowArrow[] arrows;

	private Vector3 fromPos;

	public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = level;
		level += 2;
		this.fromPos = new Vector3(0, 1.5f, 0);
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.5f);
		Invoke("Destroy", 1.5f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.1f);
		NGUITools.SetActive(arrows[index].gameObject, true);
		arrows[index].Shoot(bossManager.GetBossMiddlePoint(), fromPos);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < arrows.Length; i++) {
			arrows[i].Destroy();
		}
	}
}
