using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class PopupInbox : Popup {
  
  private const int STOP_DRAG_NUMB_ROW = 3;
  
	public UIButton btnBack;
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIDragScrollView backgroundDragScrollView;
	public GameObject noMessageLabel;
	// View message panel
	public GameObject viewMessagePanel;
	public UILabel messageTitle;
	public UILabel messageContent;
	public UIButton btnClaim;
	public UITableExtent rewardTable;
	public GameObject goldObject;
	public GameObject gemObject;
	public UILabel goldLabel;
	public UILabel gemLabel;
	
  private bool isLoading = false;
  private JSONArray messageList;
	
  public override void Init(object[] data) {
    base.Init(data);
		Utils.SetActive(noMessageLabel, false);
		Utils.SetActive(scrollview.gameObject, false);
		Utils.SetActive(viewMessagePanel, false);
    EventDelegate.Set(btnBack.onClick, EventBackToListMessages);
		Utils.SetActive(btnBack.gameObject, false);
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
			Debug.Log("LoadInboxCallback " + messages.ToString());
			messageList = messages;
			Utils.SetActive(noMessageLabel, false);
			Utils.SetActive(scrollview.gameObject, true);
			InitScrollViewData();
			// Hide notice and update lastReadInboxMes
			AccountManager.Instance.lastInboxTime = AccountManager.Instance.lastReadInboxTime = messages[messages.Length - 1].Obj.GetLong("createdAt");
			if (ScreenManager.Instance.SelectGameScreen != null) {
				ScreenManager.Instance.SelectGameScreen.ShowOrHideMailNotice();
			}
		} else {
			Utils.SetActive(noMessageLabel, true);
			Utils.SetActive(scrollview.gameObject, false);
		}
	}
	
	// TO DO: sort messages lastest to oldest
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
	
	private JSONObject selectMessage;
	
	public void EventViewMessage(JSONObject message) {
		selectMessage = message;
		Utils.SetActive(scrollview.gameObject, false);
		Utils.SetActive(viewMessagePanel, true);
		Utils.SetActive(btnBack.gameObject, true);
		messageTitle.text = message.GetString("title");
		messageContent.text = message.GetString("message");
		int goldVal = message.GetInt("goldVal");
		int gemVal = message.GetInt("gemVal");
		if (goldVal <= 0 && gemVal <= 0) {
			Utils.SetActive(goldObject, false);
			Utils.SetActive(gemObject, false);
	    EventDelegate.Set(btnClaim.onClick, delegate() {});
			Utils.SetActive(btnClaim.gameObject, false);
		} else {
			Utils.SetActive(goldObject, goldVal > 0);
			Utils.SetActive(gemObject, gemVal > 0);
			goldLabel.text = goldVal.ToString("N0");
			gemLabel.text = gemVal.ToString("N0");
			rewardTable.Reposition();
			Utils.SetActive(btnClaim.gameObject, true);
	    EventDelegate.Set(btnClaim.onClick, delegate() { ClaimReward(message.GetInt("type"), message.GetLong("createdAt"), message.GetString("fromUsername")); });
		}
	}
	
	void ClaimReward(int type, long createdAt, string fromUsername) {
		UserExtensionRequest.Instance.ClaimInboxReward(type, createdAt, fromUsername);
		// JSONObject message;
		// for (int i = 0; i < messageList.Length; i++) {
		// 	message = messageList[i].Obj;
		// 	if (message.GetLong("createdAt") == createdAt && message.GetInt("type") == type) {
		// 		messageList.Remove(i);
		// 		Debug.Log("ClaimReward " + type + " " + createdAt + " " + fromUsername);
		// 		break;
		// 	}
		// }
		//     InitScrollViewData();
	}
	
	// TO DO: update gem val
	public void ClaimRewardSuccess(JSONObject jsonData) {
		Debug.Log("ClaimRewardSuccess " + jsonData.ToString());
		AccountManager.Instance.UpdateUserCash(jsonData.GetInt("goldVal"));
		AccountManager.Instance.UpdateUserGem(jsonData.GetInt("gemVal"));
		JSONObject message;
		for (int i = 0; i < messageList.Length; i++) {
			message = messageList[i].Obj;
			if (message.GetLong("createdAt") == selectMessage.GetLong("createdAt") && message.GetInt("type") == selectMessage.GetInt("type") && (string.IsNullOrEmpty(selectMessage.GetString("fromUsername")) || message.GetString("fromUsername") == selectMessage.GetString("fromUsername"))) {
				messageList.Remove(i);
				break;
			}
		}
		EventBackToListMessages();
    InitScrollViewData();
    HUDManager.Instance.AddFlyText(Localization.Get("PopupInbox_ClaimSuccess"), Vector3.zero, 40, Color.green);
	}
	
	void EventBackToListMessages() {
		Utils.SetActive(scrollview.gameObject, true);
		Utils.SetActive(viewMessagePanel, false);
		Utils.SetActive(btnBack.gameObject, false);
	}
}
