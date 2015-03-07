#pragma strict

public var editorPath:LeanTweenPath;

private var lt1:GameObject;
private var lt2:GameObject;

private var ltPath1:LTBezierPath;
private var ltPath2:LTBezierPath;

private var iter1:float = 0.25;
private var iter2:float = 0.5;

function Start () {
	lt1 = GameObject.Find("LeanTweenAvatar1");
	lt2 = GameObject.Find("LeanTweenAvatar2");

	ltPath1 = new LTBezierPath(editorPath.vec3);
	ltPath2 = new LTBezierPath(editorPath.vec3);
}

function Update () {
	ltPath1.place( lt1.transform, iter1);
	lt2.transform.position = ltPath2.point( iter2 );

	iter1 += Time.deltaTime*0.1;
	if(iter1>1.0)
		iter1 = 0.0;
	iter2 += Time.deltaTime*0.1;
	if(iter2>1.0)
		iter2 = 0.0;
}
