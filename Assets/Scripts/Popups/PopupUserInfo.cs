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
	public UILabel killLabel;
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
			Utils.SetActive(userInfoPanel, true);
      DisplayUserInfo(user);
    } else {
			Utils.SetActive(userInfoPanel, false);
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
		Debug.Log("mUser " + mUser.ToString());
		Utils.SetActive(userInfoPanel, true);
    user = mUser;
    displayNameLabel.text = user.GetString("displayName");
    cashLabel.text = user.GetInt("cash").ToString("N0");
    killLabel.text = user.GetInt("bossKill").ToString("N0");
		Utils.SetActive(btnAddFriend.gameObject, !AccountManager.Instance.IsFriend(user.GetString("username")));
  }
  
  public void AddFriendSuccess(string fUsername) {
    if (fUsername == user.GetString("username")) {
			Utils.SetActive(btnAddFriend.gameObject, false);
    }
  }
}
