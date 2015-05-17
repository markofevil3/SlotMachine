using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class RegisterTab : Tab {

  public UIButton btnRegister;
  public UIInput usernameInput;
  public UIInput passwordInput;
  public UIInput retypePasswordInput;
  public UIInput displayNameInput;
  
  public override void Init() {
    EventDelegate.Set(btnRegister.onClick, EventRegister);
  }

  void EventRegister() {
    if (usernameInput.value != string.Empty && passwordInput.value != string.Empty && retypePasswordInput.value != string.Empty && displayNameInput.value != string.Empty) {
      Utils.Log("EventRegister");
      JSONObject jsonData = new JSONObject();
      jsonData.Add("username", usernameInput.value);
      jsonData.Add("password", passwordInput.value);
      jsonData.Add("displayName", displayNameInput.value);
      AccountManager.Instance.Register(jsonData);
    }
  }
}
