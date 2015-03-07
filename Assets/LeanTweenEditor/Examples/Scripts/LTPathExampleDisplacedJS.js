#pragma strict

public var ltPath:LeanTweenPath;

private var lt:GameObject;
private var ltHolder:GameObject;
private var missile1:GameObject;
private var missile2:GameObject;
private var missile3:GameObject;

function Start () {
	lt = GameObject.Find("LeanTweenAvatar");
	ltHolder = GameObject.Find("LeanTweenAvatarHolder");
	missile1 = GameObject.Find("Missile1");
	missile2 = GameObject.Find("Missile2");
	missile3 = GameObject.Find("Missile3");

	fireMissile1();
}

function Update () {
	ltHolder.transform.eulerAngles.y += Time.deltaTime * 30;
}

function fireMissile1(){
	LeanTween.move(missile1, LeanTween.add( ltPath.vec3, lt.transform.position), 2.5).setOrientToPath(true).setDelay(0f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(fireMissile1);
}
