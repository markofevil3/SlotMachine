using UnityEngine;
using System;
using System.Collections;
using Boomlagoon.JSON;

public class PopupUserInfo : Popup {

  private const int CACHE_USER_SECONDS = 600;

  private static JSONObject user = null;
  private static DateTime? lastRequestTime;

  public GameObject userInfoPanel;
  public UILabel displayNameLabel;
  public UILabel cashLabel;
  public UIButton btnAddFriend;
  public UIButton btnSendMessage;

  public static void SetUser(JSONObject mUser) {
    user = mUser;
    lastRequestTime = Utils.CurrentTime();
  }

  public override void Init(object[] data) {
    base.Init(data);
    string username = data[0].ToString();
    
    int cachedSeconds = 0;
    if (lastRequestTime.HasValue) {
      cachedSeconds = (int)Utils.CurrentTime().Subtract((DateTime)lastRequestTime).TotalSeconds;
    }
    // Cached user and request user is the same -> show instantly
    if (user != null && user.GetString("username") == username && cachedSeconds <= CACHE_USER_SECONDS) {
      userInfoPanel.SetActive(true);
      DisplayUserInfo(user);
    } else {
      userInfoPanel.SetActive(false);
      // Loading user info from server
      PopupManager.Instance.ShowLoadingIndicator();
      UserExtensionRequest.Instance.LoadUserInfo(username);
    }
    
    EventDelegate.Set(btnAddFriend.onClick, EventAddFriend);
  }

  private void EventAddFriend() {
    AccountManager.Instance.AddFriend(user.GetString("username"));
  }

  public void DisplayUserInfo(JSONObject mUser) {
    userInfoPanel.SetActive(true);
    user = mUser;
    displayNameLabel.text = user.GetString("displayName");
    cashLabel.text = user.GetString("cash");
    btnAddFriend.gameObject.SetActive(!AccountManager.Instance.IsFriend(user.GetString("username")));
  }
  
  public void AddFriendSuccess(string fUsername) {
    if (fUsername == user.GetString("username")) {
      btnAddFriend.gameObject.SetActive(false);
    }
  }
}
