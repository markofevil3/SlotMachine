using UnityEngine;
using System.Collections;

public class LuffyHand : MonoBehaviour {

	private float iter;
	public LeanTweenPath ltPath;
	public GameObject hand;
	public GameObject hitEffect;
	public float delay;

	private LTSpline s;

	void Start () {
		s = new LTSpline( ltPath.splineVector() );
		LeanTween.moveSpline( hand, s.pts, .4f).setDelay(delay).setOnComplete(HitTarget);
	}
	
	void HitTarget() {
		Debug.Log("HitTarget");
		hitEffect.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y + 0.15f, hand.transform.position.z);
		hitEffect.SetActive(true);
		StartCoroutine(HideHand());
	}
	
	IEnumerator HideHand() {
		yield return new WaitForSeconds(0.2f);
		hand.SetActive(false);
	}
}
