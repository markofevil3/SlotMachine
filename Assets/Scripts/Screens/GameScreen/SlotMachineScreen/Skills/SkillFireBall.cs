using UnityEngine;
using System.Collections;

public class SkillFireBall : Skill {

	public Transform fireBallPrefab;
	public Transform explodePrefab;
	private Transform ball;
	private Transform explode;
		
	public override void Init(int level, int damage, Boss boss) {
		this.boss = boss;
		SpawnParticle();
		// transform.position = ScreenManager.Instance.CurrentSlotScreen.userAvatarPanel.position;

	}
	
	void SpawnParticle() {
		ball = Instantiate(fireBallPrefab, ScreenManager.Instance.CurrentSlotScreen.userAvatarPanel.position, Quaternion.Euler(-90f,0,0)) as Transform;
		ball.parent = transform;
		LeanTween.move(ball.gameObject, boss.middlePoint.position, 0.8f).setEase(LeanTweenType.easeInQuad).setOnComplete(PreExplode);
	}
	
	void PreExplode() {
		StartCoroutine(Explode());
	}
	
	IEnumerator Explode() {
		explode = Instantiate(explodePrefab, boss.middlePoint.position, Quaternion.Euler(-90f,0,0)) as Transform;
		explode.parent = transform;
		yield return null;
		GameObject.Destroy(ball.gameObject);
		boss.Shake();
		boss.GetHit(damage);
		StartCoroutine("CheckIfAlive");
		// GameObject.Destroy(this.gameObject);
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
