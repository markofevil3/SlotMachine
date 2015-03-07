#pragma strict

public var ltPath:LeanTweenPath;
private var ltBezierPath:LTBezierPath;
private var iter:float;

function Start () {
	ltBezierPath = new LTBezierPath(ltPath.vec3);
}

function Update () {
	transform.position = ltBezierPath.point( iter );
	iter += Time.deltaTime;
	if(iter>1.0)
		iter = 0.0;
}