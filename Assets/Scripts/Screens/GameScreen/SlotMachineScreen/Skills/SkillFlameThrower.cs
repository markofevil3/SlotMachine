using UnityEngine;
using System.Collections;

public class SkillFlameThrower : Skill {

	public GameObject flameBig;
	public GameObject flameSmall;

	public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos, bool isYou) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = level;
		if (isYou) {
			flameBig.transform.position = fromPos;
			NGUITools.SetActive(flameBig, true);
			NGUITools.SetActive(flameSmall, false);
		} else {
			flameSmall.transform.position = fromPos;
	    flameSmall.transform.rotation = Quaternion.LookRotation(bossManager.GetBossMiddlePoint() - flameSmall.transform.position);
			NGUITools.SetActive(flameBig, false);
			NGUITools.SetActive(flameSmall, true);
		}
		Invoke("BossGetHit", 1.5f);
		Invoke("Destroy", 5f);
	}
	
	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		NGUITools.SetActive(flameBig, false);
		NGUITools.SetActive(flameSmall, false);
		base.Destroy();
	}
}
