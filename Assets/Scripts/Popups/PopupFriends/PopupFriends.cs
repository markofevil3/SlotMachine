using UnityEngine;
using System;
using System.Collections;
using Boomlagoon.JSON;

public class PopupFriends : Popup {
  
  private const int STOP_DRAG_NUMB_ROW = 3;
  
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIDragScrollView backgroundDragScrollView;
  
  private bool isLoading = false;
  private JSONArray friendList;
  
  public override void Init(object[] data) {
    base.Init(data);
    
    Debug.Log(SmartfoxClient.Instance.GetBuddyList().Count);
    
    if (AccountManager.Instance.friends.Length > 0) {
      LoadListFriendFromServer();
    } else {
      scrollview.gameObject.SetActive(false);
      Debug.Log("----------- DONE HAVE ANY FRIEND ----------------");
    }
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
        tempGameObject.gameObject.SetActive(true);
      }
      FriendRowScript tempRowScript = tempGameObject.GetComponent<FriendRowScript>();
      tempRowScript.Init(scrollview);
      
      if (canDrag) {
        tempRowScript.dragScrollView.enabled = true;
      } else {
        tempRowScript.dragScrollView.enabled = false;
      }
      if (i < friendList.Length) {
        tempGameObject.gameObject.SetActive(true);
        tempRowScript.UpdateRowData(friendList[i].Obj);
      } else {
        tempGameObject.gameObject.SetActive(false);
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
