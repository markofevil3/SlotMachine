using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

  public static HUDManager Instance { get; private set; }
  public GameObject root;
	public Camera camera;

  private string flyTextPath = "Prefabs/Others/HUDFlyText";
  private string damageTextPath = "Prefabs/Others/HUDDamageText";
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
  
  public HUDText AddDamageText(string text, Vector3 worldPosition, int fontSize = 45, float flyDuration = 0.4f, float stayTime = 0.5f) {
    GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(damageTextPath, typeof(GameObject)) as GameObject);
    HUDText hudText = tempGameObject.GetComponent<HUDText>();
    hudText.Init(text, fontSize, Color.white, flyTextDepth);
		worldPosition = camera.ViewportToWorldPoint(worldPosition);
		worldPosition.y += 0.2f;
		worldPosition.x += 0.2f;
		Debug.Log("@@@@@@@@ " + worldPosition);
  	tempGameObject.transform.position = worldPosition;

  	Vector3 toPosition = new Vector3(worldPosition.x, worldPosition.y + 0.15f, worldPosition.z);
		TweenAlpha tweenAlpha;
		// TweenScale tweenScale = TweenScale.Begin(tempGameObject, 0.1f, Vector3.one);
		// tweenScale.from = Vector3.zero;
  	if (flyDuration > 0) {
  	  TweenPosition tween = TweenPosition.Begin(tempGameObject, flyDuration, toPosition, true);
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
