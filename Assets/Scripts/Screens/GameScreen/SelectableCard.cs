using UnityEngine;
using System.Collections;

public class SelectableCard : MonoBehaviour {
  public UIEventTriggerExtent eventTrigger;
  public UI2DSprite sprite;

  private GameObject cardBack;
  private bool mIsSelected = false;
  private float selectCardSpeed = 0.15f;
  private float playCardSpeed = 0.3f;
	private Vector3 playedCardScale = new Vector3(0.7f, 0.7f, 1);
	private string cardBackPath = Global.SCREEN_PATH + "/GameScreen/CardBack";
  
  public bool isSelected {
    get { return mIsSelected; }
    set { mIsSelected = value; }
  }
  
  public virtual void Init(Sprite cardImage, bool addCardback = false) {
		sprite.sprite2D = cardImage;
		
		if (addCardback) {
		  GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(cardBackPath, typeof(GameObject)) as GameObject);
		  tempGameObject.name = "CardBack";
		  cardBack = tempGameObject;
		}
    // this.number = number;
    // this.suit = suit;
  }
  
  public void SelectCard() {
    if (isSelected) {
      isSelected = false;
      TweenPosition.Begin(gameObject, selectCardSpeed, new Vector3(transform.localPosition.x, 0, transform.localPosition.z));
    } else {
      isSelected = true;
      TweenPosition.Begin(gameObject, selectCardSpeed, new Vector3(transform.localPosition.x, 20.0f, transform.localPosition.z));
    }
  }
  
  // Play card animation, scale up card
  public void PlayCard(Transform newParent, Vector3 position, Vector3 rotation, bool shouldBounce = true) {
    eventTrigger.enabled = false;
    transform.parent = newParent;
    transform.localEulerAngles = rotation;
    TweenPosition.Begin(gameObject, playCardSpeed, position);
		if (shouldBounce) {
      TweenScale tween = TweenScale.Begin(gameObject, 0.2f, Vector3.one * 1.5f);
      EventDelegate.Add(tween.onFinished, PlayCardAnimationCallback, true);
		} else {
			transform.localScale = playedCardScale;
		}
  }
  
  private void PlayCardAnimationCallback() {
    TweenScale.Begin(gameObject, 0.1f, playedCardScale);
  }
  
  public void SetCardDepth(int depth) {
    sprite.depth = depth;
  }
  
  public void RotateShow() {
		Utils.SetActive(gameObject, true);
		Utils.SetActive(cardBack, true);
    
    TweenAlpha tweenAlpha = TweenAlpha.Begin(gameObject, 0.2f, 1);
    tweenAlpha.from = 0;
    
    TweenRotation tween =  TweenRotation.Begin(gameObject, 0.3f, Quaternion.Euler(0f, 90f, 0f));
    tween.delay = 0.3f;
    EventDelegate.Add(tween.onFinished, RotateShowCallback, true);
  }
  
  void RotateShowCallback() {
    TweenRotation tween =  TweenRotation.Begin(gameObject, 0.3f, Quaternion.Euler(0f, 0f, 0f));
    tween.delay = 0;
    Destroy(cardBack);
    // cardBack.SetActive(false);    
  }
}
