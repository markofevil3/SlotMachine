using UnityEngine;
using System.Collections;

public class PopupLeaveGame : Popup {
  
  public UIButton btnConfirm;
  public UIButton btnCancel;
  
  private BaseGameScreen.GameType gameType;
  
  public override void Init(object[] data) {
    base.Init(data);
    gameType = (BaseGameScreen.GameType)data[0];
    EventDelegate.Set(btnConfirm.onClick, EventLeaveGame);
    EventDelegate.Set(btnCancel.onClick, Close);
  }
  
  void EventLeaveGame() {
    SlotMachineClient.Instance.LeaveGame();
    Close();
  }
}
