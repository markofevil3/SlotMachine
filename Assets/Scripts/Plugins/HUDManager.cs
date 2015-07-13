using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

  public static HUDManager Instance { get; private set; }
  public GameObject root;

  private string flyTextPath = "Prefabs/Others/HUDFlyText";
  private int flyTextDepth = 40;
  
	void Start() {
	  Instance = this;
	}
  
  // Text will be on top popup and screen
  public HUDText AddFlyText(string text, Vector3 localPosition, int fontSize = 40, Color? c = null, float flyDuration = 0.4f, float stayTime = 1f) {
    Color color = c.HasValue ? (Color)c : Color.white;
    GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(flyTextPath, typeof(GameObject)) as GameObject);
    HUDText hudText = tempGameObject.GetComponent<HUDText>();
    hudText.Init(text, fontSize, color, flyTextDepth);
  	tempGameObject.transform.localPosition = localPosition;

  	Vector3 toPosition = new Vector3(localPosition.x, localPosition.y + 100f, localPosition.z);
		TweenAlpha tweenAlpha;
  	if (flyDuration > 0) {
  	  TweenPosition tween = TweenPosition.Begin(tempGameObject, flyDuration, toPosition, false);
    	tween.delay = stayTime;
    	
    	tweenAlpha = TweenAlpha.Begin(tempGameObject, flyDuration / 2, 0);
      tweenAlpha.delay = stayTime + (flyDuration / 2);
  	} else {
  	  tweenAlpha = TweenAlpha.Begin(tempGameObject, 0.2f, 0);
      tweenAlpha.delay = stayTime;
  	}
  	EventDelegate.Add(tweenAlpha.onFinished, delegate() { hudText.Destroy(); }, true);
  	return hudText;
  }
  
  
  
}
