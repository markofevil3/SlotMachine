using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using Sfs2X.Entities;

public class PopupFriends : Popup {
  
  private const int STOP_DRAG_NUMB_ROW = 3;
  
	public UIEventTriggerExtent tabInvite;
	public UIEventTriggerExtent tabFriends;
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIDragScrollView backgroundDragScrollView;
  
  private bool isLoading = false;
  private JSONArray friendList;
  
  public override void Init(object[] data) {
    base.Init(data);
    EventDelegate.Set(tabInvite.onClick, EventTabInvite);
    EventDelegate.Set(tabFriends.onClick, EventTabFriends);
		
		// Get list friends from smartfox buddy list
    List<Buddy> buddyList = SmartfoxClient.Instance.GetBuddyList();
    if (buddyList.Count > 0) {
			JSONObject friend;
			friendList = new JSONArray();
			for (int i = 0; i < buddyList.Count; i++) {
				friend = new JSONObject();
				if (buddyList[i].IsOnline) {
					Debug.Log("GetOnline Var");
					friend.Add("displayName", buddyList[i].GetVariable("displayName").GetStringValue());
					friend.Add("cash", buddyList[i].GetVariable("cash").GetIntValue());
				} else {
					// friend.Add("displayName", buddyList[i].GetVariable("$displayName").GetStringValue());
					// friend.Add("cash", buddyList[i].GetVariable("$cash").GetIntValue());
					// TEST CODE
					friend.Add("displayName", "offline");
					friend.Add("cash", "offline2");
				}
				friend.Add("username", buddyList[i].Name);
				friendList.Add(friend);
				InitScrollViewData(friendList);
			}
    } else {
      Utils.SetActive(scrollview.gameObject, false);
      Debug.Log("----------- DONE HAVE ANY FRIEND ----------------");
    }
  }

	void EventTabInvite() {
		MyFacebook.Instance.InviteFriends();
	}

	void EventTabFriends() {
		
	}

  // void LoadListFriendFromServer() {
  //   PopupManager.Instance.ShowLoadingPopup();
  //   isLoading = true;
  //   for (int i = 0; i < wrapContent.transform.childCount; i++) {
  //     wrapContent.transform.GetChild(i).gameObject.SetActive(false);
  //   }
  //   UserExtensionRequest.Instance.LoadFriendList();
  // }
  
  public void InitScrollViewData(JSONArray mFriendList) {
    // friendList = mFriendList;
    isLoading = false;
    wrapContent.ResetChildPositions();
    scrollview.currentMomentum = Vector3.zero;
    scrollview.ResetPosition();
    Transform tempGameObject;
    
    wrapContent.minIndex = -(friendList.Length - 1);
    wrapContent.onInitializeItem = UpdateRowDataOnScroll;
    bool canDrag = true;
    if (friendList.Length <= STOP_DRAG_NUMB_ROW) {
      canDrag = false;
      backgroundDragScrollView.enabled = false;
    } else {
      backgroundDragScrollView.enabled = true;
    }
    for (int i = 0; i < wrapContent.transform.childCount; i++) {
      tempGameObject = wrapContent.transform.GetChild(i);
      if (!tempGameObject.gameObject.activeSelf) {
				Utils.SetActive(tempGameObject.gameObject, true);
      }
      FriendRowScript tempRowScript = tempGameObject.GetComponent<FriendRowScript>();
      tempRowScript.Init(scrollview);
      
      if (canDrag) {
        tempRowScript.dragScrollView.enabled = true;
      } else {
        tempRowScript.dragScrollView.enabled = false;
      }
      if (i < friendList.Length) {
				Utils.SetActive(tempGameObject.gameObject, true);
        tempRowScript.UpdateRowData(friendList[i].Obj);
      } else {
				Utils.SetActive(tempGameObject.gameObject, false);
      }
    }
  }
  
  public void UpdateRowDataOnScroll(GameObject go, int wrapIndex, int realIndex) {
    realIndex = (int)Mathf.Abs((float)realIndex);
    if (realIndex < friendList.Length) {
      go.GetComponent<FriendRowScript>().UpdateRowData(friendList[realIndex].Obj);
    }
  }
}
