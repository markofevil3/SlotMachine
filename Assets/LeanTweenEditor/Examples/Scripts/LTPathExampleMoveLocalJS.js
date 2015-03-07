#pragma strict

public var ltPath:LeanTweenPath;

private var lt:GameObject;
private var containingCube:Transform;

function Start () {
	lt = GameObject.Find("LeanTweenAvatar");
	containingCube = GameObject.Find("containingCube").transform;
	
	loopAroundPath();
}

function Update () {
	containingCube.eulerAngles.y += Time.deltaTime * 10; // rotate the parent object
}

function loopAroundPath(){
	LeanTween.moveLocal(lt, ltPath.vec3, 4.0).setOrientToPath(true).setDelay(1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(loopAroundPath);
}