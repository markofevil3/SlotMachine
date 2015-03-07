#pragma strict

public var path:LeanTweenPath;

private var lt:GameObject;

function Start () {
	lt = GameObject.Find("LeanTweenAvatar");
	
	loopAroundCircle();

	var str:String = "";
	for(var i:int = 0; i < path.vec3.Length; i++){
		str += "new Vector3"+path.vec3[i]+",";
	}
	// Debug.Log(str);
}

private var halfCircle:Vector3[] = [new Vector3(26.2, -6.7, 66.4),new Vector3(24.0, -6.7, 78.9),new Vector3(26.1, -6.7, 73.6),new Vector3(18.9, -6.7, 84.0),new Vector3(18.9, -6.7, 84.0),new Vector3(8.4, -6.7, 91.3),new Vector3(13.7, -6.7, 89.1),new Vector3(1.2, -6.7, 91.4),new Vector3(1.2, -6.7, 91.4),new Vector3(-11.3, -6.7, 89.1),new Vector3(-6.1, -6.7, 91.3),new Vector3(-16.5, -6.7, 84.0),new Vector3(-16.5, -6.7, 84.0),new Vector3(-23.8, -6.7, 73.6),new Vector3(-21.6, -6.7, 78.9),new Vector3(-23.8, -6.7, 66.4)];

function Update () {

}

function loopAroundCircle(){
	LeanTween.move(lt, path.vec3, 4.0).setOrientToPath(true).setDelay(1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(loopAroundCircle);
}