using UnityEngine;
using System.Collections;

public class UserBottomBar : MonoBehaviour {

  public UIButton btnLeaderboard;
  public UIButton btnFriends;
  public UIButton btnSlotMachine;
  public UILabel usernameLabel;
  public UILabel cashLabel;
  public UITexture avatarTexture;

  private BaseScreen currentScreen;

  public void Init(BaseScreen currentScreen) {
    this.currentScreen = currentScreen;
    EventDelegate.Set(btnLeaderboard.onClick, EventOpenLeaderboard);
    EventDelegate.Set(btnFriends.onClick, EventOpenFriendPopup);
    EventDelegate.Set(btnSlotMachine.onClick, EventOpenSlotMachine);
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
    btnSlotMachine.isEnabled = true;
  }
  
  public void EventUserLoggedOut() {
    usernameLabel.text = string.Empty;
    cashLabel.text = string.Empty;
    btnLeaderboard.isEnabled = false;
    btnFriends.isEnabled = false;
    btnSlotMachine.isEnabled = false;
  }
  
  private void EventOpenLeaderboard() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LEADERBOARD, null, true, true);
  }
  
  private void EventOpenFriendPopup() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_FRIENDS);
  }
  
  private void EventOpenSlotMachine() {
    // PopupManager.Instance.OpenPopup(Popup.Type.POPUP_SLOT_MACHINE);
  }
}
