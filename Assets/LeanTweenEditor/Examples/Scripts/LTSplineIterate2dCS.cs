using UnityEngine;
using System.Collections;

public class LTSplineIterate2dCS : MonoBehaviour {

	private float iter;
	public LeanTweenPath ltPath;
	public GameObject ltLogo;

	private LTSpline s;

	void Start () {
		s = new LTSpline( ltPath.splineVector() );
		LeanTween.moveSpline( ltLogo, s.pts, .6f).setEase(LeanTweenType.easeOutQuad);
	}
	
	// void Update () {
	// 	// s.place2d( ltLogo.transform, iter );
	// 	ltLogo.transform.position = s.point(iter);
	// 	iter += Time.deltaTime * 0.5f;
	// 	if(iter>1f)
	// 		iter = 0f;
	// }
}
