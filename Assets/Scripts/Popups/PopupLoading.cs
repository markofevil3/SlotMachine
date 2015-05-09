using UnityEngine;
using System.Collections;

public class PopupLoading : MonoBehaviour {
  
	public UILabel loadingText;
	
  public void Open(string localizeKey = null) {
    PopupManager.Instance.ShowLoadingIndicator();
		if (localizeKey != localizeKey) {
			NGUITools.SetActive(loadingText.gameObject, true);
			loadingText.text = Localization.Get(localizeKey);
		} else {
			NGUITools.SetActive(loadingText.gameObject, false);
		}
  }
  
  public void Close() {
    TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.5f, 0);
    EventDelegate.Set(tween.onFinished, Destroy);
		PopupManager.Instance.HideLoadingIndicator();
  }
  
  void Destroy() {
    Destroy(gameObject);
  }
}
