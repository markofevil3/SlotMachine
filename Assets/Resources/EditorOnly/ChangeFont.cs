using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

public class ChangeFont : MonoBehaviour {

  public UIFont newfont;
  public bool begin = false;
  
  void Update() {
    if (begin) {
      UILabel[] labels = gameObject.GetComponentsInChildren<UILabel>();
  		foreach (UILabel label in labels) {
        label.bitmapFont = newfont;
  		}
  		begin = false;
    }
  }
}