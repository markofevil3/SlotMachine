using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIAnchorExtent : UIAnchor {
  
  public string cameraTag = string.Empty;
  
  public override void Start() {
		if ((uiCamera == null && cameraTag != string.Empty) || (uiCamera != null && cameraTag != string.Empty && uiCamera.gameObject.tag != cameraTag)) {
		  uiCamera = GameObject.FindWithTag(cameraTag).GetComponent<Camera>();
		}
		base.Start();
  }
  
  public void Reset() {
		Awake();
		enabled = true;
		Start();
	}
  
}
