using UnityEngine;
using System.Collections;

public class SkillSwordBlue : Skill {

	private Transform particle;

	public override void Init(int level, int damage, BossManager bossManager) {
		this.bossManager = bossManager;
		this.level = level;
		SpawnParticle(level);
		transform.position = bossManager.GetBossMiddlePoint();
		// StartCoroutine("CheckIfAlive");
		bossManager.Shake();
		bossManager.GetHit(damage);
		base.Init();
	}
	
	void SpawnParticle(int level) {
		particle = MyPoolManager.Instance.Spawn("IceSlash" + level, transform);
	}
	
	public override void Destroy() {
		MyPoolManager.Instance.Despawn(particle);
		base.Destroy();
	}
}
