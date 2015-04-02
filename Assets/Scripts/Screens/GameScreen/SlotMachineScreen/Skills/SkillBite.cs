using UnityEngine;
using System.Collections;

public class SkillBite : Skill {

	public Transform bitePrefab;
	
	private float[] biteSize = new float[3] { 0.25f, 0.4f, 0.55f };
	private int attackDamage;

	public override void Init(int aLevel, int damage, Boss boss) {
		this.boss = boss;
		this.attackDamage = damage;
		this.level = aLevel;
		SpawnParticle();
		transform.position = boss.middlePoint.position;
		// StartCoroutine("CheckIfAlive");
		StartCoroutine("BossGetHit");
		base.Init();
	}
	
	void SpawnParticle() {
		bitePrefab.gameObject.SetActive(true);
		bitePrefab.GetComponent<ParticleSystem>().startSize = biteSize[level - 1];
	}

	IEnumerator BossGetHit() {
		yield return new WaitForSeconds(0.3f);
		boss.Shake();
		boss.GetHit(attackDamage);
	}

	// IEnumerator CheckIfAlive () {
	// 	while(true) {
	// 		yield return new WaitForSeconds(0.5f);
	// 		if (transform.childCount == 0) {
	// 			GameObject.Destroy(this.gameObject);
	// 		}
	// 	}
	// }
}
