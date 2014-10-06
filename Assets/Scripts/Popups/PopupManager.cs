﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour {
  
  private float dimAnimateSpeed = 0.1f;
  private float dimAlpha = 0.7f;
  private List<Popup> openingPopup = new List<Popup>();
  
  private PopupResult popupResult;
  private PopupLeaveGame popupLeaveGame;
  private PopupCreateRoom popupCreateRoom;
  private PopupUserInfo popupUserInfo;
  private PopupFriends popupFriends;
  private PopupSlotMachine popupSlotMachine;

  private PopupLoading popupLoading;
  
  public PopupUserInfo PopupUserInfo {
    get { return popupUserInfo; }
  }
  
  public PopupFriends PopupFriends {
    get { return popupFriends; }
  }
  
  public PopupSlotMachine PopupSlotMachine {
    get { return popupSlotMachine; }
  }
  
  public static PopupManager Instance { get; private set; }
	public GameObject root;
	public Camera camera;
  
  public GameObject dimBackground;
  public UISprite dimBackgroundSprite;
  public GameObject dimPanel;

	// Use this for initialization
	void Awake () {
	  Instance = this;
	  HideDimNoAnimation();
	}
  
  public void FadeInScreen() {
    dimBackground.SetActive(true);
    dimBackgroundSprite.color = Color.black;
    TweenAlpha tween = TweenAlpha.Begin(dimBackground, 0.8f, 0);
		tween.from = 0.8f;
  }
  
  public void ShowNotification(string iconName, string text) {
    GameObject tempGameObject = NGUITools.AddChild(dimPanel, Resources.Load(Global.POPUP_PATH + "/Others/DropdownNotification", typeof(GameObject)) as GameObject);
    tempGameObject.name = "DropdownNotification";
    DropdownNotification dropdown = tempGameObject.GetComponent<DropdownNotification>();
    dropdown.Init(iconName, text);
  }
  
  public void ShowDim() {
    dimBackground.SetActive(true);
    // dimBackgroundSprite.color = Color.white;
    TweenAlpha tween = TweenAlpha.Begin(dimBackground, dimAnimateSpeed, dimAlpha);
		tween.from = 0;
  }
  
  public void HideDim() {
		LeanTween.cancel(dimBackground);
		LeanTween.value(dimBackground, DoUpdateDimAlpha, dimBackgroundSprite.alpha, 0, dimAnimateSpeed, new Hashtable() {
			{"onComplete", "HideDimCallback"},
			{"onCompleteTarget", gameObject}
		});
	}
  
  public void HideDimNoAnimation() {
    dimBackgroundSprite.alpha = 0;
		dimBackground.SetActive(false);
	}
  
  private void DoUpdateDimAlpha(float updateVal) {
		dimBackgroundSprite.alpha = updateVal;
	}
  
  private void HideDimCallback() {
    dimBackground.SetActive(false);
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
	    case Popup.Type.POPUP_SLOT_MACHINE:
	    	if (popupSlotMachine == null) {
	      	GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupSlotMachine/PopupSlotMachine", typeof(GameObject)) as GameObject);
	       	tempGameObject.name = "PopupSlotMachine";
	       	popupSlotMachine = tempGameObject.GetComponent<PopupSlotMachine>();
	       	tempPopup = popupSlotMachine as Popup;
	       	popupSlotMachine.Init(data);
	       	if (shouldAnimate) {
	       	  popupSlotMachine.Open();
	       	} else {
	       	  popupSlotMachine.OpenPopupNoAnimation();
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
	    case Popup.Type.POPUP_SLOT_MACHINE:
	      popupSlotMachine = null;
	    break;
	  }
  }
  
  // Show device loading indicator
  public void ShowLoadingIndicator() {
    #if UNITY_IPHONE
			if (Global.isTablet) {
				Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.WhiteLarge);
			} else {
				Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.White);
			}
    #elif UNITY_ANDROID
      Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
    #endif
    Handheld.StartActivityIndicator();
  }
  
  // Hide device loading indicator
  public void HideLoadingIndicator() {
    Handheld.StopActivityIndicator();
  }
  
  public void ShowLoadingPopup() {
    if (popupLoading == null) {
      GameObject tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.POPUP_PATH + "/PopupLoading", typeof(GameObject)) as GameObject);
     	tempGameObject.name = "PopupLoading";
     	popupLoading = tempGameObject.GetComponent<PopupLoading>();
     	popupLoading.Open();
    }
  }
  
  public void CloseLoadingPopup() {
    if (popupLoading != null) {
      popupLoading.Close();
      popupLoading = null;
    }
  }
}