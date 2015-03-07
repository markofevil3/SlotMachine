#pragma strict

public var vehiclesPrefabs:GameObject[];
public var roadPaths:LeanTweenPath[];

private var i:int;
private var j:int;

function Start () {
	var carsPerRoad:float = 15.0;

	for(i = 0; i < roadPaths.Length; i++){
		var ltPath:LTBezierPath = new LTBezierPath(roadPaths[i].vec3);
		carsPerRoad = ltPath.length * 0.015*1.5;
		
		// print("len:"+ltPath.length+" carsPerRoad:"+carsPerRoad+" isRound:"+isRound);
		if(roadPaths[i]){
			for(j = 0; j < carsPerRoad; j++){
				var car:GameObject = GameObject.Instantiate(vehiclesPrefabs[Random.Range(0,vehiclesPrefabs.Length)], transform.position, transform.rotation );
				car.AddComponent(BusCarControl);
				car.GetComponent(BusCarControl).init( j*1.0/carsPerRoad + Random.Range(0.0, 0.8/carsPerRoad), ltPath, this);


			}
		}
	}

}

