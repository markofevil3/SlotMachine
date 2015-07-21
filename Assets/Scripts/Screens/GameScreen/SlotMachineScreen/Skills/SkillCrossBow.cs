using UnityEngine;
using System.Collections;

public class SkillCrossBow : Skill {

	public GameObject arrow;
	public GameObject arrowHit;
	public GameObject arrowHitEffect;
	public GameObject bloodEffect;

	public override void Init(int level, int damage, BossManager bossManager, Vector3 fromPos) {
		this.bossManager = bossManager;
		this.damage = damage;
		this.level = level;
		NGUITools.SetActive(arrow, true);
		NGUITools.SetActive(arrowHit, false);
		NGUITools.SetActive(arrowHitEffect, false);
		NGUITools.SetActive(bloodEffect, false);
		transform.position = fromPos;
		Vector2 random = Random.insideUnitCircle / 6f;
		Vector3 toPos = bossManager.GetBossMiddlePoint();
		toPos.y += random.y;
		toPos.x += random.x;
    transform.rotation = Quaternion.LookRotation(Vector3.forward, toPos - transform.position);
		// LeanTween.moveY(gameObject, bossManager.GetBossMiddlePoint().y + random.y, 0.3f).setEase(LeanTweenType.easeInExpo).setOnComplete(Explode);
		LeanTween.move(gameObject, toPos, 0.3f).setEase(LeanTweenType.easeInExpo).setOnComplete(Explode);
		Invoke("Destroy", 1f);
	}
	
	void Explode() {
		NGUITools.SetActive(arrow, false);
		NGUITools.SetActive(arrowHit, true);
		NGUITools.SetActive(arrowHitEffect, true);
		NGUITools.SetActive(bloodEffect, true);
	}
}
