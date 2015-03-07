using UnityEngine;
using System.Collections;

[AddComponentMenu("LeanTween/LeanTweenPath")]

public class LeanTweenPath:MonoBehaviour {
	public int count;
	
	public Transform[] pts;
	public Vector3[] path;
	public enum LeanTweenPathType{
		bezier,
		spline
	}
	public LeanTweenPathType pathType;
	public float controlSize = 1f;

	public bool nodesMaximized = true;
	public bool creatorMaximized = false;
	public bool importMaximized = false;

	private int i;
	private int k;
	public int lastCount = 1;

	public static Color curveColor;
	public static Color lineColor;

	private void init(){
		
		if(path!=null && path.Length!=0 && (pts == null || pts.Length == 0)){ // transfer over paths made with the legacy path variable
			for (i=transform.childCount-1; i>=0; i--) {
				DestroyImmediate( transform.GetChild(i).gameObject );
			}
			pts = new Transform[ path.Length ];
			for(i=0;i<path.Length;i++){
				if(i>3 && i%4==0){
					
				}else{
					pts[i] = createChild(i, path[i]);
					// Debug.Log("creating i:"+i+" at:"+path[i]);
				}
			}
			reset();
			path = new Vector3[0];
			lastCount = count = 1;
		}

		if(pts == null || pts.Length == 0){ // initial creation
			if(pathType==LeanTweenPathType.bezier){
				pts = new Transform[]{createChild(0, new Vector3(0f,0f,0f)), createChild(1, new Vector3(5f,0f,0f)), createChild(2, new Vector3(4f,0f,0f)), createChild(3, new Vector3(5f,5f,0f))};
			}else{
				pts = new Transform[]{createChild(0, new Vector3(0f,0f,0f)), createChild(1, new Vector3(2f,0f,0f)), createChild(2, new Vector3(3f,2f,0f))};
			}
			reset();
			//lastCount = count = 1;
		}

		// Debug.Log("count:"+count+" lastCount:"+lastCount);
		if(lastCount!=count){ // A curve must have been added or subtracted
			
			if(pathType==LeanTweenPathType.bezier){ // BEZIER
				Vector3 lastPos = Vector3.zero;
				Transform[] newPts = new Transform[ count ];

				if(lastCount>count){ // remove unused points
					//Debug.Log("removing stuff count:"+count);
					k = 0;
					for(i=0;i<pts.Length;i++){ // loop through to see if it was any of the in-between nodes deleted (otherwise it will just delete it from the end)
						if(i%4!=0 && pts[i]==null){
							// Debug.Log("deleting i:"+i);
							DestroyImmediate( pts[i-1].gameObject );
							DestroyImmediate( pts[i-2].gameObject );
							i += 1;
							k += -2;
						}else if(k < newPts.Length){
							// Debug.Log("k:"+k+" i:"+i);
							newPts[k] = pts[i];
							//Debug.Log("transfer over:"+k);
							initNode(newPts[k],k);
							k++;
							if(pts[i])
								lastPos = pts[i].localPosition;
						}
					}
				}else{
					int lastI = 0;
					for(i=0;i<newPts.Length;i++){
						if(i<pts.Length){ // transfer old points
							newPts[i] = pts[i];
							if(pts[i])
								lastPos = pts[i].localPosition;
							lastI = i;
						}else{ // add in a new point
							k = pts.Length + i;
							// Debug.Log("adding new "+k);

							Vector3 addPos = new Vector3(5f,5f,0f);
							if(i>=2){
								Vector3 one = newPts[lastI].localPosition;
								Vector3 two = newPts[lastI-2].localPosition;
								
								addPos = one - two;
								if(addPos.magnitude<controlSize*2f){
									addPos = addPos.normalized*controlSize*2f;
								}
							}

							if(i%4==1){
								newPts[i] = createChild(k, lastPos+addPos*0.6f);
							}else if(i%4==2){
								newPts[i] = createChild(k, lastPos+addPos*0.3f);
							}else if(i%4==3){
								newPts[i] = createChild(k, lastPos+addPos);
							}
						}
					}
				}

				pts = newPts;
			}else{ // SPLINE
				Transform[] newPts = new Transform[ count ];
				k = 0;
				for(i=0; i<pts.Length; i++){ // Loop over points to find Transforms that have been deleted
					if(pts[i]!=null){
						if(k<newPts.Length){
							newPts[k] = pts[i];
							initNode(newPts[k], k);
							k++;
						}
					}
				}
				
				k = 0;
				if(count>lastCount){ // Add in new points
					int diff =  count - lastCount;
					// Debug.Log("adding in point diff:"+diff);
					for (i=0; i<diff; i++) {
						k = pts.Length + i;
						// Debug.Log("new k:"+k+" newPts.Length-1:"+(newPts.Length-1));
						if(k <= newPts.Length-1){
							Vector3 addPos = new Vector3(1f,1f,0f);
							if(k>=2){
								Vector3 diff3 = newPts[k-1].localPosition - newPts[k-2].localPosition;
								addPos = newPts[k-1].localPosition + diff3;
							}
							newPts[k] = createChild(k,addPos);
						}
					}
				}
				pts = newPts;
			}
			lastCount = count;
		}

		reset();
	}

