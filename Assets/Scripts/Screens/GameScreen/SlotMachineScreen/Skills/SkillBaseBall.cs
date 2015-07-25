using UnityEngine;
using System.Collections;

public class SkillBaseBall : Skill {

	public GameObject[] baseBallHits;

	public override void Init(int aLevel, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = aLevel;
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.15f);
		Invoke("Destroy", 1.5f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.15f);
		Vector3 toPos = bossManager.GetBossMiddlePoint();
		Vector2 random = Random.insideUnitCircle / 6f;
		toPos.y += random.y;
		toPos.x += random.x;		
		baseBallHits[index].transform.position = toPos;
		NGUITools.SetActive(baseBallHits[index], true);
	}

	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < baseBallHits.Length; i++) {
			NGUITools.SetActive(baseBallHits[i], false);
		}
		base.Destroy();
	}
}
