using UnityEngine;
using System.Collections;

public class LuffyHand : MonoBehaviour {

	private float iter;
	public LeanTweenPath ltPath;
	public GameObject hand;
	public float delay;

	private LTSpline s;

	void Start () {
		s = new LTSpline( ltPath.splineVector() );
		LeanTween.moveSpline( hand, s.pts, .5f).setDelay(delay).setEase(LeanTweenType.easeOutQuad).setOnComplete(HitTarget);
	}
	
	void HitTarget() {
		Debug.Log("HitTarget");
	}
}
