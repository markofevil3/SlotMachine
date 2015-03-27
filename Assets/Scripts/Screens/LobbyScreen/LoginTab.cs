using UnityEngine;
using System.Collections;

public class LoginTab : Tab {
  
  public UIButton btnLogin;
  public UIButton btnClose;
  public UIInput usernameInput;
  public UIInput passwordInput;
  
  public override void Init() {
    EventDelegate.Set(btnLogin.onClick, EventLogin);
    EventDelegate.Set(btnClose.onClick, Close);
    // EventDelegate.Set(btnLoginFacebook.onClick, EventLoginFacebook);
  }
  
  public void ClearInput() {
    usernameInput.value = string.Empty;
    passwordInput.value = string.Empty;
  }
  
  private void EventLogin() {
    // SmartfoxClient.Instance.ManualConnect(usernameInput.value);
    if (usernameInput.value != string.Empty && passwordInput.value != string.Empty) {
      PopupManager.Instance.ShowLoadingPopup();
      SmartfoxClient.Instance.LoginUser(usernameInput.value.ToLower(), passwordInput.value);
    }
  }
}
