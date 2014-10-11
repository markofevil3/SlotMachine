using UnityEngine;
using System.Collections;

public class SelectGameScreen : BaseScreen {

  private string GAME_ITEM_PREFAB = Global.SCREEN_PATH + "/SelectGameScreen/GameItem";
  private UserBottomBar userBottomBar;

  public UIButton btnBack;
  public UIButton btnFruitGame;
  public UIButton btnHalloweenGame;

  public override void Init(object[] data) {
    EventDelegate.Set(btnBack.onClick, BackToLobbyScreen);
    EventDelegate.Set(btnFruitGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_FRUITS); });
    EventDelegate.Set(btnHalloweenGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN); });
    
    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.USER_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
   	tempGameObject.name = "UserBottomBar";
   	userBottomBar = tempGameObject.GetComponent<UserBottomBar>();
   	userBottomBar.Init(this);
  }

  void EventOpenSlotGame(BaseSlotMachineScreen.GameType type) {
    SlotMachineClient.Instance.JoinRoom(type);
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
}
