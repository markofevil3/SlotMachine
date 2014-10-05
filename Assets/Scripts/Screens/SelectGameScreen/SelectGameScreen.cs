using UnityEngine;
using System.Collections;

public class SelectGameScreen : BaseScreen {

  private string GAME_ITEM_PREFAB = Global.SCREEN_PATH + "/SelectGameScreen/GameItem";
  private UserBottomBar userBottomBar;

  public UITableExtent table;
  public UIButton btnBack;

  public override void Init(object[] data) {
    EventDelegate.Set(btnBack.onClick, BackToLobbyScreen);
    GameObject tempGameObject;
    for (int i = 0; i < (int)BaseGameScreen.GameType.TOTAL; i++) {
      tempGameObject = NGUITools.AddChild(table.gameObject, Resources.Load(GAME_ITEM_PREFAB, typeof(GameObject)) as GameObject);
      tempGameObject.name = i.ToString();
      UIEventTriggerExtent eventTrigger = tempGameObject.GetComponent<UIEventTriggerExtent>();
      eventTrigger.inputParams = new object[] {i};
      EventDelegate.Set(eventTrigger.onClick, delegate() { OpenSelectRoomScreen((int)eventTrigger.inputParams[0]); });
    }
    table.Reset();
    
    tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.USER_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
   	tempGameObject.name = "UserBottomBar";
   	userBottomBar = tempGameObject.GetComponent<UserBottomBar>();
   	userBottomBar.Init(this);
  }

  void BackToLobbyScreen() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LOBBY);
  }
  
  public override void Open() {
    
  }

  public override void Close() {
		ScreenManager.Instance.SelectGameScreen = null;
		base.Close();
	}

  private void OpenSelectRoomScreen(int type) {
    // Open Room Select Sreen, GameType param
    BaseGameScreen.GameType gameType = (BaseGameScreen.GameType) type;
    switch(gameType) {
      case BaseGameScreen.GameType.TIEN_LEN_MB:
        PopupManager.Instance.ShowLoadingPopup();
        TLMBClient.Instance.JoinLobby();
        // ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_ROOM, new object[]{(int)type});
      break;
    }
  }
}
