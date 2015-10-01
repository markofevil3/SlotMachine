using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour {
  
  private float dimAnimateSpeed = 0.3f;
  private float dimAlpha = 0.5f;
  private List<Popup> openingPopup = new List<Popup>();
  
  private PopupResult popupResult;
  private PopupLeaveGame popupLeaveGame;
  private PopupCreateRoom popupCreateRoom;
  private PopupUserInfo popupUserInfo;
  private PopupFriends popupFriends;
  private PopupInviteToGame popupInviteToGame;
  private PopupInviteGameConfirm popupInviteGameConfirm;
  private PopupReloadGame popupReloadGame;
	private PopupSetting popupSetting;
	private PopupInbox popupInbox;
	private PopupPayout popupPayout;

  private PopupLoading popupLoading;
  
  public PopupUserInfo PopupUserInfo {
    get { return popupUserInfo; }
  }
  
  public PopupFriends PopupFriends {
    get { return popupFriends; }
  }
  
  public PopupInviteToGame PopupInviteToGame {
    get { return popupInviteToGame; }
  }

  public PopupInbox PopupInbox {
    get { return popupInbox; }
  }

  public static PopupManager Instance { get; private set; }
	public GameObject root;
	public Camera camera;
  
  public GameObject dimBackground;
  public UISprite dimBackgroundSprite;
  public GameObject dimPanel;
  private bool isShowingDim = false;

	// Use this for initialization
	void Awake () {
	  Instance = this;
	  HideDimNoAnimation();
	}
  
	public void Restart() {
		CloseAllNoAnimation();
	}
	
  public void FadeInScreen() {
		Utils.SetActive(dimBackground, true);
    dimBackgroundSprite.color = Color.black;
    TweenAlpha tween = TweenAlpha.Begin(dimBackground, dimAlpha, 0);
		tween.from = dimAlpha;
  }
  
  public void ShowNotification(string iconName, string text) {
    GameObject tempGameObject = NGUITools.AddChild(dimPanel, Resources.Load(Global.POPUP_PATH + "/Others/DropdownNotification", typeof(GameObject)) as GameObject);
    tempGameObject.name = "DropdownNotification";
    DropdownNotification dropdown = tempGameObject.GetComponent<DropdownNotification>();
    dropdown.Init(iconName, text);
  }
  
  public void ShowDim() {
    if (!isShowingDim) {
      isShowingDim = true;
			Utils.SetActive(dimBackground, true);
      // dimBackgroundSprite.color = Color.white;
      TweenAlpha tween = TweenAlpha.Begin(dimBackground, dimAnimateSpeed, dimAlpha);
  		tween.from = 0;
    }
  }
  
  public void HideDim() {
    if (openingPopup.Count == 0) {
      LeanTween.cancel(dimBackground);
  		// LeanTween.value(dimBackground, DoUpdateDimAlpha, dimBackgroundSprite.alpha, 0, dimAnimateSpeed, new Hashtable() {
  		// 	{"onComplete", "HideDimCallback"},
  		// 	{"onCompleteTarget", gameObject}
  		// });
  		LeanTween.value(dimBackground, DoUpdateDimAlpha, dimBackgroundSprite.alpha, 0, dimAnimateSpeed).setOnComplete(HideDimCallback);
    }
	}
  
  public void HideDimNoAnimation() {
    if (openingPopup.Count == 0) {
      dimBackgroundSprite.alpha = 0;
  		HideDimCallback();
    }
	}
  
  private void DoUpdateDimAlpha(float updateVal) {
		dimBackgroundSprite.alpha = updateVal;
	}
  
  private void HideDimCallback() {
    isShowingDim = false;
		Utils.SetActive(dimBackground, false);
  }
  
  public void OpenPopup(Popup.Type type, object[] data = null, bool shouldAnimate = true) {
    Popup tempPopup = null;
		switch(type) {
	    case Popup.Type.POPUP_RESULT:
	    	if (popupResult == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupResult/PopupResult", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupResult";
	       	popupResult = tempGameObject.GetComponent<PopupResult>();
	       	tempPopup = popupResult as Popup;
	       	popupResult.Init(data);
	       	if (shouldAnimate) {
	       	  popupResult.Open();
	       	} else {
	       	  popupResult.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_LEAVE_GAME:
	    	if (popupLeaveGame == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupLeaveGame", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupResult";
	       	popupLeaveGame = tempGameObject.GetComponent<PopupLeaveGame>();
	       	tempPopup = popupLeaveGame as Popup;
	       	popupLeaveGame.Init(data);
	       	if (shouldAnimate) {
	       	  popupLeaveGame.Open();
	       	} else {
	       	  popupLeaveGame.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_CREATE_ROOM:
	    	if (popupCreateRoom == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupCreateRoom/PopupCreateRoom", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupCreateRoom";
	       	popupCreateRoom = tempGameObject.GetComponent<PopupCreateRoom>();
	       	tempPopup = popupCreateRoom as Popup;
	       	popupCreateRoom.Init(data);
	       	if (shouldAnimate) {
	       	  popupCreateRoom.Open();
	       	} else {
	       	  popupCreateRoom.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_USER_INFO:
	    	if (popupUserInfo == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupUserInfo", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupUserInfo";
	       	popupUserInfo = tempGameObject.GetComponent<PopupUserInfo>();
	       	tempPopup = popupUserInfo as Popup;
	       	popupUserInfo.Init(data);
	       	if (shouldAnimate) {
	       	  popupUserInfo.Open();
	       	} else {
	       	  popupUserInfo.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_FRIENDS:
	    	if (popupFriends == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupFriends/PopupFriends", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupFriends";
	       	popupFriends = tempGameObject.GetComponent<PopupFriends>();
	       	tempPopup = popupFriends as Popup;
	       	popupFriends.Init(data);
	       	if (shouldAnimate) {
	       	  popupFriends.Open();
	       	} else {
	       	  popupFriends.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_INBOX:
	    	if (popupFriends == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupInbox/PopupInbox", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupInbox";
	       	popupInbox = tempGameObject.GetComponent<PopupInbox>();
	       	tempPopup = popupInbox as Popup;
	       	popupInbox.Init(data);
	       	if (shouldAnimate) {
	       	  popupInbox.Open();
	       	} else {
	       	  popupInbox.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_INVITE_TO_GAME:
	    	if (popupInviteToGame == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupInviteToGame/PopupInviteToGame", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupInviteToGame";
	       	popupInviteToGame = tempGameObject.GetComponent<PopupInviteToGame>();
	       	tempPopup = popupInviteToGame as Popup;
	       	popupInviteToGame.Init(data);
	       	if (shouldAnimate) {
	       	  popupInviteToGame.Open();
	       	} else {
	       	  popupInviteToGame.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_INVITE_TO_GAME_CONFIRM:
	    	if (popupInviteGameConfirm == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupInviteGameConfirm", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupInviteGameConfirm";
	       	popupInviteGameConfirm = tempGameObject.GetComponent<PopupInviteGameConfirm>();
	       	tempPopup = popupInviteGameConfirm as Popup;
	       	popupInviteGameConfirm.Init(data);
	       	if (shouldAnimate) {
	       	  popupInviteGameConfirm.Open();
	       	} else {
	       	  popupInviteGameConfirm.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_RELOAD_GAME:
	    	if (popupReloadGame == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupReloadGame", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupReloadGame";
	       	popupReloadGame = tempGameObject.GetComponent<PopupReloadGame>();
	       	tempPopup = popupReloadGame as Popup;
	       	popupReloadGame.Init(data);
	       	if (shouldAnimate) {
	       	  popupReloadGame.Open();
	       	} else {
	       	  popupReloadGame.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_SETTING:
	    	if (popupReloadGame == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupSetting", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupSetting";
	       	popupSetting = tempGameObject.GetComponent<PopupSetting>();
	       	tempPopup = popupSetting as Popup;
	       	popupSetting.Init(data);
	       	if (shouldAnimate) {
	       	  popupSetting.Open();
	       	} else {
	       	  popupSetting.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	    case Popup.Type.POPUP_PAYOUT:
	    	if (popupPayout == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupPayout", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupPayout";
	       	popupPayout = tempGameObject.GetComponent<PopupPayout>();
	       	tempPopup = popupPayout as Popup;
	       	popupPayout.Init(data);
	       	if (shouldAnimate) {
	       	  popupPayout.Open();
	       	} else {
	       	  popupPayout.OpenPopupNoAnimation();
	       	}
	     	}
	    break;
	  }
	  if (tempPopup != null) {
	    openingPopup.Add(tempPopup);
	  }
  }
  
  // Remove popup from opening list
  public void ClosePopup(Popup popup) {
    openingPopup.Remove(popup);
    switch(popup.type) {
	    case Popup.Type.POPUP_RESULT:
	      popupResult = null;
	    break;
	    case Popup.Type.POPUP_LEAVE_GAME:
	      popupLeaveGame = null;
	    break;
	    case Popup.Type.POPUP_CREATE_ROOM:
	      popupCreateRoom = null;
	    break;
	    case Popup.Type.POPUP_USER_INFO:
	      popupUserInfo = null;
	    break;
	    case Popup.Type.POPUP_FRIENDS:
	      popupFriends = null;
	    break;
	    case Popup.Type.POPUP_INVITE_TO_GAME:
	      popupInviteToGame = null;
	    break;
	    case Popup.Type.POPUP_INVITE_TO_GAME_CONFIRM:
	      popupInviteGameConfirm = null;
	    break;
	    case Popup.Type.POPUP_RELOAD_GAME:
	      popupReloadGame = null;
	    break;
	    case Popup.Type.POPUP_SETTING:
	      popupSetting = null;
	    break;
	    case Popup.Type.POPUP_INBOX:
	      popupInbox = null;
	    break;
	    case Popup.Type.POPUP_PAYOUT:
	      popupPayout = null;
	    break;
	  }
  }
  
	public void CloseAll() {
		for (int i = 0; i < openingPopup.Count; i++) {
			openingPopup[i].Close();
			i--;
		}
	}
	
	public void CloseAllNoAnimation() {
		for (int i = 0; i < openingPopup.Count; i++) {
			openingPopup[i].CloseNoAnimation();
			i--;
		}
	}
	
  // Show device loading indicator
  public void ShowLoadingIndicator() {
    #if UNITY_IPHONE
			if (Global.isTablet) {
				Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.WhiteLarge);
			} else {
				Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.White);
			}
    #elif UNITY_ANDROID
      Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
    #endif
    #if UNITY_IPHONE || UNITY_ANDROID
	    Handheld.StartActivityIndicator();
    #endif
  }
  
  // Hide device loading indicator
  public void HideLoadingIndicator() {
	  #if UNITY_IPHONE || UNITY_ANDROID
	    Handheld.StopActivityIndicator();
	  #endif
  }
  
  public void ShowLoadingPopup(string localizeKey = null, bool showDim = true) {
    if (popupLoading == null) {
      GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupLoading", typeof(GameObject)) as GameObject);
     	tempGameObject.name = "PopupLoading";
     	popupLoading = tempGameObject.GetComponent<PopupLoading>();
     	popupLoading.Open(localizeKey, showDim);
		} else {
     	popupLoading.SetLoadingText(localizeKey);
		}
  }
  
  public void CloseLoadingPopup() {
    if (popupLoading != null) {
      popupLoading.Close();
      popupLoading = null;
    }
  }
}
