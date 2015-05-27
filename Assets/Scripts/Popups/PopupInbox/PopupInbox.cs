using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class PopupInbox : Popup {
  
  private const int STOP_DRAG_NUMB_ROW = 3;
  
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIDragScrollView backgroundDragScrollView;
	public GameObject noMessageLabel;
  
  private bool isLoading = false;
  private JSONArray messageList;
	
  public override void Init(object[] data) {
    base.Init(data);
		Utils.SetActive(noMessageLabel, false);
		Utils.SetActive(scrollview.gameObject, false);
		LoadInboxMessages();
    // EventDelegate.Set(tabInvite.onClick, EventTabInvite);
    // EventDelegate.Set(tabFriends.onClick, EventTabFriends);
  }

	void LoadInboxMessages() {
		isLoading = true;
		UserExtensionRequest.Instance.LoadInboxData();
	}

	public void LoadInboxCallback(JSONArray messages) {
    isLoading = false;
		if (messages != null && messages.Length > 0) {
			messageList = messages;
			Utils.SetActive(noMessageLabel, false);
			Utils.SetActive(scrollview.gameObject, true);
			InitScrollViewData();
		} else {
			Utils.SetActive(noMessageLabel, true);
			Utils.SetActive(scrollview.gameObject, false);
		}
		Debug.Log("LoadInboxCallback " + messages.ToString());
	}
	
  public void InitScrollViewData() {
    wrapContent.ResetChildPositions();
    scrollview.currentMomentum = Vector3.zero;
    scrollview.ResetPosition();
    Transform tempGameObject;
    
    wrapContent.minIndex = -(messageList.Length - 1);
    wrapContent.onInitializeItem = UpdateRowDataOnScroll;
    bool canDrag = true;
    if (messageList.Length <= STOP_DRAG_NUMB_ROW) {
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
      InboxRowScript tempRowScript = tempGameObject.GetComponent<InboxRowScript>();
      tempRowScript.Init(scrollview);
      if (canDrag) {
        tempRowScript.dragScrollView.enabled = true;
      } else {
        tempRowScript.dragScrollView.enabled = false;
      }
      if (i < messageList.Length) {
				Utils.SetActive(tempGameObject.gameObject, true);
        tempRowScript.UpdateRowData(messageList[i].Obj);
      } else {
				Utils.SetActive(tempGameObject.gameObject, false);
      }
    }
  }
  
  public void UpdateRowDataOnScroll(GameObject go, int wrapIndex, int realIndex) {
    realIndex = (int)Mathf.Abs((float)realIndex);
    if (realIndex < messageList.Length) {
      go.GetComponent<InboxRowScript>().UpdateRowData(messageList[realIndex].Obj);
    }
  }
}
