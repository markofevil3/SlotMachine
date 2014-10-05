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
	
	public virtual void Init() {}
	public virtual void Init(object[] data) {}
	
	// Destroy screen
	public virtual void Close() {
		Destroy(gameObject);	  
	}
	
	// Open new screen
	public virtual void Open() {}
	
	// Hide screen, dont destroy
	public virtual void Hide() {
	  gameObject.SetActive(false);
	}
	
	// Show hidden screen
	public virtual void Show() {
	  gameObject.SetActive(true);
	}
	
	public virtual void FadeIn() {
	  TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.3f, 1);
	  tween.from = 0.3f;
	}
	
	public virtual void FadeOut() {
	  TweenAlpha tween = TweenAlpha.Begin(gameObject, 0.2f, 0);
	  tween.from = 1;
	}
}