	private void reset(){
		if(pathType==LeanTweenPathType.bezier){
			for(i=0;i<pts.Length;i++){
				LeanTweenPathControl[] ct = new LeanTweenPathControl[2];
				if(i%4==0){
					if( i+2 < pts.Length && pts[i+2] )
						ct[0] = pts[i+2].gameObject.GetComponent<LeanTweenPathControl>();
				}else if(i%4==3){
					ct[0] = gameObject.GetComponent<LeanTweenPathControl>();
					if(i+3<pts.Length && pts[i+3])
						ct[1] = pts[i+3].gameObject.GetComponent<LeanTweenPathControl>();
				}

				if(pts[i]){
					LeanTweenPathControl pathControl = pts[i].gameObject.GetComponent<LeanTweenPathControl>();
					pathControl.init( i%4==0||i%4==3, ct);
				}
			}
		}else{
			for(i=0;i<pts.Length;i++){
				LeanTweenPathControl[] ct = new LeanTweenPathControl[2];
				if(pts[i]){
					LeanTweenPathControl pathControl = pts[i].gameObject.GetComponent<LeanTweenPathControl>();
					pathControl.init( ct );
				}
			}
		}
		this.count = this.lastCount = pts.Length;
	}

	public Transform createChild(int i, Vector3 pos ){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.AddComponent<LeanTweenPathControl>();
		Transform trans = go.transform;
		DestroyImmediate( go.GetComponent<BoxCollider>() );
		trans.parent = transform;
		initNode(trans, i);
		// trans.name = "pt"+i;
		trans.localPosition = pos;
	
		return trans;
	}

	private void initNode( Transform trans, int i ){
		if(trans!=null){
			if(pathType==LeanTweenPathType.bezier){
				int iMod = i%4;
				bool isPoint = iMod==0||iMod==3;
				string type = isPoint ? "Node" : "Control";
				// Debug.Log("setting i:"+i);
				int ptArea = (i/8)+1;
				if(i==3)
					ptArea = 1;
				string cntrlNum = "";
				if(i<8)
					ptArea = (i+1)/4;
				if(isPoint==false){
					cntrlNum = iMod==2 ? "-0" : "-1";
					trans.localScale = new Vector3(0.5f,0.5f,0.25f);
				}else{
					trans.localScale = Vector3.one * 0.5f;
				}
				trans.name = /*"path"+Mathf.Floor(i/4)+"-"+t*/type+ptArea+cntrlNum;
			}else{
				trans.localScale = Vector3.one * 0.25f;
				trans.name = "path"+i;
			}
		}
	}

	void Start () {
		init();
		for(i=0; i < pts.Length; i++){
			if(pts[i])
				pts[i].gameObject.SetActive(false);
		}
	}

	public void OnDrawGizmos(){
		init();
		
		if(pathType==LeanTweenPathType.bezier){
			for(int i = 0; i < pts.Length-3; i += 4){
				if(pts[i+1] && pts[i+2] && pts[i+3]){
					Vector3 first = i>3 ? pts[i-1].position : pts[i].position;
					
					Gizmos.color = Color.magenta;
					LeanTween.drawBezierPath(first, pts[i+2].position, pts[i+1].position, pts[i+3].position);
					
					Gizmos.color = Color.white;
					Gizmos.DrawLine(first,pts[i+2].position);
					Gizmos.DrawLine(pts[i+1].position,pts[i+3].position);
				}
			}
			for(int i = 0; i < pts.Length; i++){
				int iMod = i%4;
				bool isPoint = iMod==0||iMod==3;
				if(pts[i])
					pts[i].localScale = isPoint ? Vector3.one * controlSize * 0.5f : new Vector3(1f,1f,0.5f) * controlSize * 0.5f;
			}
		}else{
			for(i=0;i<pts.Length;i++){
				if(pts[i]){
					pts[i].localScale = Vector3.one * controlSize * 0.25f;
				}
			}
			LTSpline s = new LTSpline( splineVector() );
			Gizmos.color = Color.magenta;
			s.gizmoDraw();
		}
	}

	public Vector3[] splineVector(){
		Vector3[] p = new Vector3[ pts.Length + 2 ];
		int k = 0;
		for(int i = 0; i < p.Length; i++){
			if(pts[k]){
				p[i] = pts[k].position;
				// Debug.Log("k:"+k+" i:"+i);
				if(i>=1 && i < p.Length-2){
					k++;
				}
			}
		}
		return p;
	}

	public Vector3[] vec3{
		get{
			if(pathType==LeanTweenPathType.bezier){
				Vector3[] p = new Vector3[ pts.Length ];
				for(i=0; i < p.Length; i++){
					p[i] = i>3 && i%4==0 ? pts[i-1].position : pts[i].position;
				}
				return p;
			}else{
				return splineVector();
			}
		}
		set{

		}
	}
}