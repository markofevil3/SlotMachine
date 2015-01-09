using UnityEngine;
using System.Collections;

public class SkillBite : Skill {

	public Transform bitePrefab;
	
	private float[] size = new float[3] { 0.25f, 0.4f, 0.55f };
	private int attackDamage;
	private int level;

	public override void Init(int level, int damage, Boss boss) {
		this.boss = boss;
		this.attackDamage = damage;
		this.level = level;
		SpawnParticle();
		transform.position = boss.middlePoint.position;
		StartCoroutine("CheckIfAlive");
		StartCoroutine("BossGetHit");
	}
	
	void SpawnParticle() {
		Transform bite = Instantiate(bitePrefab) as Transform;
		bite.parent = transform;
		bite.position = Vector3.zero;
		bite.localPosition = new Vector3(0, 0, 0);
		bite.GetComponent<ParticleSystem>().startSize = size[level - 1];
	}

	IEnumerator BossGetHit() {
		yield return new WaitForSeconds(0.3f);
		boss.Shake();
		boss.GetHit(attackDamage);
	}

	IEnumerator CheckIfAlive () {
		while(true) {
			yield return new WaitForSeconds(0.5f);
			if (transform.childCount == 0) {
				GameObject.Destroy(this.gameObject);
			}
		}
	}
}
