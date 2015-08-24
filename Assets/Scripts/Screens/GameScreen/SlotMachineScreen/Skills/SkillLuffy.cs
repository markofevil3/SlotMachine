using UnityEngine;
using System.Collections;

public class SkillLuffy : Skill {

	public LuffyHand[] hands;

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
		Invoke("Destroy", 1.5f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.05f);
		NGUITools.SetActive(hands[index].gameObject, true);
		hands[index].Shoot(bossManager.GetBossMiddlePoint(), fromPos);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < hands.Length; i++) {
			hands[i].Destroy();
		}
		base.Destroy();
	}

	// public LuffyHand[] hands;
	//
	// public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos, bool isYou) {
	// 	this.bossManager = bossManager;
	// 	this.damage = damage;
	// 	int smallDamage = (int)(damage / level);
	// 	if (isYou) {
	// 		transform.localScale = Vector3.one;
	// 	} else {
	// 		transform.localScale = new Vector3(0.45f, 0.45f, 1);
	// 		transform.position = fromPos;
	//     transform.rotation = Quaternion.LookRotation(Vector3.forward, bossManager.GetBossMiddlePoint() - transform.position);
	// 	}
	// 	for (int i = 0; i < level; i++) {
	// 		if (i == level - 1 && level > 1) {
	// 			SpawnParticle(i, (damage - ((level - 1) * smallDamage)));
	// 		} else {
	// 			SpawnParticle(i, smallDamage);
	// 		}
	// 	}
	// 	Invoke("BossGetHit", 0.5f);
	//
	// 	Invoke("Destroy", 2f);
	// }
	//
	// void BossGetHit() {
	// 	bossManager.Shake();
	// 	bossManager.GetHit(damage);
	// }
	//
	// void SpawnParticle(int index, int smallDamage) {
	// 	LuffyHand hand = hands[index];
	// 	hand.gameObject.SetActive(true);
	// 	hand.Init(this.bossManager, smallDamage);
	// }
}
