using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIViewportExtent : UIViewport {

  public string cameraTag = string.Empty;
  
  public override void Start() {
		if ((sourceCamera == null && cameraTag != string.Empty) || (sourceCamera != null && cameraTag != string.Empty && sourceCamera.gameObject.tag != cameraTag)) {
		  sourceCamera = GameObject.FindWithTag(cameraTag).GetComponent<Camera>();
		}
		base.Start();
  }

}
