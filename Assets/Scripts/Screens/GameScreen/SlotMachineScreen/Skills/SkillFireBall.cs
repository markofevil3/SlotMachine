using UnityEngine;
using System.Collections;

public class SkillFireBall : Skill {

	public Transform fireBallPrefab;
	public Transform explodePrefab;
		
	public override void Init(int level, int damage, Boss boss) {
		this.boss = boss;
		this.damage = damage;
		explodePrefab.gameObject.SetActive(false);
		SpawnParticle();
		base.Init();
		// transform.position = ScreenManager.Instance.CurrentSlotScreen.userAvatarPanel.position;

	}
	
	void SpawnParticle() {
		fireBallPrefab.position = ScreenManager.Instance.CurrentSlotScreen.userAvatarPanel.position;
		fireBallPrefab.gameObject.SetActive(true);
		LeanTween.move(fireBallPrefab.gameObject, boss.middlePoint.position, 0.8f).setEase(LeanTweenType.easeInQuad).setOnComplete(PreExplode);
	}
	
	void PreExplode() {
		StartCoroutine(Explode());
	}
	
	IEnumerator Explode() {
		explodePrefab.position = boss.middlePoint.position;
		explodePrefab.gameObject.SetActive(true);
		yield return null;
		fireBallPrefab.gameObject.SetActive(false);
		boss.Shake();
		boss.GetHit(damage);
		// GameObject.Destroy(this.gameObject);
	}
}
