using UnityEngine;
using System.Collections;

public class PopupLoading : MonoBehaviour {
  
  public void Open() {
    
  }
  
  public void Close() {
    TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Set(tween.onFinished, Destroy);
  }
  
  void Destroy() {
    Destroy(gameObject);
  }
}
