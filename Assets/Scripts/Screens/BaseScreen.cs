using UnityEngine;
using System.Collections;

public class BaseScreen : MonoBehaviour {
	
	public enum Type {
    LOBBY,
    SELECT_GAME,
    SELECT_ROOM,
    GAME_SCREEN,
    LEADERBOARD,
    SLOT_GAME_SCREEN
  }
	
	public Type type;
	
	private GameObject handler;
	private string callback;
	
	public virtual void Init() {}
	public virtual void Init(object[] data) {}
	
	// Destroy screen
	public virtual void Close() {
		DestroyCallBack();
		Destroy(gameObject);	  
	}
	
	// Open new screen
	public virtual void Open() {}
	
	// Hide screen, dont destroy
	public virtual void Hide() {
		Utils.SetActive(gameObject, false);
	}
	
	// Show hidden screen
	public virtual void Show() {
		Utils.SetActive(gameObject, true);
	}
	
	public virtual void FadeIn() {
	  TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.3f, 1);
	  tween.from = 0.3f;
	}
	
	public virtual void FadeOut() {
	  TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.2f, 0);
	  tween.from = 1;
	}
		
	public virtual void FadeOutAndDestroy(GameObject mHandler, string mCallback) {
		handler = mHandler;
		callback = mCallback;
	  TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.6f, 0);
	  tween.from = 1;
  	EventDelegate.Set(tween.onFinished, FadeOutAndDestroyCallback);
	}
	
	void FadeOutAndDestroyCallback() {
		if (handler != null) {
			handler.SendMessage(callback, null, SendMessageOptions.DontRequireReceiver);
		}
		Close();
	}
	
	public virtual void DestroyCallBack() {}
}
