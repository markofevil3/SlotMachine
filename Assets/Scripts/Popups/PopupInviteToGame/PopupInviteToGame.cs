using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

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
    if (AccountManager.Instance.friends.Length > 0) {
      LoadListFriendFromServer();
      EventDelegate.Set(btnSendInvite.onClick, EventSendInvite);
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
    Debug.Log("EventSendInvite " + string.Join(",", listInviteUsers.ToArray()));
    SlotMachineClient.Instance.InviteToGame(listInviteUsers, ScreenManager.Instance.CurrentSlotScreen.GetRoomId());
  }

  void LoadListFriendFromServer() {
    PopupManager.Instance.ShowLoadingPopup();
    isLoading = true;
    for (int i = 0; i < wrapContent.transform.childCount; i++) {
      wrapContent.transform.GetChild(i).gameObject.SetActive(false);
    }
    UserExtensionRequest.Instance.LoadFriendList();
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
