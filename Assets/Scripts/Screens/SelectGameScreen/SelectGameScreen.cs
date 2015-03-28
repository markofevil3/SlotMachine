using UnityEngine;
using System.Collections;

public class SelectGameScreen : BaseScreen {

  private string GAME_ITEM_PREFAB = Global.SCREEN_PATH + "/SelectGameScreen/GameItem";

  public UIButton btnBack;
  public UIButton btnLeaderboard;
  public UIButton btnDailyBonus;
  public UIButton btnZombieGame;
  public UIButton btnDragonGame;
  public UIButton btnPirateGame;
	
	public UILabel usernameLabel;
	public UILabel coinLabel;
	public UILabel goldLabel;

  public override void Init(object[] data) {
    EventDelegate.Set(btnBack.onClick, BackToLobbyScreen);
    EventDelegate.Set(btnZombieGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN); });
    EventDelegate.Set(btnDragonGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_DRAGON); });
    EventDelegate.Set(btnPirateGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_PIRATE); });
    EventDelegate.Set(btnLeaderboard.onClick, EventOpenLeaderboard);
    
		usernameLabel.text = AccountManager.Instance.username;
		coinLabel.text = AccountManager.Instance.cash.ToString("N0");
  }

  private void EventOpenLeaderboard() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LEADERBOARD, null, true, true);
  }

  void EventOpenSlotGame(BaseSlotMachineScreen.GameType type) {
    SlotMachineClient.Instance.JoinRoom(type);
  }

  void BackToLobbyScreen() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LOBBY);
  }

  public override void Close() {
		ScreenManager.Instance.SelectGameScreen = null;
		base.Close();
	}
}
