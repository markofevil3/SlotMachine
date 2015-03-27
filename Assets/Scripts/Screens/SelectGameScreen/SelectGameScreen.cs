using UnityEngine;
using System.Collections;

public class SelectGameScreen : BaseScreen {

  private string GAME_ITEM_PREFAB = Global.SCREEN_PATH + "/SelectGameScreen/GameItem";
  private UserBottomBar userBottomBar;

  public UIButton btnBack;
  public UIButton btnLeaderboard;
  public UIButton btnDailyBonus;
  public UIButton btnZombieGame;
  public UIButton btnDragonGame;
  public UIButton btnPirateGame;

  public override void Init(object[] data) {
    EventDelegate.Set(btnBack.onClick, BackToLobbyScreen);
    EventDelegate.Set(btnZombieGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN); });
    EventDelegate.Set(btnDragonGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_DRAGON); });
    EventDelegate.Set(btnPirateGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_PIRATE); });
    
    // GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.USER_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
    //    	tempGameObject.name = "UserBottomBar";
    //    	userBottomBar = tempGameObject.GetComponent<UserBottomBar>();
    //    	userBottomBar.Init(this);
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
