#pragma strict
private var roadPath:LTBezierPath;

public enum BusCarState{
	dead, running, stopping, hit
}

private var state:int = 1;
private var carSpeed:double = 1.0;
private var carTopSpeed:double;
private var charId:int;
private static var carCounter:int;
private var ptIter:double;
private var carFleet:BusCarFleet;
private var meshFilter:MeshFilter;
private var camDistance:float;

public function init( startPt:float, roadPath:LTBezierPath, carFleet:BusCarFleet ){
	this.roadPath = roadPath;
	ptIter = startPt;
	this.carFleet = carFleet;
	
	carTopSpeed =  1000 / roadPath.length * 0.009;

	gameObject.name = "vehicle" + carCounter;
	charId = carCounter;
	carCounter++;
}

function distanceFromCamera():float{
	return (Camera.main.transform.position - transform.position).magnitude;
}

private var lastIter:float;
public var wasUpdated:boolean = false;

function Update () {
	wasUpdated = false;
	if(state == BusCarState.running /*&& renderer.isVisible*/){
		// print("Time.frameCount%10:"+(Time.frameCount%10)+" charId%10:"+(charId%10));
		if(camDistance<8000 || Time.frameCount%3==charId%3){
			if(GetComponent.<Renderer>().isVisible){
				wasUpdated = true;
				roadPath.place( transform, ptIter, Vector3.up);
			}else{
				transform.position = roadPath.point( ptIter );
			}
		}

		if(carSpeed<1.0)
			carSpeed += Time.deltaTime / 5.0;
		ptIter += Time.deltaTime * carTopSpeed * carSpeed;
		if(ptIter>1.0)
			ptIter = 0.0;
	}else if(state==BusCarState.dead){
		
	}

	if(state>0 && Time.frameCount%10==charId%10){
		var hitR : RaycastHit;
		var fwd:Vector3 = transform.TransformDirection(Vector3.forward + Vector3.up*0.007);
		if(Physics.Raycast(transform.position, fwd, hitR, 10.0)) {
			// if(gameObject.name.IndexOf("127"))
			// 	print("name:"+hitR.transform.gameObject.name + " bus?:"+(hitR.transform.gameObject.name.IndexOf("Player")>=0) + " car?:"+(hitR.transform.gameObject.name.IndexOf("vehicle")>=0) + " state:"+state);
			if(hitR.transform.gameObject.tag.IndexOf("Player")>=0 || hitR.transform.gameObject.name.IndexOf("vehicle")>=0){
	    		state=BusCarState.stopping;
	    		carSpeed = 0.0;
	    	}
	    }else{
	    	state=BusCarState.running;
	    }

	    Debug.DrawRay (transform.position, transform.TransformDirection ( Vector3.forward + Vector3.up*0.007 ) * 10.0, Color.red);
	}

}

function OnCollisionEnter(collision : Collision) {
	Debug.Log('collision.name:'+collision.gameObject.name);
	// if(state && collision.gameObject.name.IndexOf("Bus")>=0){
	// 	// Crashed
	// 	crash();

	// 	rigidbody.isKinematic = false;
	// 	rigidbody.useGravity = true;
	// }
}

function crash(){
	state = BusCarState.dead;
}