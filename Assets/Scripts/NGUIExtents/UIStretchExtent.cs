using UnityEngine;
using System.Collections;

public class UIStretchExtent : UIStretch {
  
  public string cameraTag = string.Empty;
  
  public override void Start() {
		if ((uiCamera == null && cameraTag != string.Empty) || (uiCamera != null && cameraTag != string.Empty && uiCamera.gameObject.tag != cameraTag)) {
		  uiCamera = GameObject.FindWithTag(cameraTag).GetComponent<Camera>();
		}
		base.Start();
  }
  
  public void Reset() {
		enabled = true;
		Awake();
		Start();
	}
  
}
