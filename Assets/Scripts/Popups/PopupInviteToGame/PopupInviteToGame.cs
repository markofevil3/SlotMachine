using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using Sfs2X.Entities;

public class PopupInviteToGame : Popup {
  
  private const int STOP_DRAG_NUMB_ROW = 3;
  
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIDragScrollView backgroundDragScrollView;
  public UIButton btnSendInvite;
  
  private bool isLoading = false;
  private JSONArray friendList;
  private JSONArray friendListRows;
  private List<String> listInviteUsers = new List<String>();
  
  public override void Init(object[] data) {
    base.Init(data);
    List<Buddy> buddyList = SmartfoxClient.Instance.GetBuddyList();
    if (buddyList.Count > 0) {
      EventDelegate.Set(btnSendInvite.onClick, EventSendInvite);
			JSONObject friend;
			friendList = new JSONArray();
			for (int i = 0; i < buddyList.Count; i++) {
				friend = new JSONObject();
				if (buddyList[i].IsOnline) {
					Debug.Log("GetOnline Var");
					friend.Add("displayName", buddyList[i].GetVariable("displayName").GetStringValue());
					friend.Add("cash", buddyList[i].GetVariable("cash").GetIntValue());
				} else {
					friend.Add("displayName", buddyList[i].GetVariable("$displayName").GetStringValue());
					friend.Add("cash", buddyList[i].GetVariable("$cash").GetIntValue());
				}
				friend.Add("username", buddyList[i].Name);
				friendList.Add(friend);
				InitScrollViewData(friendList);
			}
    } else {
      Utils.SetActive(scrollview.gameObject, false);
      Utils.SetActive(btnSendInvite.gameObject, false);
      Debug.Log("----------- DONE HAVE ANY FRIEND ----------------");
    }
  }

  public void AddInviteUser(String username) {
    listInviteUsers.Add(username);
  }

  public void RemoveInviteUser(String username) {
    listInviteUsers.Remove(username);
  }

  void EventSendInvite() {
		if (listInviteUsers.Count > 0) {
			JSONArray arr = new JSONArray();
			for (int i = 0; i < listInviteUsers.Count; i++) {
				arr.Add(listInviteUsers[i]);
			}
	    Debug.Log("EventSendInvite " + arr.ToString());
			
	    UserExtensionRequest.Instance.InviteToGame(arr, ScreenManager.Instance.CurrentSlotScreen.GetCrtGameType(), ScreenManager.Instance.CurrentSlotScreen.GetRoomId());
			Close();
		}
  }
  
  public void InitScrollViewData(JSONArray mFriendList) {
    friendList = mFriendList;
    friendListRows = new JSONArray();
    JSONObject row = new JSONObject();
    for (int i = 0; i < friendList.Length; i++) {
      if (Utils.IsOdd(i)) {
        row.Add("right", friendList[i].Obj);
        friendListRows.Add(row);
      } else {
        row = new JSONObject();
        row.Add("left", friendList[i].Obj);
        if (i == friendList.Length - 1) {
          friendListRows.Add(row);
        }
      }
    }
    Debug.Log(friendListRows.ToString());
    isLoading = false;
    wrapContent.ResetChildPositions();
    scrollview.currentMomentum = Vector3.zero;
    scrollview.ResetPosition();
    Transform tempGameObject;
    
    wrapContent.minIndex = -(friendListRows.Length - 1);
    wrapContent.onInitializeItem = UpdateRowDataOnScroll;
    bool canDrag = true;
    if (friendListRows.Length <= STOP_DRAG_NUMB_ROW) {
      canDrag = false;
      backgroundDragScrollView.enabled = false;
    } else {
      backgroundDragScrollView.enabled = true;
    }
    for (int i = 0; i < wrapContent.transform.childCount; i++) {
      tempGameObject = wrapContent.transform.GetChild(i);
      if (!tempGameObject.gameObject.activeSelf) {
        tempGameObject.gameObject.SetActive(true);
      }
      InviteRowScript tempRowScript = tempGameObject.GetComponent<InviteRowScript>();
      tempRowScript.Init(scrollview);
      
      if (canDrag) {
        tempRowScript.dragScrollView.enabled = true;
      } else {
        tempRowScript.dragScrollView.enabled = false;
      }
      if (i < friendListRows.Length) {
        tempGameObject.gameObject.SetActive(true);
        tempRowScript.UpdateRowData(friendListRows[i].Obj);
      } else {
        tempGameObject.gameObject.SetActive(false);
      }
    }
  }
  
  public void UpdateRowDataOnScroll(GameObject go, int wrapIndex, int realIndex) {
    realIndex = (int)Mathf.Abs((float)realIndex);
    if (realIndex < friendListRows.Length) {
      go.GetComponent<InviteRowScript>().UpdateRowData(friendListRows[realIndex].Obj);
    }
  }
}
