using UnityEngine;
using System.Collections;

public class HUDText : MonoBehaviour {
  
  public UILabel label;
  
  public void Init(string text, int fontSize, Color color, int depth) {
    label.text = text;
  	label.color = color;
  	label.fontSize = fontSize;
  	label.depth = depth;
  }
  
  public void Destroy() {
    Destroy(gameObject);
  }
}
