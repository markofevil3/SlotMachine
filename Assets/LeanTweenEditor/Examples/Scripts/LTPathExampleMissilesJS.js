#pragma strict

public var ltPath:LeanTweenPath;

private var lt:GameObject;
private var missile1:GameObject;
private var missile2:GameObject;
private var missile3:GameObject;

function Start () {
	lt = GameObject.Find("LeanTweenAvatar");
	missile1 = GameObject.Find("Missile1");
	missile2 = GameObject.Find("Missile2");
	missile3 = GameObject.Find("Missile3");

	fireMissile1();
	LeanTween.delayedCall(gameObject, 0.5f, fireMissile2);
	LeanTween.delayedCall(gameObject, 1.0f, fireMissile3);
}

function Update () {
	lt.transform.eulerAngles.y += Time.deltaTime * 30;
}

function fireMissile1(){
	LeanTween.moveLocal(missile1, ltPath.vec3, 2.50).setOrientToPath(true).setDelay(0f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(fireMissile1);
}

function fireMissile2(){
	LeanTween.moveLocal(missile2, ltPath.vec3, 2.5).setOrientToPath(true).setDelay(0f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(fireMissile2);
}

function fireMissile3(){
	LeanTween.moveLocal(missile3, ltPath.vec3, 2.5).setOrientToPath(true).setDelay(0f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(fireMissile3);
}