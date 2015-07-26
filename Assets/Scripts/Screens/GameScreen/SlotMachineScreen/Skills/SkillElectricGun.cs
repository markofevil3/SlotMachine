using UnityEngine;
using System.Collections;

public class SkillElectricGun : Skill {

	public GameObject[] electricHit;

	public override void Init(int aLevel, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = aLevel;
		for (int i = 0; i < level; i++) {
			StartCoroutine(Shoot(i));
		}
		Invoke("BossGetHit", 0.1f);
		Invoke("Destroy", 1.5f);
	}
	
	IEnumerator Shoot(int index) {
		yield return new WaitForSeconds(index * 0.1f);
		Vector3 toPos = bossManager.GetBossMiddlePoint();
		Vector2 random = Random.insideUnitCircle / 4f;
		toPos.y += random.y;
		toPos.x += random.x;		
		electricHit[index].transform.position = toPos;
		NGUITools.SetActive(electricHit[index], true);
	}

	void BossGetHit() {
		bossManager.Shake();
		bossManager.GetHit(damage);
	}
	
	public override void Destroy() {
		for (int i = 0; i < electricHit.Length; i++) {
			NGUITools.SetActive(electricHit[i], false);
		}
		base.Destroy();
	}
}
