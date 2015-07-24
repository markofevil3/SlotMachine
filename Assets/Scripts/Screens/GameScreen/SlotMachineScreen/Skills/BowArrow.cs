using UnityEngine;
using System.Collections;

public class BowArrow : MonoBehaviour {

	public GameObject arrow;
	public GameObject arrowHit;
	public GameObject arrowHitEffect;
	
	public void Shoot(Vector3 toPos, Vector3 fromPos) {
		NGUITools.SetActive(arrow, true);
		NGUITools.SetActive(arrowHit, false);
		NGUITools.SetActive(arrowHitEffect, false);
		transform.position = fromPos;
		Vector2 random = Random.insideUnitCircle / 8f;
		toPos.y += random.y;
		toPos.x += random.x;
    transform.rotation = Quaternion.LookRotation(Vector3.forward, toPos - transform.position);
		// LeanTween.moveY(gameObject, bossManager.GetBossMiddlePoint().y + random.y, 0.3f).setEase(LeanTweenType.easeInExpo).setOnComplete(Explode);
		LeanTween.move(gameObject, toPos, 0.4f).setEase(LeanTweenType.easeInExpo).setOnComplete(Explode);
	}
	
	void Explode() {
		NGUITools.SetActive(arrow, false);
		NGUITools.SetActive(arrowHit, true);
		NGUITools.SetActive(arrowHitEffect, true);
	}
	
	public void Destroy() {
		NGUITools.SetActive(gameObject, false);
	}
	
}
