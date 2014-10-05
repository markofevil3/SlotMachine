using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UITableExtent : UITable {
  
  public bool doRepositionNow = false;
	
	public void Reset() {
		enabled = true;
		Start();
	}
	
	public override void LateUpdate ()
	{
	  #if UNITY_EDITOR
		  if (doRepositionNow) {
  			Reset();
  			doRepositionNow = false;
  		}
		#endif
		base.LateUpdate();
	}
  
}
