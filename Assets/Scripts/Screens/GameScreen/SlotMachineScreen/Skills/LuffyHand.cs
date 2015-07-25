using UnityEngine;
using System.Collections;

public class LuffyHand : MonoBehaviour {

	private float iter;
	public LeanTweenPath ltPath;
	public GameObject hand;
	public GameObject hitEffect;
	public float delay;

	private LTSpline s;
	private BossManager bossManager;
	private int damage;
	private Vector3 oldPos;

	// void Start () {
	//
	// }
	
	public void Init(BossManager bossManager, int damage) {
		this.bossManager = bossManager;
		this.damage = damage;
		oldPos = transform.position;
		s = new LTSpline( ltPath.splineVector() );		
		LeanTween.moveSpline( hand, s.pts, .4f).setDelay(delay).setOnComplete(HitTarget);
	}
	
	void HitTarget() {
		hitEffect.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y + 0.15f, hand.transform.position.z);
		hitEffect.SetActive(true);
		StartCoroutine(HideHand());
	}
	
	IEnumerator HideHand() {
		yield return new WaitForSeconds(0.2f);
		transform.position = oldPos;
		hitEffect.SetActive(false);
		hand.SetActive(false);
	}
}
