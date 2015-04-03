using UnityEngine;
using System.Collections;

public class Lighting : Skill {

	public virtual void Init(int level, int damage, BossManager bossManager) {
		size = new Vector3(120f, 120f, 1);
		transform.localScale = size;
		transform.position = ScreenManager.Instance.CurrentSlotScreen.userAvatarPanel.position;
		// TEST CODE - change to new boss position
		Vector3 target = bossManager.transform.position;
		// Rotate skill to look at target
		transform.rotation = Quaternion.Euler(0 , 0, Mathf.Atan2((target.y - transform.position.y), (target.x - transform.position.x))*Mathf.Rad2Deg);			
		this.bossManager = bossManager;
		this.damage = damage;
	}
	
	// public override void Explode() {
	// 	boss.GetHit(damage);
	// }
}
