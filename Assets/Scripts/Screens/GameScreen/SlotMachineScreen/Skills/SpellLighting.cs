using UnityEngine;
using System.Collections;

public class SpellLighting : MonoBehaviour {

	public GameObject lightBall;
	public GameObject lightBallHit;
	
	Vector3 toPos;
	
	public void Shoot(Vector3 mToPos, Vector3 fromPos) {
		transform.position = fromPos;
		NGUITools.SetActive(lightBall, true);
		NGUITools.SetActive(lightBallHit, false);
		Vector2 random = Random.insideUnitCircle / 6f;
		mToPos.y += random.y;
		mToPos.x += random.x;
		this.toPos = mToPos;
		LeanTween.move(gameObject, toPos, 0.5f).setOnComplete(Explode);
	}
	
	void Explode() {
		lightBallHit.transform.position = toPos;
		NGUITools.SetActive(lightBallHit, true);
		StartCoroutine(ExplodeCallback());
	}

	IEnumerator ExplodeCallback() {
		yield return new WaitForSeconds(0.1f);
		NGUITools.SetActive(lightBall, false);
	}
	
	public void Destroy() {
		NGUITools.SetActive(gameObject, false);
	}
}
