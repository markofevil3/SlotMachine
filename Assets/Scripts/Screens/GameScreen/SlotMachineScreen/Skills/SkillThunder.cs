using UnityEngine;
using System.Collections;

public class SkillThunder : Skill {

	public Transform[] particles;
		
	public override void Init(int level, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		for (int i = 0; i < level; i++) {
			StartCoroutine(SpawnParticle(i, 0.2f * i));
		}
		transform.position = bossManager.middlePoint.position;
		// StartCoroutine("CheckIfAlive");
		bossManager.Shake();
		bossManager.GetHit(damage);
		base.Init();
	}
	
	IEnumerator SpawnParticle(int index, float delay) {
		yield return new WaitForSeconds(delay);
		Transform thunder = particles[index];
		thunder.gameObject.SetActive(true);
		thunder.localPosition = new Vector3(60f * (index - 1f), Random.Range(-70f, 70f), 0);
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
