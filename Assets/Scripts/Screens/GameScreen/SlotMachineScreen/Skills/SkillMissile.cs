using UnityEngine;
using System.Collections;

public class SkillMissile : Skill {

	public Missile[] missles;

	private Vector3 fromPos;

	public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = level;
		this.fromPos = new Vector3(1f, 1.5f, 0);
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.7f);
		Invoke("Destroy", 2f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.25f);
		missles[index].gameObject.SetActive(true);
		// NGUITools.SetActive(meteors[index].gameObkiject, true);
		missles[index].Shoot(bossManager.GetBossMiddlePoint(), fromPos);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < missles.Length; i++) {
			missles[i].Destroy();
		}
		base.Destroy();
	}
}
