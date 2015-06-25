using UnityEngine;
using System.Collections;

public class PopupLoading : MonoBehaviour {
  
	public UILabel loadingText;
	public UISprite dimBackground;
	
  public void Open(string localizeKey = null, bool showDim = true) {
    PopupManager.Instance.ShowLoadingIndicator();
		if (localizeKey != null) {
			NGUITools.SetActive(loadingText.gameObject, true);
			loadingText.text = Localization.Get(localizeKey);
		} else {
			NGUITools.SetActive(loadingText.gameObject, false);
		}
		if (showDim) {
			dimBackground.color = new Color(0,0,0,0.5f);
		} else {
			dimBackground.color = Color.clear;
		}
  }
  
	public void SetLoadingText(string localizeKey) {
		if (localizeKey != null) {
			NGUITools.SetActive(loadingText.gameObject, true);
			loadingText.text = Localization.Get(localizeKey);
		} else {
			NGUITools.SetActive(loadingText.gameObject, false);
		}
	}
	
  public void Close() {
		StartCoroutine(CloseDelay());

  }
  
	IEnumerator CloseDelay() {
		yield return new WaitForSeconds(0.2f);
    TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.2f, 0);
    EventDelegate.Set(tween.onFinished, Destroy);
	}
	
  void Destroy() {
		PopupManager.Instance.HideLoadingIndicator();
    Destroy(gameObject);
  }
}
