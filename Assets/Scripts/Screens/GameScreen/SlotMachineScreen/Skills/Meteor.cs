using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

	public GameObject meteor;
	public GameObject meteorHit;
	
	Vector3 toPos;
	
	public void Shoot(Vector3 mToPos, Vector3 fromPos) {
		transform.position = fromPos;
		NGUITools.SetActive(meteor, true);
		NGUITools.SetActive(meteorHit, false);
		Vector2 random = Random.insideUnitCircle / 4f;
		mToPos.y += random.y;
		mToPos.x += random.x;
		this.toPos = mToPos;
		LeanTween.move(gameObject, toPos, 0.6f).setOnComplete(Explode);
	}
	
	void Explode() {
		meteorHit.transform.position = toPos;
		NGUITools.SetActive(meteorHit, true);
		StartCoroutine(ExplodeCallback());
	}

	IEnumerator ExplodeCallback() {
		yield return new WaitForSeconds(0.1f);
		NGUITools.SetActive(meteor, false);
	}
	
	public void Destroy() {
		NGUITools.SetActive(gameObject, false);
	}
	
}
