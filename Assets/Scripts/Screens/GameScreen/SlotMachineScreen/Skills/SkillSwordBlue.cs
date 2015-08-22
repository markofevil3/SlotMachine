using UnityEngine;
using System.Collections;

public class SkillSwordBlue : Skill {

	public GameObject[] swordHits;

	public override void Init(int aLevel, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = aLevel;
		Vector3 toPos = bossManager.GetBossMiddlePoint();
		// Vector2 random = Random.insideUnitCircle / 6f;
		// toPos.y += random.y;
		// toPos.x += random.x;
		// transform.position = toPos;
		
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.1f);
		Invoke("Destroy", 1.5f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.2f);
		NGUITools.SetActive(swordHits[index], true);
	}

	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < swordHits.Length; i++) {
			NGUITools.SetActive(swordHits[i], false);
		}
		base.Destroy();
	}
}
