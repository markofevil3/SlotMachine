using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {

  public UILabel label;
  
  public void SetText(string text) {
    label.text = text;
  }
  
  public void Destroy() {
    Destroy(gameObject);
  }
  
}
