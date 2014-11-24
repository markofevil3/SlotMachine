using UnityEngine;
using System.Collections;

public class UserBottomBar : MonoBehaviour {

  public UIButton btnLeaderboard;
  public UIButton btnFriends;
  public UIButton btnSetting;
  public UILabel usernameLabel;
  public UILabel cashLabel;
  public UITexture avatarTexture;

  private BaseScreen currentScreen;

  public void Init(BaseScreen currentScreen) {
    this.currentScreen = currentScreen;
    EventDelegate.Set(btnLeaderboard.onClick, EventOpenLeaderboard);
    EventDelegate.Set(btnFriends.onClick, EventOpenFriendPopup);
    EventDelegate.Set(btnSetting.onClick, EventOpenSetting);
    if (SmartfoxClient.Instance.IsLoggedIn) {
      EventUserLoggedIn();
    } else {
      EventUserLoggedOut();
    }
  }
  
  public void EventUserLoggedIn() {
    usernameLabel.text = AccountManager.Instance.username;
    cashLabel.text = AccountManager.Instance.cash.ToString("N0");
    
    btnLeaderboard.isEnabled = true;
    btnFriends.isEnabled = true;
    btnSetting.isEnabled = true;
  }
  
  public void EventUserLoggedOut() {
    usernameLabel.text = string.Empty;
    cashLabel.text = string.Empty;
    btnLeaderboard.isEnabled = false;
    btnFriends.isEnabled = false;
    btnSetting.isEnabled = false;
  }
  
  private void EventOpenLeaderboard() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LEADERBOARD, null, true, true);
  }
  
  private void EventOpenFriendPopup() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_FRIENDS);
  }

  private void EventOpenSetting() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_SETTING);
  }
	
}
