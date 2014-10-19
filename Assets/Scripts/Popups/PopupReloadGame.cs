using UnityEngine;
using System.Collections;

public class PopupReloadGame : Popup {

  public UIButton btnReload;
  
  public override void Init(object[] data) {
    base.Init(data);		
    EventDelegate.Set(btnReload.onClick, EventReloadGame);
	  PopupManager.Instance.CloseLoadingPopup();
  }
  
  void EventReloadGame() {
		SmartfoxClient.Instance.Restart();
  }
	
}
