using UnityEngine;
using System.Collections;

public class PlayerSlotScript : MonoBehaviour {
  
  public UILabel usernameLabel;
  public UILabel cashLabel;
  public UITexture avatarSprite;
  public UIEventTriggerExtent eventTrigger;
	public GameObject glowBackground;
	public GameObject btnAdd;
  // public UISprite circleSprite;
  // public UIButton btnThrowCard;
  // public UIButton btnSit;
  private string avatarLink;
	
  [HideInInspector]
  public string username;
  [HideInInspector]
  public int seatIndex;
  [HideInInspector]
  public Vector3 bubbleChatPos {
    get { return transform.position; }
  }
  
  public void Init(string mUsername, string displayName, long cash, string avatarLink) {
    username = mUsername;
    usernameLabel.text = displayName;
    cashLabel.text = cash.ToString("N0");
		this.avatarLink = avatarLink;
    if (this.avatarLink != string.Empty) {
			StartCoroutine(DisplayAvatar());
      // avatarSprite.spriteName = avatarName;
		} else {
			
		}
    eventTrigger.inputParams = new object[] {username};
    EventDelegate.Set(eventTrigger.onClick, delegate() { EventShowUserInfo((string)eventTrigger.inputParams[0]); });
    HideGlow();
		NGUITools.SetActive(btnAdd, false);
    // circleSprite.gameObject.SetActive(false);
    // btnSit.gameObject.SetActive(false);
    // btnThrowCard.gameObject.SetActive(true);
  }
  
	IEnumerator DisplayAvatar() {
		WWW www = new WWW(this.avatarLink);
		yield return www;
		if (www.texture != null) {
			avatarSprite.mainTexture = www.texture;
		}
		www.Dispose();
	}
	
  public void InitEmpty() {
    username = string.Empty;
    usernameLabel.text = string.Empty;
    cashLabel.text = string.Empty;
		NGUITools.SetActive(btnAdd, true);
    EventDelegate.Set(eventTrigger.onClick, OpenInvitePlayerPopup);
    HideGlow();
    // btnThrowCard.gameObject.SetActive(false);
    // 
    // btnSit.gameObject.SetActive(true);
    // EventDelegate.Set(btnSit.onClick, EventSit);
  }
  
  void EventShowUserInfo(string mUsername) {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_USER_INFO, new object[] { mUsername });
  }
  
  void OpenInvitePlayerPopup() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_INVITE_TO_GAME, null);
  }
  
  public void UpdateCash(long cashVal) {
    cashLabel.text = cashVal.ToString("N0");
  }
  
  public bool IsEmpty() {
    return username == string.Empty;
  }
  
  public bool IsThisUser(string mUsername) {
    return username == mUsername;
  }
  
  private void EventSit() {
    TLMBClient.Instance.Sit(seatIndex);
  }
  
	public void ShowGlow() {
		NGUITools.SetActive(glowBackground, true);
		CancelInvoke("FadeOutGlow");
		Invoke("FadeOutGlow", 3.0f);
	}
	
	private void HideGlow() {
		NGUITools.SetActive(glowBackground, false);
	}

	private void FadeOutGlow() {
		TweenAlpha tween = TweenAlpha.Begin(glowBackground, 0.5f, 0);
    EventDelegate.Add(tween.onFinished, HideGlow, true);
	}
}
