using UnityEngine;
using System.Collections;

public class DropdownNotification : MonoBehaviour {

  public UISprite iconSprite;
  public UILabel label;

	// Use this for initialization
	public void Init (string iconName, string text) {
	  if (iconName != string.Empty) {
	    iconSprite.spriteName = iconName;
	  }
	  label.text = text;
    // transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    // LeanTween.rotateLocal (gameObject, Vector3.zero, 0.6f).setEase(LeanTweenType.easeOutBounce).setOnComplete(Hide);
	  
    TweenRotation tween = TweenRotation.Begin(gameObject, 0.5f,  Quaternion.Euler(0f, 0f, 0f));
    tween.from = new Vector3(90f, 0, 0);
    EventDelegate.Add(tween.onFinished, Hide, true);
	}

  void Hide() {
    TweenRotation tween = TweenRotation.Begin(gameObject, 0.5f,  Quaternion.Euler(90f, 0f, 0f));
    tween.delay = 3.0f;
    EventDelegate.Add(tween.onFinished, Destroy, true);
  }

  void Destroy() {
    Destroy(gameObject);
  }
}
