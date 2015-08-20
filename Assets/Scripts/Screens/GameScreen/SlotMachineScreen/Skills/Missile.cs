using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	public GameObject missile;
	public GameObject missileHit;
	
	Vector3 toPos;
	
	public void Shoot(Vector3 mToPos, Vector3 fromPos) {
		// transform.position = fromPos;
		NGUITools.SetActive(missileHit, false);
		Vector2 random = Random.insideUnitCircle / 4f;
		Vector3 parallelDirection = fromPos - mToPos;
		mToPos.y += random.y;
		mToPos.x += random.x;
		this.toPos = mToPos;
		transform.position = parallelDirection + toPos;
		NGUITools.SetActive(missile, true);
		LeanTween.move(gameObject, toPos, 0.6f).setOnComplete(Explode);
	}
	
	void Explode() {
		missileHit.transform.position = toPos;
		NGUITools.SetActive(missile, false);
		NGUITools.SetActive(missileHit, true);
		StartCoroutine(ExplodeCallback());
	}

	IEnumerator ExplodeCallback() {
		yield return new WaitForSeconds(0.1f);
	}
	
	public void Destroy() {
		NGUITools.SetActive(missileHit, false);
		NGUITools.SetActive(missile, false);
		NGUITools.SetActive(gameObject, false);
	}
	
}
