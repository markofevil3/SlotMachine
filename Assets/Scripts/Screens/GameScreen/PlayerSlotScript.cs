using UnityEngine;
using System.Collections;

public class PlayerSlotScript : MonoBehaviour {
  
  public UILabel usernameLabel;
  public UILabel cashLabel;
  public UISprite avatarSprite;
  public UISprite circleSprite;
  public UIButton btnThrowCard;
  public UIButton btnSit;
  
  [HideInInspector]
  public string username;
  [HideInInspector]
  public int seatIndex;
  [HideInInspector]
  public Vector3 bubbleChatPos {
    get { return transform.position; }
  }
  
  public void Init(int mSeatIndex, string mUsername, string displayName, int cash, string avatarName) {
    seatIndex = mSeatIndex;
    username = mUsername;
    usernameLabel.text = displayName;
    cashLabel.text = cash.ToString("N0");
    if (avatarName != string.Empty) {
      avatarSprite.spriteName = avatarName;
    }
    circleSprite.gameObject.SetActive(false);
    btnSit.gameObject.SetActive(false);
    btnThrowCard.gameObject.SetActive(true);
  }
  
  public void InitEmpty(int mSeatIndex) {
    seatIndex = mSeatIndex;
    btnThrowCard.gameObject.SetActive(false);
    
    btnSit.gameObject.SetActive(true);
    EventDelegate.Set(btnSit.onClick, EventSit);
  }
  
  private void EventSit() {
    TLMBClient.Instance.Sit(seatIndex);
  }
  
  public void StopCircle() {
    LeanTween.cancel(gameObject);
    HideCircle();
  }
  
  public void ShowCircle(float duration) {
    circleSprite.gameObject.SetActive(true);
    TweenAlpha tween = TweenAlpha.Begin(circleSprite.gameObject, 0.5f, 1f);
    tween.from = 0;
    LeanTween.value(gameObject, UpdateCircleValue, 0, 1, duration).setOnComplete(HideCircle);
  }
  
  public void HideCircle() {
    TweenAlpha tween = TweenAlpha.Begin(circleSprite.gameObject, 0.2f, 0);
    EventDelegate.Add(tween.onFinished, HideCallback, true);
  }
  
  private void HideCallback() {
    circleSprite.gameObject.SetActive(false);
  }
  
  void UpdateCircleValue(float updateVal) {
    circleSprite.fillAmount = updateVal;
  }
}
