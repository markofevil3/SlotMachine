using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIGridExtent : UIGrid {
  
  public bool doRepositionNow = false;
	
	public void Reset() {
		enabled = true;
		Start();
	}
	
	public override void  Update ()
	{
	  #if UNITY_EDITOR
		  if (doRepositionNow) {
  			Reset();
  			doRepositionNow = false;
  		}
		#endif
		base.Update();
	}
}
