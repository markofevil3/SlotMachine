using UnityEngine;
using System.Collections;

public class SkillThunder : Skill {

	public Transform thunderPrefab;
		
	public override void Init(int level, int damage, Boss boss) {
		Debug.Log("SkillThunder " + level + " " + damage);
		this.boss = boss;
		for (int i = 0; i < level; i++) {
			StartCoroutine(SpawnParticle(i, 0.2f * i));
		}
		transform.position = boss.middlePoint.position;
		StartCoroutine("CheckIfAlive");
		boss.Shake();
		boss.GetHit(damage);
	}
	
	IEnumerator SpawnParticle(int index, float delay) {
		yield return new WaitForSeconds(delay);
		Transform thunder = Instantiate(thunderPrefab) as Transform;
		thunder.parent = transform;
		thunder.position = Vector3.zero;
		thunder.localPosition = new Vector3(60f * (index - 1f), Random.Range(-70f, 70f), 0);
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
