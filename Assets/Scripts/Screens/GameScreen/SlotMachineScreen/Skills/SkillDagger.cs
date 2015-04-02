using UnityEngine;
using System.Collections;

public class SkillDagger : Skill {

	public Transform[] particles;
	
	private float[] rotation = new float[3] { 90f, 60f, 130f };

	public override void Init(int level, int damage, Boss boss) {
		this.boss = boss;
		for (int i = 0; i < level; i++) {
			StartCoroutine(SpawnParticle(i, 0.2f * i));
		}
		transform.position = boss.middlePoint.position;
		// StartCoroutine("CheckIfAlive");
		boss.Shake();
		boss.GetHit(damage);
		base.Init();
	}
	
	IEnumerator SpawnParticle(int index, float delay) {
		yield return new WaitForSeconds(delay);
		Transform sword = particles[index];
		sword.gameObject.SetActive(true);
		// sword.parent = transform;
		// sword.position = Vector3.zero;
		// sword.localPosition = new Vector3(0, 0, 0);
		sword.GetComponent<ParticleSystem>().startRotation = rotation[index] * Mathf.Deg2Rad;
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
