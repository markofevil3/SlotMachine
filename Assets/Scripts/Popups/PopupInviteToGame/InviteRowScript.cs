using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class InviteRowScript : MonoBehaviour {
  
  [HideInInspector]
  public JSONObject rowData;
  
  public GameObject leftUser;
  public GameObject rightUser;
  
  public UILabel leftUsernameLabel;
  public UITexture leftAvatarTexture;
  public UIToggle leftCheckbox;
  public UIEventTriggerExtent leftCheckboxListener;
  
  public UILabel rightUsernameLabel;
  public UITexture rightAvatarTexture;
  public UIToggle rightCheckbox;
  public UIEventTriggerExtent rightCheckboxListener;
  
  public UIDragScrollView dragScrollView;
  
  private JSONObject leftUserData;
  private JSONObject rightUserData;
  private bool leftCheckboxState = false;
  private bool rightCheckboxState = false;
  
  public void Init(UIScrollView scrollview) {
    if (dragScrollView != null) {
      dragScrollView.scrollView = scrollview;
    }
  }
  
  public void UpdateRowData(JSONObject data) {
    rowData = data;
    leftUserData = data.GetObject("left");
    if (data.ContainsKey("right")) {
      rightUserData = data.GetObject("right");
    } else {
      rightUserData = null;
    }
    if (leftUserData != null) {
      Utils.SetActive(leftUser, true);
      leftUsernameLabel.text = leftUserData.GetString("displayName");
      leftCheckbox.value = leftCheckboxState;
      EventDelegate.Set(leftCheckboxListener.onClick, delegate() { EventCheckboxChanged(true); });
    } else {
      Utils.SetActive(leftUser, false);
    }
    if (rightUserData != null) {
      Utils.SetActive(rightUser, true);
      rightUsernameLabel.text = rightUserData.GetString("displayName");
      rightCheckbox.value = rightCheckboxState;
      EventDelegate.Set(rightCheckboxListener.onClick, delegate() { EventCheckboxChanged(false); });
    } else {
      Utils.SetActive(rightUser, false);
    }
  }
  
  void EventCheckboxChanged(bool isLeft) {
    if (isLeft) {
      leftCheckboxState = leftCheckbox.value = !leftCheckbox.value;
      if (PopupManager.Instance.PopupInviteToGame != null) {
        if (leftCheckboxState) {
          PopupManager.Instance.PopupInviteToGame.AddInviteUser(leftUserData.GetString("username"));
        } else {
          PopupManager.Instance.PopupInviteToGame.RemoveInviteUser(leftUserData.GetString("username"));
        }
      }
    } else {
      rightCheckboxState = rightCheckbox.value = !rightCheckbox.value;
      if (PopupManager.Instance.PopupInviteToGame != null) {
        if (rightCheckboxState) {
          PopupManager.Instance.PopupInviteToGame.AddInviteUser(rightUserData.GetString("username"));
        } else {
          PopupManager.Instance.PopupInviteToGame.RemoveInviteUser(rightUserData.GetString("username"));
        }
      }
    }
  }
}
