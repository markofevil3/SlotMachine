using UnityEngine;
using System.Collections;

public class PopupInviteGameConfirm : Popup {

  public UIButton btnConfirm;
  public UIButton btnCancel;
  public UILabel messageLabel;
  
  private BaseSlotMachineScreen.GameType gameType;
  private string roomId;
  private string inviterName;
  
  public override void Init(object[] data) {
    base.Init(data);
    gameType = (BaseSlotMachineScreen.GameType)data[0];
    roomId = data[1].ToString();
    inviterName = data[2].ToString();
    messageLabel.text = data[3].ToString();
    EventDelegate.Set(btnConfirm.onClick, EventJoinGame);
    EventDelegate.Set(btnCancel.onClick, Close);
  }
  
  void EventJoinGame() {
    SlotMachineClient.Instance.JoinRoom(gameType, roomId);		
    Close();
  }
}
