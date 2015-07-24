using UnityEngine;
using System.Collections;

public class SpellFire : MonoBehaviour {

	public GameObject fireBall;
	public GameObject fireBallHit;
	
	Vector3 toPos;
	
	public void Shoot(Vector3 mToPos, Vector3 fromPos) {
		transform.position = fromPos;
		NGUITools.SetActive(fireBall, true);
		NGUITools.SetActive(fireBallHit, false);
		Vector2 random = Random.insideUnitCircle / 6f;
		mToPos.y += random.y;
		mToPos.x += random.x;
		this.toPos = mToPos;
		LeanTween.move(gameObject, toPos, 0.5f).setOnComplete(Explode);
	}
	
	void Explode() {
		fireBallHit.transform.position = toPos;
		NGUITools.SetActive(fireBallHit, true);
		StartCoroutine(ExplodeCallback());
	}

	IEnumerator ExplodeCallback() {
		yield return new WaitForSeconds(0.1f);
		NGUITools.SetActive(fireBall, false);
	}
	
	public void Destroy() {
		NGUITools.SetActive(gameObject, false);
	}
}
