using UnityEngine;
using System.Collections;

public class PopupCreateRoom : Popup {

  public UIButton btnCreateRoom;
  public UIInput roomNameInput;
  public UIInput roomPasswordInput;
  public UIPopupList betSelectPopupList;
  public UILabel betSelectLabel;
  
  private int[] minBetList = new int[] {2000, 5000, 10000, 100000, 1000000};
  
  
  public override void Init(object[] data) {
    base.Init(data);
    
    // Set Bet Filter
    for (int i = 0; i < minBetList.Length; i++) {
      betSelectPopupList.items.Add(minBetList[i].ToString("N0"));
    }

    EventDelegate.Set(btnCreateRoom.onClick, EventCreateRoom);
  }
  
	private void EventCreateRoom() {
		if (roomNameInput.value != string.Empty) {
			if (ScreenManager.Instance.SelectRoomScreen != null) {
				// create new room with fake ID
				TLMBClient.Instance.Create();
        // ScreenManager.Instance.SetScreen(BaseScreen.Type.GAME_SCREEN, new object[]{ScreenManager.Instance.SelectRoomScreen.gameType, 1000});
	    }
			Close();
		}
	}
}
