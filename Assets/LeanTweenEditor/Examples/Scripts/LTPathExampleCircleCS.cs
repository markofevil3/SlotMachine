using UnityEngine;
using System.Collections;

public class LTPathExampleCircleCS : MonoBehaviour {

	public LeanTweenPath path;

	private GameObject lt;

	// Use this for initialization
	void Start () {
		lt = GameObject.Find("LeanTweenAvatar");
	
		loopAroundCircle();
	}
	
	void loopAroundCircle(){
		LeanTween.move(lt, path.vec3, 4.0f).setOrientToPath(true).setDelay(1.0f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(loopAroundCircle);
	}
}
