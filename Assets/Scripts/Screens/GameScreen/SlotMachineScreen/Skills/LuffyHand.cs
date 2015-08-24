using UnityEngine;
using System.Collections;

public class LuffyHand : MonoBehaviour {

	public GameObject hand;
	public GameObject hitEffect;
	
	public void Shoot(Vector3 toPos, Vector3 fromPos) {
		hand.SetActive(true);
		hitEffect.SetActive(false);
		transform.position = fromPos;
		Vector2 random = Random.insideUnitCircle / 6f;
		toPos.y += random.y;
		toPos.x += random.x;
    transform.rotation = Quaternion.LookRotation(Vector3.forward, toPos - transform.position);
		// LeanTween.moveY(gameObject, bossManager.GetBossMiddlePoint().y + random.y, 0.3f).setEase(LeanTweenType.easeInExpo).setOnComplete(Explode);
		LeanTween.move(gameObject, toPos, 0.3f).setOnComplete(Explode);
	}
	
	void Explode() {
		hitEffect.SetActive(true);
		StartCoroutine(ExplodeCallback());
	}

	IEnumerator ExplodeCallback() {
		yield return new WaitForSeconds(0.05f);
		hand.SetActive(false);
	}
	
	public void Destroy() {
		gameObject.SetActive(false);
	}

	// private float iter;
	// public LeanTweenPath ltPath;
	// public GameObject hand;
	// public GameObject hitEffect;
	// public float delay;
	//
	// private LTSpline s;
	// private BossManager bossManager;
	// private int damage;
	// private Vector3 oldPos;
	//
	// // void Start () {
	// //
	// // }
	//
	// public void Init(BossManager bossManager, int damage) {
	// 	this.bossManager = bossManager;
	// 	this.damage = damage;
	// 	oldPos = transform.position;
	// 	s = new LTSpline( ltPath.splineVector() );
	// 	LeanTween.moveSpline( hand, s.pts, .4f).setDelay(delay).setOnComplete(HitTarget);
	// }
	//
	// void HitTarget() {
	// 	hitEffect.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y + 0.15f, hand.transform.position.z);
	// 	hitEffect.SetActive(true);
	// 	StartCoroutine(HideHand());
	// }
	//
	// IEnumerator HideHand() {
	// 	yield return new WaitForSeconds(0.2f);
	// 	transform.position = oldPos;
	// 	hitEffect.SetActive(false);
	// 	hand.SetActive(false);
	// }
	
	
}
