using UnityEngine;
using System.Collections;

public class Lighting : Skill {

	public override void Init(Boss boss, int damage, Vector3 startPos) {
		size = new Vector3(120f, 120f, 1);
		transform.localScale = size;
		transform.position = startPos;
		Vector3 target = boss.bossSprite.transform.position;
		// Rotate skill to look at target
		transform.rotation = Quaternion.Euler(0 , 0, Mathf.Atan2((target.y - transform.position.y), (target.x - transform.position.x))*Mathf.Rad2Deg);			
		this.boss = boss;
		this.damage = damage;
	}
	
	public override void Explode() {
		boss.GetHit(damage);
	}
}
